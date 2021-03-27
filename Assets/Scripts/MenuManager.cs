using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public float sensitivity = PlayerInput.lookSensitivity;
    public float fov = WeaponManager.defaultFOV;
    public bool invertY = PlayerInput.InvertYAxis;
    [SerializeField] Text sensText;
    [SerializeField] Text sensActiveText;
    [SerializeField] Text fovText;

    [SerializeField] Slider sensSlider;
    [SerializeField] Slider fovSlider;

    [SerializeField] InputField sensInput;
    [SerializeField] InputField fovInput;

    void Start()
    {
        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";
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

    public void ApplySettings()
    {
        PlayerInput.lookSensitivity = sensitivity;
        PlayerInput.InvertYAxis = invertY;
        WeaponManager.defaultFOV = fov;
        sensText.text = sensitivity * 100 + "";
        fovText.text = fov + "";
    }

    public void setSensitivity(float n)
    {
        sensitivity = n / 100f;
        sensActiveText.text = n + "";
        sensSlider.value = n;
        sensInput.text = n + "";
    }

    public void setSensitivity(string s) //I'm a GENIUS
    {
        int n = int.Parse(s);
        setSensitivity(n);
    }

    public void setFov(float n)
    {
        fov = n;
        //fovActiveText.text = n + "";
        fovSlider.value = n;
        fovInput.text = n + "";
    }

    public void setFov(string s) //Watch me do it again. Watch me.
    {
        int n = int.Parse(s);
        setFov(n);
    }

    public void resetSettings()
    {
        sensitivity = 0.15f;
        fov = 60f;
        invertY = false;
        setFov(fov);
        setSensitivity(sensitivity*100);
    }
}
