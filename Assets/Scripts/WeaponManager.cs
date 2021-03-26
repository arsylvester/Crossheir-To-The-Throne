using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    // This class handles all the gun logic
    
    public float defaultFOV = 60f;
    [SerializeField] float aimingFOV = 30f;

    bool isReloading = false;
    int currentAmmo = 3;
    public Text ammo;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] RawImage[] dots;
    public bool[] dotsActive = new bool[3];

    PlayerInput m_InputHandler;
    PlayerController m_PlayerController;

    public bool isAiming { get; private set; }
    public bool wasAiming { get; private set; }

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_PlayerController = GetComponent<PlayerController>();
        setFOV(defaultFOV);

        for (int x = 0; x < 3; x++)
        {
            dotsActive[x] = true;
        }
    }

    void Update()
    {
        //Rootin tootin ready for shootin
        
        if (isReloading)
            return; //No gun actions performed if in reload animation

        if (m_InputHandler.GetReloadButtonDown())
        {
            // isAiming = false;
            reload();
            return;
        }

        //Are we aiming?
        isAiming = m_InputHandler.GetAimInputDown();

        //Prevent the gun from firing every frame
        if (m_InputHandler.fireInputWasDown && m_InputHandler.GetFireInputReleased())
            m_InputHandler.fireInputWasDown = false;

        if (m_InputHandler.GetFireInputDown() && !m_InputHandler.fireInputWasDown)
        {
            shoot();
            m_InputHandler.fireInputWasDown = true;
        }

        manageAiming();
    }

    bool shoot() //Make sure nothing is preventing you from shooting
    {
        if (currentAmmo != 0 && !inShotDelay())
        {
            handleShoot();
            currentAmmo -= 1;
            ammo.text = currentAmmo + "";
            updateHUD();
            return true;
        }

        return false;
    }

    void handleShoot() //Actually fires the gun
    {
        
        if (Physics.Raycast(m_PlayerController.PlayerCamera.transform.position, m_PlayerController.PlayerCamera.transform.forward,
            out RaycastHit hit, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore))
        {
            //print(hit.point);
            Debug.DrawRay(m_PlayerController.PlayerCamera.transform.position, transform.TransformDirection(m_PlayerController.PlayerCamera.transform.forward) * hit.distance, Color.yellow); // this just doesn't work?
            Instantiate(bulletImpact, hit.point, Quaternion.identity); //create clone of prefab at the hit point.
        }
        
    }

    bool inShotDelay() //Return true if it hasn't been x time since the last shot
    {
        //if (m_LastTimeShot + DelayBetweenShots < Time.time)
        //    return true;

        return false;
    }

    void reload()
    {
        currentAmmo = 3;
        ammo.text = currentAmmo + "";
        //add more code to make this a real reload
        updateHUD();
    }

    void manageAiming() //TODO: Make transition framerate independent
    {
        if (isAiming)
        {
            wasAiming = true;

            if (m_PlayerController.PlayerCamera.fieldOfView > aimingFOV)
                m_PlayerController.PlayerCamera.fieldOfView--;
        }
        else if (wasAiming)
        {
            if (m_PlayerController.PlayerCamera.fieldOfView < defaultFOV)
                m_PlayerController.PlayerCamera.fieldOfView++;
            else
                wasAiming = false;
        }
        
    }

    public void setFOV(float fov)
    {
        m_PlayerController.PlayerCamera.fieldOfView = fov;
        //WeaponCamera.fieldOfView = fov * WeaponFovMultiplier;
    }

    void updateHUD()
    {
        int c = 0;

        dots[0].enabled = false;
        dots[1].enabled = false;
        dots[2].enabled = false;

        while (c < currentAmmo)
        {
            dots[c].enabled = true;
            c++;
        }
    }
}
