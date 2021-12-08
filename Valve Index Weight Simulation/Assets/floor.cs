using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floor : MonoBehaviour
{
    public GameManager gameManager;
    public tutorial tutorialManager;
    public Transform fracturedBottle;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other){
        if(other.tag == "InteractionObejct"){
            Instantiate(fracturedBottle, other.transform.position, Quaternion.identity);
            Destroy(other.transform.gameObject);
            gameManager.isSpawned = false;
            if(gameManager.tutorial){
                tutorialManager.bottlesInCrate ++;
                if(tutorialManager.bottlesInCrate > 2){
                    gameManager.tutorial = false;
                    gameManager.spawnBottles();
                    tutorialManager.runOurEnumerator();
                }
            Debug.Log("bottles in crate: " + tutorialManager.bottlesInCrate);
            }else{
                gameManager.isDestroyedOnFloor();
            }
        }
    }
}
