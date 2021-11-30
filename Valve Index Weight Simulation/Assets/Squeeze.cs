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
    public Transform controllerAnchor;
    public ConfigurableJoint myJoint;
    public Vector3 anchor = new Vector3(0.0f,0.0f,0.0f);

    public float minimumDampning = 0.0f;
    public float maximumDampning = 0.35f;
    public float minimumSqueezeInput = 0.0f;
    public float maximumSqueezeInput = 0.98f;

    public ComplexThrowable complexControl;
    public bool isAdjustable;




    void Start(){
        prefab = GetComponent<Rigidbody>();

        complexControl = this.gameObject.GetComponent<ComplexThrowable>();

        /*if(isAdjustable){
            this.gameObject.AddComponent<ConfigurableJoint>();
            myJoint = GetComponent<ConfigurableJoint>();
            myJoint.autoConfigureConnectedAnchor = false;
            myJoint.configuredInWorldSpace = false;
            myJoint.connectedAnchor = Vector3.zero;
            myJoint.anchor = this.transform.InverseTransformPoint(controllerAnchor.position);
            myJoint.rotationDriveMode = RotationDriveMode.Slerp;
            JointDrive myJointDrive = myJoint.slerpDrive;
            myJointDrive.positionDamper = positionDamper;
            myJoint.slerpDrive = myJointDrive;
        }*/
    }

    // Update is called once per frame
    void Update()
    {   
        //Goes through the hands of the player.
        foreach (var handsInVR in Player.instance.hands){
            //if one of the players hands is holding this oobject
            if(handsInVR.ObjectIsAttached(this.gameObject)){
                if(isAdjustable){
                    float squeezeMappedToDamp = (squeezeInput.axis - minimumSqueezeInput)/(maximumSqueezeInput-minimumSqueezeInput) * (maximumDampning - minimumDampning);
                    //ease dampinging in and out using a parametric function
                    //float sqrtSqueezeMap = squeezeMappedToDamp * squeezeMappedToDamp;
                    //float parametricDampning = sqrtSqueezeMap / (2.0f *(sqrtSqueezeMap - squeezeMappedToDamp) + 1.0f);
                    
                    complexControl.positionDamper = squeezeMappedToDamp;
                }
                if(squeezeInput.axis > 0.97f){
                    if(isAdjustable){
                        prefab.useGravity = false;
                        prefab.constraints = RigidbodyConstraints.FreezeRotation;
                    }
                }else if(squeezeInput.axis < 0.97f){
                    prefab.useGravity = true;
                    prefab.constraints = RigidbodyConstraints.None;
                }
            }
        }
    }
}
