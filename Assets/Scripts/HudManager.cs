using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    [SerializeField] Text tripleKill;
    [SerializeField] Text collateral;
    [SerializeField] GameObject HUD;
    [SerializeField] GameObject PauseMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject ammoCluster;
    [SerializeField] GameObject xhair;
    private int currentXhair;

    private GameObject loadedCluster;
    private RawImage[] chamberIcons;

    private Queue<Text> txtFeed = new Queue<Text>();
    private bool isPrintingFeed = false;
    private bool isMinMode = false;

    void Start()
    {
        tripleKill.enabled = false;
        collateral.enabled = false;
        isMinMode = MenuManager.getMinMode();

        loadedCluster = ammoCluster.transform.GetChild(1).gameObject;

        if (isMinMode) //remove the empty chamber images when in minmode
            goMinMode();

        //get the icons for each of the 3 rounds
        chamberIcons = loadedCluster.GetComponentsInChildren<RawImage>();
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

    private void goMinMode()
    {
        //disable the empty chamber images
        ammoCluster.transform.GetChild(0).gameObject.SetActive(false);

        ammoCluster.GetComponent<RectTransform>().localPosition = new Vector3(0, -69, 0); //nice

        ammoCluster.GetComponent<RectTransform>().localScale = new Vector3(0.1f, 0.1f, 1f);
    }

    private void exitMinMode()
    {
        ammoCluster.transform.GetChild(0).gameObject.SetActive(true);

        ammoCluster.GetComponent<RectTransform>().localPosition = new Vector3(0, 270, 0);

        ammoCluster.GetComponent<RectTransform>().localScale = new Vector3(0.6f, 0.6f, 1f);
    }

    public void setXhairSize(float s)
    {
        xhair.transform.GetChild(currentXhair).GetComponent<RectTransform>().localScale = new Vector3(s, s, 1);
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

    public void updateAmmo(int currentAmmo)
    {
        int c = 0;

        chamberIcons[0].enabled = false;
        chamberIcons[1].enabled = false;
        chamberIcons[2].enabled = false;

        while (c < currentAmmo)
        {
            chamberIcons[c].enabled = true;
            c++;
        }
    }

    public void showHud(bool b)
    {
        HUD.gameObject.SetActive(b);
    }

    public void showPauseMenu(bool b)
    {
        PauseMenu.gameObject.SetActive(b);
    }

    public void showOptionsMenu(bool b)
    {
        OptionsMenu.gameObject.SetActive(b);
    }
}
