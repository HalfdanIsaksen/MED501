using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class precisionCalculator : MonoBehaviour
{
    public double distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        if(other.tag == "InteractionObejct"){
            distance = Math.Sqrt(((transform.position.x - other.transform.position.x)*(transform.position.x - other.transform.position.x))*((transform.position.z - other.transform.position.z)*(transform.position.z - other.transform.position.z)));
            Debug.Log("Distance: " + distance);
        }
    }
}
