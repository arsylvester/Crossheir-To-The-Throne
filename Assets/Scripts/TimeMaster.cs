using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMaster : MonoBehaviour
{
    public static string timerText;
    public static bool timerActive = false;

    private static float startTime = 0;

    public Text uiText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            float currentTime = Time.time - startTime;
            int minutes = (int)currentTime / 60;
            timerText = minutes + ":" + (currentTime - minutes);
            //print(currentTime);
        }
        uiText.text = timerText;
    }

    public static void startTimer()
    {
        startTime = Time.time;
        timerActive = true;
    }

    public static void endTimer()
    {
        timerActive = false;
    }
}
