using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // This class handles all the gun logic
    
    public float DefaultFOV = 90f;
    bool isReloading = false;
    int currentAmmo = 3;
    [SerializeField] GameObject bulletImpact;

    PlayerInput m_InputHandler;
    PlayerController m_PlayerController;

    public bool IsAiming { get; private set; }

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_PlayerController = GetComponent<PlayerController>();
    }

    void Update()
    {
        //rootin tootin ready for shootin
        
        if (isReloading)
            return; //you can't shoot if your gun isn't loaded.

        if (m_InputHandler.GetReloadButtonDown())
        {
            // isAiming = false;
            reload();
            return;
        }

        //prevent the gun from firing every frame
        if (m_InputHandler.fireInputWasDown && m_InputHandler.GetFireInputReleased())
            m_InputHandler.fireInputWasDown = false;

        if (m_InputHandler.GetFireInputDown() && !m_InputHandler.fireInputWasDown)
        {
            shoot();
            m_InputHandler.fireInputWasDown = true;
            return;
        }
            
    }

    bool shoot()
    {
        if (currentAmmo != 0 && !inShotDelay())
        {
            handleShoot();
            currentAmmo -= 1;
            return true;
        }

        return false;
    }

    void handleShoot()
    {
        
        if (Physics.Raycast(m_PlayerController.PlayerCamera.transform.position, m_PlayerController.PlayerCamera.transform.forward,
            out RaycastHit hit, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore))
        {
            //print(hit.point);
            Debug.DrawRay(m_PlayerController.PlayerCamera.transform.position, transform.TransformDirection(m_PlayerController.PlayerCamera.transform.forward) * hit.distance, Color.yellow); // this just doesn't work?
            Instantiate(bulletImpact, hit.point, Quaternion.identity); //create clone of prefab at the hit point.
        }
        
    }

    bool inShotDelay() //return true if it hasn't been x time since the last shot
    {
        //if (m_LastTimeShot + DelayBetweenShots < Time.time)
        //    return true;

        return false;
    }

    void reload()
    {
        currentAmmo = 3;
        //add more code to make this a real reload
    }

    public void setFOV(float fov)
    {
        m_PlayerController.PlayerCamera.fieldOfView = fov;
        //WeaponCamera.fieldOfView = fov * WeaponFovMultiplier;
    }
}
