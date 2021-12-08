using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
using System;


public class Squeeze : MonoBehaviour
{
    //Variable to get the input we want from the Valve Controller
    public SteamVR_Action_Single squeezeInput;
    //Rigidbody to hold the prefeb we want instatiated
    public Rigidbody prefab;

    public float minimumDampning = 0.0f;
    public float maximumDampning = 0.35f;
    public float minimumSqueezeInput = 0.0f;
    public float maximumSqueezeInput = 0.5f;
    //max default 0.98
    public bool isFirm, isLoose;

    public ComplexThrowable complexControl;
    public bool c1,c2;

    public GameManager gameManager;


    void Start(){
        isLoose = false;
        isFirm = false;
        prefab = GetComponent<Rigidbody>();
        
        complexControl = this.gameObject.GetComponent<ComplexThrowable>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {   
        if(!c1){
            //Goes through the hands of the player.
            foreach (var handsInVR in Player.instance.hands){
                //if one of the players hands is holding this oobject
                if(handsInVR.ObjectIsAttached(this.gameObject)){
                    if(!c2){
                        float squeezeMappedToDamp = (squeezeInput.axis - minimumSqueezeInput)/(maximumSqueezeInput-minimumSqueezeInput) * (maximumDampning - minimumDampning);
                        complexControl.positionDamper = squeezeMappedToDamp;

                    }else if(c2){
                        complexControl.positionDamper = 0.03f;
                    }
                    if(squeezeInput.axis > 0.6f){
                        prefab.useGravity = false;
                        prefab.constraints = RigidbodyConstraints.FreezeRotation;
                        this.gameObject.transform.parent = handsInVR.transform;
                        if(!isFirm){
                            gameManager.data.variableGrip ++;
                            isFirm = true;
                            isLoose = false;
                        }
                        
                    }else if(squeezeInput.axis < 0.6f){
                        this.gameObject.transform.parent = null;

                        prefab.useGravity = true;
                        prefab.constraints = RigidbodyConstraints.None;
                        if(!isLoose){
                            gameManager.data.variableGrip ++;
                            isLoose = true;
                            isFirm = false;
                        }
                    }
                }
            }
        }
    }
}
