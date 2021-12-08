using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Valve.VR.InteractionSystem;

public class GameManager : MonoBehaviour
{
    public Transform spawnPointBookShelf;
    public Transform spawnPointFirePlace;
    public Transform spawnPointBox;
    public Transform bottle;

    public Transform[] spawnPointsTutorial = new Transform[3];
    public Transform[] bottleTutorial = new Transform[3];

    public Collider tutorialCollider;

    public GameObject magicCircle;
    public GameObject scoreDisplay;

    public int bottlesPlaced = 0;

    public bool isSpawned = false;

    public bool tutorial = true;

    public ComplexThrowable complexThrowable;



    public bool firstCondition, intro, endOfTest, c1, c2, c3, end, task = true, fairyTask = true, fairyGuiding, isFairySayingGoodJob = true, vizartsSayinggoodbye = false;

    public Transform fairyIntro, fairyHeIsComing, fairyFireplace, fairyBox, fairyGoodjob, fairyFirstC1, fairyFirstC2, fairyFirstC3, fairyC1, fairyC2, fairyC3;

    public Transform wizard, door;

    public bool isSomeoneSpeaking = false;

    public bool isFootStepsPlaying, isWizardIntroPlaying;

    //Variables for date logging
    public float timeFromStart, tutorialTime;

    public string filename = "";
    [System.Serializable]
    public class Data{
        public string name;
        public string condition;
        
        public string bottlePlacement;
        public int regrasp;

        public double precision;
        public int variableGrip;
        public float time;
    }

    public Data data = new Data();

    void Start(){
        //timeFromStart = Time.realtimeSinceStartup;
        filename = Application.dataPath + "/data" + data.condition + ".csv";
        if(firstCondition){
            fairyTask = false;
        }
        if(intro){
            StartCoroutine(fairyIntroSpawn());
        }else{
            spawnBottles();
        }
    }
    // Update is called once per frame
    void Update()
    {
        sequenceManagement();

        if(bottlesPlaced <= 2){
            data.bottlePlacement = "H1";
        }else if(3 <= bottlesPlaced && bottlesPlaced <= 5){
            data.bottlePlacement = "H2";
        }else if(6 <= bottlesPlaced && bottlesPlaced <= 9){
           data.bottlePlacement = "H3";
           if(bottlesPlaced == 8){
               //data.time = timeFromStart - tutorialTime;
           }
        }
    }

    public void spawnBottles(){
        intro = false;
        
        if(tutorial){
            isFootStepsPlaying = true;
            isSomeoneSpeaking = true;
            cleaning();
        }else{
            tutorialCollider.enabled = false;
            if(c1){
                bottle.GetComponent<Squeeze>().c1 = true;
                bottle.GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.FixedJoint;
                bottle.GetComponent<ComplexThrowable>().attachmentFlags = Hand.AttachmentFlags.SnapOnAttach;
            }else if(c2){
                bottle.GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.ConfigurableJoint;
                bottle.GetComponent<ComplexThrowable>().attachmentFlags = 0;
                bottle.GetComponent<Squeeze>().c1 = false;
                bottle.GetComponent<Squeeze>().c2 = true;
            }else if(c3){
                bottle.GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.ConfigurableJoint;
                bottle.GetComponent<ComplexThrowable>().attachmentFlags = 0;
                bottle.GetComponent<Squeeze>().c1 = false;
                bottle.GetComponent<Squeeze>().c2 = false;
            }
                magicCircle.SetActive(true);
                scoreDisplay.SetActive(true);
                Transform bottleClone;
            if(bottlesPlaced < 3){
                if(!isSpawned){
                    data.variableGrip = 0;
                    bottleClone = Instantiate(bottle, spawnPointBookShelf.position, Quaternion.identity);
                    complexThrowable = bottleClone.GetComponent<ComplexThrowable>();
                    //data.bottlePlacement = "H1";
                    isSpawned = true;
                }
            }else if(2 < bottlesPlaced && bottlesPlaced < 6){
                if(!isSpawned){
                    data.variableGrip = 0;
                    bottleClone = Instantiate(bottle, spawnPointFirePlace.position, Quaternion.identity);
                    complexThrowable = bottleClone.GetComponent<ComplexThrowable>();
                    //data.bottlePlacement = "H2";
                    isSpawned = true;
                }
            }else if(5 < bottlesPlaced && bottlesPlaced < 9){
                if(!isSpawned){
                    data.variableGrip = 0;
                    bottleClone = Instantiate(bottle, spawnPointBox.position, Quaternion.identity);
                    complexThrowable = bottleClone.GetComponent<ComplexThrowable>();
                    //data.bottlePlacement = "H3";
                    isSpawned = true;
                }
            }else if( bottlesPlaced > 8){
                Debug.Log("End is near");
                end = true;
                //data.time = timeFromStart - tutorialTime;
                //WriteCSV();
            }
        }
        //Debug.Log(bottlesPlaced);
    }

