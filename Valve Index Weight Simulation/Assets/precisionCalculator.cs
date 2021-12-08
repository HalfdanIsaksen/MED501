using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class precisionCalculator : MonoBehaviour
{
    public double distance, totalDistance;

    public GameManager gameManager;
    public bool isDone = false;
    public Hand handsSource;
    public bool ifPlaced = true;


    public Material circleMaterial;
    public Color circleBaseColor;

    public float precisionValue = 0.0001f;
    public GameObject fakeBottle;
    public GameObject bottle;

    public GameObject scoreDisplay;

    public Texture[] scoreTexture = new Texture[10];
    
    void Start(){
        circleMaterial = GetComponent<Renderer>().material;
        circleBaseColor = circleMaterial.color;
        scoreDisplay.SetActive(true);
    }

    void OnTriggerEnter(Collider other){
        ifPlaced = true;
        //checks if the object colliding has the tag InteractionObject on it
        if(other.tag == "InteractionObejct"){
            //our bottle object is set to be equals the obejct that has entered the trigger
            bottle = other.transform.gameObject;

            if(!handsSource.ObjectIsAttached(bottle)){
                //calculates the distance from our placeing circles centerpoint to our objects centerpoint
                distance = Math.Sqrt(((transform.position.x - other.transform.position.x)*(transform.position.x - other.transform.position.x))*((transform.position.z - other.transform.position.z)*(transform.position.z - other.transform.position.z)));
                totalDistance = totalDistance + distance;
                //Debug.Log("Distance: " + distance);
                //if the distance is less than our Precision value
                if(distance < precisionValue){
                    
                    ifPlaced = true;
                    //if placed run method onPlace
                    if(ifPlaced){
                        onPlace(bottle);
                    }
                    
                }
            }
        }
    }
    public void onPlace(GameObject bottle){
        //handsSource.DetachObject(bottle);
        //set our bottles that are placed to be plus one and ispawned to false
        gameManager.bottlesPlaced ++;
        gameManager.isSpawned = false;
        //make our bottle stand still where it is
        bottle.GetComponent<Rigidbody>().useGravity = false;
        //(bottle.GetComponent<Rigidbody>().isKinematic = true;
        gameManager.setDataSheet();

        gameManager.data.precision = totalDistance/gameManager.data.regrasp;
        
        if(gameManager.bottlesPlaced < 9){
            gameManager.WriteCSV();
        }
        //gameManager.data.variableGrip = 0;
        //if(gameManager.fairyTask){
            gameManager.spawnBottles();
        //}
        ifPlaced = false;
        StartCoroutine(instantiateFakeBottle());
        destroyPlacedObject();
        //gameManager.WriteCSV();
    }
    
    // Destroys bottle
    public void destroyPlacedObject(){
        Destroy(bottle);
    }

    public void changeScore(){
        Renderer scoreRenderer = scoreDisplay.GetComponent<Renderer>();
        scoreRenderer.material.SetTexture("_MainTex", scoreTexture[gameManager.bottlesPlaced]);
    }

    IEnumerator instantiateFakeBottle(){
        circleMaterial.color = Color.green;
        // Displays FakeBottle, by turning on display/physical appearance and the collider
        fakeBottle.GetComponent<MeshCollider>().enabled = true;
        fakeBottle.GetComponent<MeshRenderer>().enabled = true;
        changeScore();
        yield return new WaitForSeconds(1);
        circleMaterial.color = circleBaseColor;
        // Hides FakeBottle, turns off display/physical appearance and collider
        fakeBottle.GetComponent<MeshCollider>().enabled = false;
        fakeBottle.GetComponent<MeshRenderer>().enabled = false;
        //fakeBottle.SetActive(false);
    }

}
