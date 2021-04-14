using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventItem : MonoBehaviour
{
    [SerializeField] Text eventName;
    [SerializeField] Text eventMultiplier;
    public bool hasMultiplier;

    public void SetName(string n)
    {
        eventName.text = n;
    }

    public void SetMultiplier(string m)
    {
        eventMultiplier.text = "x" + m;
    }

    public IEnumerator FadeIn(float in_duration, float pause, float out_duration) /*Fade in text over in_duration, then FadeOut with pause and out_duration*/
    {
        GameObject temp = gameObject; //we need a temp reference in case the real object is destroyed mid loop
        eventMultiplier.enabled = hasMultiplier;

        //play a spawn animation
        eventName.fontSize = 50;

        int size = 50;
        for (float t = 0f; t < in_duration; t += Time.deltaTime)
        {
            if (temp == null)
                yield break; //check to make sure the object wasn't deleted already

            size = 50 - (int)(10 * (t / in_duration)); //goes from 50 to 40
            eventName.fontSize = size;
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(0.22f);

        /*
        //Play a seperate animation for the multiplier text.

        if (hasMultiplier)
        {
            //eventMultiplier.enabled = true;

            //play a seperate animation for the multiplier
            for (float t = 0f; t < in_duration; t += Time.deltaTime)
            {
                size = 50 - (int)(10 * (t / in_duration)); //goes from 50 to 40
                eventMultiplier.fontSize = size;
                yield return new WaitForEndOfFrame();
            }
        }
        */

        if (temp == null)
            yield break; //do it again

        //call fade out because I can't think of a better way to do this
        StartCoroutine(FadeOut(pause, out_duration));
    }

    public IEnumerator FadeOut(float pause, float duration) /*fade out name and multiplier text over a duration, then destroy the gameObject.*/
    {
        GameObject temp = gameObject; //we need a temp reference in case the real object is destroyed mid loop
        eventName.color = new Color(1f, 0.8660925f, 0.2311321f, 1); //set to 1 alpha
        eventMultiplier.color = eventName.color;

        yield return new WaitForSecondsRealtime(pause);

        float alpha = 1f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            if (temp == null)
                yield break; //check to make sure the object wasn't deleted already

            alpha = 1 - (t / duration);
            eventName.color = new Color(1f, 0.8660925f, 0.2311321f, alpha);
            eventMultiplier.color = eventName.color;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
