using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

//TODO: Clean up unneeded code

public class MenuManager : MonoBehaviour
{
    public static float sensitivity;
    public static float fov;
    public static bool invertY;


    [SerializeField] Text sensText;
    [SerializeField] Text fovText;

    [SerializeField] Slider sensSlider;
    [SerializeField] Slider fovSlider;

    [SerializeField] InputField sensInput;
    [SerializeField] InputField fovInput;

    [SerializeField] Toggle yaxisToggle;


    #region Player Prefs Keys

    private const string FOV_PREF = "fov";
    private const string SENSITIVITY_PREF = "sens";
    private const string YAXIS_PREF = "yaxis";

    #endregion

    void Start()
    {
        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";

        sensitivity = PlayerPrefs.GetFloat(SENSITIVITY_PREF, 0.15f);
        fov = PlayerPrefs.GetFloat(FOV_PREF, 60f);
        int yaxis = PlayerPrefs.GetInt(YAXIS_PREF, 0);
        invertY = yaxis > 0;

        sensSlider.value = sensitivity * 100;
        fovSlider.value = fov;

        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";

        yaxisToggle.isOn = invertY;

    }

    public void LauchGame()
    {
        SceneManager.LoadScene("Henrys Realm");
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SetPref(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
    }

    private void SetPref(string key, int value)
    {
        PlayerPrefs.SetInt(key, value);
    }

    public void setSensitivity(float n)
    {
        sensitivity = n / 100f;

        sensSlider.value = n;
        sensInput.text = n + "";

        //Set PlayerPref to n
        SetPref(SENSITIVITY_PREF, sensitivity);

        print("Sensitivity set: " + sensitivity);
    }

    public void setSensitivity(string s) //I'm a GENIUS
    {
        int n = int.Parse(s);
        setSensitivity(n);
    }

    public void setFov(float n)
    {
        fov = n;
        fovSlider.value = n;
        fovInput.text = n + "";

        //Set PlayerPref to n
        SetPref(FOV_PREF, n);

        print("Fov set: " + fov);
    }

    public void setFov(string s) //Watch me do it again. Watch me.
    {
        int n = int.Parse(s);
        setFov(n);
    }

    public void setYAxis(bool toggle)
    {
        if (toggle)
            SetPref(YAXIS_PREF, 1);
        else
            SetPref(YAXIS_PREF, 0);

        print("Invert Y Axis set: " + toggle);
    }

    public void resetSettings()
    {
        sensitivity = 0.15f;
        fov = 60f;
        invertY = false;

        setFov(60f);
        setSensitivity(15);
        setYAxis(false);
    }

    public static float getFov()
    {
        return PlayerPrefs.GetFloat(FOV_PREF);
    }

    public static float getSens()
    {
        return PlayerPrefs.GetFloat(SENSITIVITY_PREF);
    }

    public static bool getInvertY()
    {
        int yaxis = PlayerPrefs.GetInt(YAXIS_PREF, 0);
        return yaxis > 0;
    }
}
