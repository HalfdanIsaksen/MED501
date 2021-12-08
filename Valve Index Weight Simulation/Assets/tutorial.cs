using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    public GameManager gameManager;
    public int bottlesInCrate = 0;

    public GameObject[] bottles = new GameObject[3];

    void Update(){

    }
    void OnTriggerEnter(Collider other){
        if(other.tag == "InteractionObejct"){
            bottles[bottlesInCrate] = other.transform.gameObject;
            bottlesInCrate ++;
            other.transform.gameObject.tag = "Finish";
            if(bottlesInCrate > 2){
                StartCoroutine(destroyTutorialBottles());
                gameManager.tutorial = false;
                gameManager.task = true;
                this.gameObject.GetComponent<Collider>().enabled = false;
                gameManager.data.variableGrip = 0;
            }
        }
        Debug.Log("bottles in crate: " + bottlesInCrate);
    }

    public void runOurEnumerator(){
        StartCoroutine(destroyTutorialBottles());
    }

    IEnumerator destroyTutorialBottles(){

        yield return new WaitForSeconds(2);
        for(int i = 0; i < bottles.Length; i++){
            Destroy(bottles[i].gameObject);
        }
    }
}
