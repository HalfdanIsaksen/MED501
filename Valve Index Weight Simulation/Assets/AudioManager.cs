using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource intro, getToWork, sequenceChange, testEnd, footSteps;

    private bool introPlayed = false;
    private bool getToWorkPlayed = false;
    private bool sequenceChangePlayed = false;
    private bool testEndPlayed = false;

    private bool footStepsPlayed = false;

    float speakDelayTime = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        intro = this.transform.Find("intro").gameObject.GetComponent<AudioSource>();
        getToWork = this.transform.Find("getToWork").gameObject.GetComponent<AudioSource>();
        sequenceChange = this.transform.Find("sequenceChange").gameObject.GetComponent<AudioSource>();
        testEnd = this.transform.Find("testEnd").gameObject.GetComponent<AudioSource>();
        footSteps = this.transform.Find("footSteps").gameObject.GetComponent<AudioSource>();
       
        //PlayTestEnd();
        //Debug.Log(intro.clip.length);
        
    }

    public void PlayIntro()
    {
        if(introPlayed == false)
        {
            intro.PlayDelayed(speakDelayTime);
            introPlayed = true;
        }          
    }

    public void PlayGetToWork()
    {
        if(getToWorkPlayed == false)
        {
            getToWork.PlayDelayed(speakDelayTime);
            getToWorkPlayed = true;
        } 
    }

    public void PlaySequenceChange()
    {
        if(sequenceChangePlayed == false)
        {
            sequenceChange.PlayDelayed(speakDelayTime);
            sequenceChangePlayed = true;
        }
        
    }

    public void PlayTestEnd()
    {
        if(testEndPlayed == false)
        {
            testEnd.PlayDelayed(speakDelayTime);
            testEndPlayed = true;
        }
        
    }

    public void PlayFootsteps(){

        if(footStepsPlayed == false){
            footSteps.Play();
            footStepsPlayed = true;
        }
    }
    public void setFootStepsPlayed(bool _footSteps){
        footStepsPlayed = _footSteps;
    }

}