    public void cleaning(){
        magicCircle.SetActive(false);
        scoreDisplay.SetActive(false);
        for(int i = 0; i < spawnPointsTutorial.Length; i++){
            bottleTutorial[i] = Instantiate(bottle, spawnPointsTutorial[i].position, Quaternion.identity);
            if(c1){
                bottleTutorial[i].GetComponent<Squeeze>().c1 = true;
                bottleTutorial[i].GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.FixedJoint;
                bottleTutorial[i].GetComponent<ComplexThrowable>().attachmentFlags = Hand.AttachmentFlags.SnapOnAttach;
            }else if(c2){
                bottleTutorial[i].GetComponent<Squeeze>().c1 = false;
                bottleTutorial[i].GetComponent<Squeeze>().c2 = true;
                bottleTutorial[i].GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.ConfigurableJoint;
                bottle.GetComponent<ComplexThrowable>().attachmentFlags = 0;
            }else if(c3){
                bottleTutorial[i].GetComponent<Squeeze>().c1 = false;
                bottleTutorial[i].GetComponent<Squeeze>().c2 = false;
                bottleTutorial[i].GetComponent<ComplexThrowable>().attachMode = ComplexThrowable.AttachMode.ConfigurableJoint;
                bottle.GetComponent<ComplexThrowable>().attachmentFlags = 0;
            }
        }
    }

    public void isDestroyedOnFloor(){
        Transform bottleClone;
        if(bottlesPlaced < 3){
            if(!isSpawned){
            bottleClone = Instantiate(bottle, spawnPointBookShelf.position, Quaternion.identity);
            isSpawned = true;
            }
        }else if(2 < bottlesPlaced && bottlesPlaced < 6){
            if(!isSpawned){
            bottleClone = Instantiate(bottle, spawnPointFirePlace.position, Quaternion.identity);
            isSpawned = true;
            }
        }else if(5 < bottlesPlaced && bottlesPlaced < 9){
            if(!isSpawned){
            bottleClone = Instantiate(bottle, spawnPointBox.position, Quaternion.identity);
            isSpawned = true;
            }
        }
    }
    public void setTutorialTime(){
        tutorialTime = timeFromStart;
    }

