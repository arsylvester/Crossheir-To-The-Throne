using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] Text tripleKill;
    [SerializeField] Text collateral;
    Queue<Text> txtFeed = new Queue<Text>();
    bool isPrintingFeed = false;

    void Start()
    {
        tripleKill.enabled = false;
        collateral.enabled = false;
    }

    void LateUpdate()
    {
        if (txtFeed.Count != 0 && !isPrintingFeed)
        {
            isPrintingFeed = true;
            Text i = txtFeed.Dequeue();
            StartCoroutine(textFeedHelper(i));
        }
    }

    public IEnumerator textFeedHelper(Text txt) //I can't think of a better way to do this right now.
    {
        StartCoroutine(fadeInOut(txt));
        yield return new WaitForSecondsRealtime(0.8f);
        isPrintingFeed = false;
    }

    public IEnumerator fadeInOut(Text txt)
    {
        txt.enabled = true;
        txt.color = new Color(1f, 0.8660925f, 0.2311321f, 0); //set to 0 alpha
        txt.rectTransform.position = new Vector3(0,0,0);

        float alpha;
        float duration = 0.15f;
        int finalOffsetY = 35;
        int startPosY = 25;
        int posY;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            alpha = t / duration; //alpha equals the loops completion percent
            posY = startPosY + (int)(alpha * finalOffsetY);
            txt.color = new Color(1f, 0.8660925f, 0.2311321f, alpha);
            txt.rectTransform.localPosition = new Vector3(0, posY, 0);

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSecondsRealtime(1f);

        alpha = 1f;
        duration = 0.5f;
        for (float t = 0f; t < duration; t += Time.deltaTime)
        {
            alpha = 1 - (t / duration);
            txt.color = new Color(1f, 0.8660925f, 0.2311321f, alpha);

            yield return new WaitForEndOfFrame();
        }

        txt.enabled = false;
    }
    public void DisplayTripleKill()
    {
        //StartCoroutine(fadeInOut(tripleKill));
        txtFeed.Enqueue(tripleKill);
    }

    public void DisplayCollateral()
    {
        //StartCoroutine(fadeInOut(collateral));
        txtFeed.Enqueue(collateral);
    }
}
