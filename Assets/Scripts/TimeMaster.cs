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
    public static List<float> PRVcheckPointTimes = new List<float>();

    private static float startTime = 0;

    public Text uiText;
    public GameObject BuzzerLocation;

    // Start is called before the first frame update
    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 9999f);
        //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));
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

        if (Input.GetKeyDown(KeyCode.K))
        {
            resetHighScore();
        }
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
        checkPointTimes.Clear();
        startTime = Time.time;
        timerActive = true;
        checkPointTimes.Add(startTime);
        WeaponManager.resetStats();
        //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));
    }

    public static void endTimer(int status)
    {
        timerActive = false;
        checkPointTimes.Add(Time.time);
        PRVcheckPointTimes = checkPointTimes;
        //PRVcheckPointTimes.Sort();

        if (status > 0 && currentTime < highScore)
        {
            highScore = currentTime;
            
            PlayerPrefs.SetFloat("HighScore", highScore);
            //print("HIGHSCORE: " + timeToString(PlayerPrefs.GetFloat("HighScore", 9999f)));
        }
    }

    public static void checkPoint()
    {
        checkPointTimes.Add(Time.time);
        print(Time.time);
    }

    public static string getCheckpoint(int ckpt)
    {
        if (checkPointTimes.Count < ckpt)
        {
            return "-1";
        }

        print(checkPointTimes[ckpt] + " - " + checkPointTimes[ckpt - 1]);

        float time = checkPointTimes[ckpt] - checkPointTimes[ckpt - 1];
        return timeToString(time);
    }

    public static void resetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
    }
}
