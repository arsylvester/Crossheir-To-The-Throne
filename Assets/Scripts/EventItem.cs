using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventItem : MonoBehaviour
{
    [SerializeField] Text eventName;
    [SerializeField] Text eventMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        //eventMultiplier.text = "";
    }

    public void SetName(string n)
    {
        eventName.text = n;
    }

    public void SetMultiplier(string m)
    {
        eventMultiplier.text = "x" + m;
    }

    public void EnableMultiplier(bool x)
    {
        eventMultiplier.enabled = x;
    }

    public IEnumerator FadeOut(float pause, float duration) //fade out name and multiplier text over a duration, then destroy the gameObject.
    {
        GameObject temp = gameObject; //we need a temp reference in case the real object is destroyed mid loop
        eventName.color = new Color(1f, 0.8660925f, 0.2311321f, 1); //set to 0 alpha
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
