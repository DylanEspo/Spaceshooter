using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;

    [SerializeField] RectTransform myRectTransform;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if(myRectTransform != null)
        {
            myRectTransform = GetComponent<RectTransform>();
        }
    }

    IEnumerator FadeOutIn()
    {
        yield return FadeOut(3f);
        Debug.Log("Faded out");
        yield return FadeIn(2f);
        Debug.Log("Faded in");
    }

    public void FadeOutImmediate()
    {
        canvasGroup.alpha = 1;
    }


     public IEnumerator FadeOut(float pTime)
     {
        //For every frame update the alpha a certain amount
        while (canvasGroup.alpha < 1) //alpha is not 1
        {
            // moving alpha towards 1
            //n = time/deltaTime
            //delataAlpha = 1/n
            //deltaAlpha = 1 * deltaTime /time
            canvasGroup.alpha += Time.deltaTime / pTime;
            yield return null;
         }
     }

     public IEnumerator FadeIn(float pTime)
     {
        //For every frame update the alpha a certain amount
        while (canvasGroup.alpha > 0) //alpha is not 1
        {
            // moving alpha towards 1
            //n = time/deltaTime
            //delataAlpha = 1/n
            //deltaAlpha = 1 * deltaTime /time
            canvasGroup.alpha -= Time.deltaTime / pTime;
            yield return null;
        }
     }
}
