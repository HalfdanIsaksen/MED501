using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
/*Check if it can be put out on the player so the script doesn't
need to be on both hands but can control both hands at the same time
see ShowController script*/


public class HandsController : MonoBehaviour
{
    //distance to object that is gonna be picked up
    public float distToPickup = 0.2f;
    // boolean to check if the hand is closed or not
    public bool handClosed = false;
    //Layermask to check if the object we are trying to pickup is one we can pick up
    public LayerMask pickupLayer;
    //Or hand that is used
    public SteamVR_Input_Sources handSource;
    //Obejct we are holding in or hand
    Rigidbody objectHolding;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate(){

    }
}
