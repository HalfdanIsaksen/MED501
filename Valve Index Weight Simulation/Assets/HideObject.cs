using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;
public class HideObject : MonoBehaviour
{
    public GameObject childObject;
    // Update is called once per frame
    void Update()
    {
        foreach (var handsInVR in Player.instance.hands){
            if(handsInVR.ObjectIsAttached(this.gameObject)){
                MeshRenderer meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
                Collider thisCollider = this.gameObject.GetComponent<Collider>();
                thisCollider.enabled = false;
                meshRenderer.enabled = false;

                childObject =  GameObject.Find("FakeItChild");
                childObject.SetActive(true);
            }
        }
    }
}
