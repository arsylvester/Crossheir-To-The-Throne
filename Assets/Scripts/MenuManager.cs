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
    public static float volume;
    public static float xhairSize;
    public static bool invertY;
    public static bool minmode;
    public static int xhairStyle;

    private float default_sensitivity = 0.15f;
    private float default_fov = 60f;
    private float default_volume = 90f;
    private float default_xhairSize = 1f;
    private bool default_invertY = false;
    private bool default_minmode = false;
    private int default_xhairStyle = 0;

    [Header("I gave up and made this script reference other scripts.")]
    [SerializeField] PlayerController m_PlayerController;
    [SerializeField] HudManager m_HudManager;
    [SerializeField] WeaponManager m_WeaponManager;
    [SerializeField] PlayerInput m_PlayerInput;

    [Header("Set these to the placeholder text values.")]
    [SerializeField] Text sensText;
    [SerializeField] Text fovText;
    [SerializeField] Text volumeText;
    [SerializeField] Text xhairSizeText;

    [Header("Sliders")]
    [SerializeField] Slider sensSlider;
    [SerializeField] Slider fovSlider;
    [SerializeField] Slider volumeSlider;
    [SerializeField] Slider xhairSizeSlider;

    [Header("Input fields")]
    [SerializeField] InputField sensInput;
    [SerializeField] InputField fovInput;
    [SerializeField] InputField volumeInput;
    [SerializeField] InputField xhairSizeInput;

    [Header("Toggles")]
    [SerializeField] Toggle yaxisToggle;
    [SerializeField] Toggle minmodeToggle;

    [Header("Drop downs")]
    [SerializeField] Dropdown xhairStyleDropdown;


    #region Player Prefs Keys

    private const string VOLUME_PREF = "volume";
    private const string FOV_PREF = "fov";
    private const string SENSITIVITY_PREF = "sens";
    private const string YAXIS_PREF = "yaxis";
    private const string HUD_MINMODE_PREF = "minmode";
    private const string XHAIR_SIZE_PREF = "xhair-size";
    private const string XHAIR_STYLE_PREF = "xhair-style";

    #endregion

    void Start()
    {
        m_PlayerInput = FindObjectOfType<PlayerInput>();
        m_WeaponManager = FindObjectOfType<WeaponManager>();
        
        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";

        sensitivity = PlayerPrefs.GetFloat(SENSITIVITY_PREF, default_sensitivity);
        fov = PlayerPrefs.GetFloat(FOV_PREF, default_fov);
        volume = PlayerPrefs.GetFloat(VOLUME_PREF, default_volume);
        xhairSize = PlayerPrefs.GetFloat(XHAIR_SIZE_PREF, default_xhairSize);
        xhairStyle = PlayerPrefs.GetInt(XHAIR_STYLE_PREF, default_xhairStyle);
        int mm = PlayerPrefs.GetInt(HUD_MINMODE_PREF, 0);
        minmode = mm > 0;
        int yaxis = PlayerPrefs.GetInt(YAXIS_PREF, 0);
        invertY = yaxis > 0;

        //last minuite fix
        if (fov == 0f)
            setFov(default_fov);
        if (sensitivity == 0f)
            setSensitivity(default_sensitivity);

        sensSlider.value = sensitivity * 100;
        fovSlider.value = fov;
        volumeSlider.value = volume;
        AkSoundEngine.SetRTPCValue("MasterVolume", volume);

        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";
        volumeText.text = volume + "";
        xhairSizeText.text = xhairSize + "";

        yaxisToggle.isOn = invertY;
        minmodeToggle.isOn = minmode;

        xhairStyleDropdown.value = xhairStyle;
        //xhairStyleDropdown.captionText.text = "Select one.";

    }

    public void LauchGame()
    {
        //SceneManager.LoadScene("Henrys Realm");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ResumeGame()
    {
        m_PlayerController.unpauseGame();
    }

    public void ResetRun()
    {
        //teleport player back to start and reset timer.
        ResetScene resetScene = FindObjectOfType<ResetScene>(); //this is terribly unoptimal
        resetScene.ResetRun();
        ResumeGame();
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene(0);
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

        if (m_PlayerInput != null)
            m_PlayerInput.lookSensitivity = sensitivity;

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

        if (m_WeaponManager != null)
            m_WeaponManager.setFOV(fov);

        print("Fov set: " + fov);
    }

    public void setFov(string s) //Watch me do it again. Watch me.
    {
        int n = int.Parse(s);
        setFov(n);
    }

    public void setVolume(float n)
    {
        volume = n;
        volumeSlider.value = n;
        volumeInput.text = n + "";
        SetPref(VOLUME_PREF, n);
        AkSoundEngine.SetRTPCValue("MasterVolume", n);
        print("Volume set: " + volume);
    }

    public void setVolume(string s)
    {
        int n = int.Parse(s);
        setVolume(n);
    }

    public void setXhairSize(float n)
    {
        xhairSize = n;
        xhairSizeSlider.value = n;
        xhairSizeInput.text = n + "";
        SetPref(XHAIR_SIZE_PREF, n);
        m_HudManager.setXhairSize(n);
        print("Xhair Size set: " + n);
    }

    public void setXhairSize(string s)
    {
        int n = int.Parse(s);
        setXhairSize(n);
    }

    public void setXhairStyle(int s)
    {
        if (m_HudManager != null)
        {
            xhairStyle = s;
            SetPref(XHAIR_STYLE_PREF, s);
            m_HudManager.setXhair(s);
        }
    }

    public void setYAxis(bool toggle)
    {
        if (toggle)
            SetPref(YAXIS_PREF, 1);
        else
            SetPref(YAXIS_PREF, 0);

        print("Invert Y Axis set: " + toggle);
    }

    public void setMinMode(bool toggle)
    {
        if (m_HudManager != null)
        {
            if (toggle)
            {
                SetPref(HUD_MINMODE_PREF, 1);
                m_HudManager.goMinMode();
            }
            else
            {
                SetPref(HUD_MINMODE_PREF, 0);
                m_HudManager.exitMinMode();
            }


            print("Hud MinMode: " + toggle);
        }
    }

    public void resetSettings()
    {
        setVolume(default_volume);
        setFov(default_fov);
        setSensitivity(default_sensitivity * 100);
        setYAxis(default_invertY);

        if (m_HudManager != null)
        {
            setMinMode(default_minmode);
            setXhairSize(default_xhairSize);
            //add xhair style
        }
    }

    public void playButtonSound()
    {
        AkSoundEngine.PostEvent("Select", gameObject);
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

    public static bool getMinMode()
    {
        int value = PlayerPrefs.GetInt(HUD_MINMODE_PREF, 0);
        return value > 0;
    }
}
