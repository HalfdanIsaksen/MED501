using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{
    public float fadeSpeedMat1 = 12f;  

    public IEnumerator FadeFairy()
    {
        Color objectColor1 = this.GetComponent<SkinnedMeshRenderer>().materials[0].color;

        float fadeAmountMat1 = objectColor1.a;

        while(fadeAmountMat1 > 0)
        {
            fadeAmountMat1 = objectColor1.a - (fadeSpeedMat1 * Time.deltaTime);
            objectColor1 = new Color(objectColor1.r, objectColor1.g, objectColor1.b, fadeAmountMat1);
            this.GetComponent<SkinnedMeshRenderer>().materials[0].color = objectColor1;

            if(objectColor1.a <= 0)
            {
                Destroy(gameObject);
            }
            yield return null;
        }        
        
    }

}