    public void sequenceManagement(){
        if(intro){
            if(!isSomeoneSpeaking){
                if(!isFootStepsPlaying){
                    wizard.GetComponent<AudioManager>().PlayFootsteps();
                    if(wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        door.transform.Rotate(0,  10 * Time.deltaTime, 0, Space.Self);
                    }else{
                        isFootStepsPlaying = true;
                    }
                }else if(!isWizardIntroPlaying){
                    //Open door first before playing
                    //play wizard intro
                    if(!wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        wizard.GetComponent<AudioManager>().PlayIntro();
                        wizard.GetComponent<AudioManager>().setFootStepsPlayed(false);
                        
                        if(!wizard.GetComponent<AudioManager>().intro.isPlaying){
                            door.transform.rotation = Quaternion.Euler(0,0,0);
                            spawnBottles();
                        }
                    }
                }
            }
        }else if(tutorial){
            if(firstCondition){
                if(isSomeoneSpeaking){
                    if(c1){
                        Instantiate(fairyFirstC1, fairyFirstC1.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }else if(c2){
                        Instantiate(fairyFirstC2, fairyFirstC2.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }else if(c3){
                        Instantiate(fairyFirstC3, fairyFirstC3.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }
                }
            }else{
                if(isSomeoneSpeaking){
                    if(c1){
                        Instantiate(fairyC1, fairyC1.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }else if(c2){
                        Instantiate(fairyC2, fairyC2.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }else if(c3){
                        Instantiate(fairyC3, fairyC3.position, Quaternion.identity);
                        isSomeoneSpeaking = false;
                    }
                }
            }
        }else if(task){
            if(firstCondition){
                Debug.Log("FirstCon");
                if(!fairyTask){
                    Debug.Log("not fairyTask");
                    if(!isSomeoneSpeaking){
                        Debug.Log("not no man speaking");
                        if(isFootStepsPlaying){
                            Debug.Log("Is ma footsteps creaping");
                            wizard.GetComponent<AudioManager>().PlayFootsteps();
                            if(wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                                door.transform.Rotate(0,  10 * Time.deltaTime, 0, Space.Self);
                            }else if(!wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                                isFootStepsPlaying = false;
                            }
                        }else{
                            wizard.GetComponent<AudioManager>().PlayGetToWork();
                            isSomeoneSpeaking = true;
                        }
                    }else if(!wizard.GetComponent<AudioManager>().getToWork.isPlaying){
                        wizard.GetComponent<AudioManager>().setFootStepsPlayed(false);
                        door.transform.rotation = Quaternion.Euler(0,0,0);
                        data.time = Time.time;
                        spawnBottles();
                        fairyTask = true;
                    }
                }else{
                    if(bottlesPlaced == 3){
                        if(!fairyGuiding){
                            Instantiate(fairyFireplace, fairyFireplace.position, Quaternion.identity);
                            fairyGuiding = true;
                        }
                    }else if(bottlesPlaced == 6){
                        if(fairyGuiding){
                            Instantiate(fairyBox, fairyBox.position, Quaternion.identity);
                            fairyGuiding = false;
                            isFootStepsPlaying = true;
                        }
                    }else if(bottlesPlaced == 9){
                        
                        WriteCSV();
                        task = false;
                        end = true;
                    }
                }
            }else{
                if(isFairySayingGoodJob){
                    Instantiate(fairyGoodjob, fairyGoodjob.position, Quaternion.identity);
                    setTutorialTime();
                    data.time = Time.time;
                    spawnBottles();
                    isFairySayingGoodJob = false;
                }else if(bottlesPlaced == 3){
                    if(!fairyGuiding){
                        Instantiate(fairyFireplace, fairyFireplace.position, Quaternion.identity);
                        fairyGuiding = true;
                    }
                }else if(bottlesPlaced == 6){
                    if(fairyGuiding){
                        Instantiate(fairyBox, fairyBox.position, Quaternion.identity);
                        fairyGuiding = false;
                        isFootStepsPlaying = true;
                    }
                }else if(bottlesPlaced == 9){
                    WriteCSV();
                    task = false;
                    end = true;
                }
            }
        }else if(end){
            Debug.Log("Time: " + data.time);
            Debug.Log("Tutorial Time:" + tutorialTime);
            if(endOfTest){
                if(isFootStepsPlaying){
                    wizard.GetComponent<AudioManager>().PlayFootsteps();
                    if(wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        door.transform.Rotate(0,  10 * Time.deltaTime, 0, Space.Self);
                    }else if(!wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        isFootStepsPlaying = false;
                    }
                }else{
                    if(!wizard.GetComponent<AudioManager>().testEnd.isPlaying && !vizartsSayinggoodbye){
                        wizard.GetComponent<AudioManager>().PlayTestEnd();
                        vizartsSayinggoodbye = true;
                    }
                }
            }else{
                if(isFootStepsPlaying){
                    wizard.GetComponent<AudioManager>().PlayFootsteps();
                    if(wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        door.transform.Rotate(0,  10 * Time.deltaTime, 0, Space.Self);
                    }else if(!wizard.GetComponent<AudioManager>().footSteps.isPlaying){
                        isFootStepsPlaying = false;
                    }
                }else{
                    if(!wizard.GetComponent<AudioManager>().sequenceChange.isPlaying && !vizartsSayinggoodbye){
                        wizard.GetComponent<AudioManager>().PlaySequenceChange();
                        vizartsSayinggoodbye = true;
                    }
                }

            }
            
        }

    }

    public void WriteCSV()
    {
        TextWriter tw = new StreamWriter(filename, true);

            tw.WriteLine(data.name + ';' + 
                            data.condition + ';' +
                            data.bottlePlacement + ';' + 
                            data.precision + ';' +
                            data.regrasp + ';' +
                            data.variableGrip + ';' +
                            data.time);
        tw.Close();
    }

    public void setDataSheet(){
        data.regrasp = complexThrowable.regrasp;
    }

    private IEnumerator fairyIntroSpawn(){
        isSomeoneSpeaking = true;
        yield return new WaitForSeconds(3.0f);
        Instantiate(fairyIntro, fairyIntro.position, Quaternion.identity);
    }
}
