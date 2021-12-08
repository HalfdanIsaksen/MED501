using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerFairy : MonoBehaviour
{
    // Start is called before the first frame update
    public GameManager gameManager;
    public AudioClip speak;
    private AudioSource fairyIntro, currentSpeak; 
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        fairyIntro = gameObject.GetComponent<AudioSource>();
        currentSpeak = gameObject.AddComponent<AudioSource>();

        //set the clip
        currentSpeak.clip = speak;

        StopAllCoroutines();
        StartCoroutine(FadeTracks(currentSpeak));
        
    }
    private IEnumerator FadeTracks(AudioSource speak)
    {
 
        
        float timeToFade = 4.00f;
        float timeElapsed = 0;

        //start speak clip a little delayed, after the Fairy sound has faded
        speak.PlayDelayed(2.0f);

     

        //while(timeElapsed < timeToFade)
        while(true)
        {
            //fade the two clips
            fairyIntro.volume = Mathf.Lerp(0.3f, 0.005f, timeElapsed / timeToFade); 
            timeElapsed += Time.deltaTime;

            //Debug.Log(timeElapsed);

            if(!speak.isPlaying)
            {
                gameManager.isSomeoneSpeaking = false;
                //reference to the fade of Fairy gameobjects
                //StartCoroutine(gameObject.GetComponentInChildren<Fade>().FadeFairy());
                StartCoroutine(FairyDie());
                
            } 
            yield return null;
        }            
    } 
    private IEnumerator FairyDie()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
