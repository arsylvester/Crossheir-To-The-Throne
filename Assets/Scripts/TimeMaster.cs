using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeMaster : MonoBehaviour
{
    public static bool timerActive = false;
    public static string timerText = "00:00.00";
    public static float currentTime = 0;
    public static float highScore = 9999f;
    public static List<float> checkPointTimes = new List<float>();

    private static float startTime = 0;

    public Text uiText;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 9999f);
        //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));

        checkPointTimes.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if(timerActive)
        {
            currentTime = Time.time - startTime;
            timerText = timeToString(currentTime);
        }
        uiText.text = timerText;
    }

    public static string timeToString(float time)
    {
        // Unity String formatting is actually garbage
        int minutes = (int)time / 60;
        time = time % 60;
        int seconds = (int)time;
        time -= seconds;
        string str = minutes.ToString("00") + ":" + seconds.ToString("00") + "." + ((int)(time * 100)).ToString("00");
        return str;
    }

    public static void startTimer()
    {
        startTime = Time.time;
        timerActive = true;
        checkPointTimes.Add(startTime);
        WeaponManager.resetStats();
        //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));
    }

    public static void endTimer(int status)
    {
        timerActive = false;
        if (status > 0 && currentTime < highScore)
        {
            highScore = currentTime;
            checkPointTimes.Add(currentTime);
            PlayerPrefs.SetFloat("HighScore", highScore);
            //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));
        }
        else
        {
            checkPointTimes.Clear();
        }
    }

    public static void checkPoint()
    {
        checkPointTimes.Add(Time.time);
    }

    public static void resetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
