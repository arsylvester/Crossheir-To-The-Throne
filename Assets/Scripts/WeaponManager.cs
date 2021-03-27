using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour
{
    // This class handles all the gun logic

    [Header("FOV")]
    public float defaultFOV = 60f;
    [SerializeField] float aimingFOV = 30f;

    //Gun vars
    bool isReloading = false;
    int currentAmmo = 3;
    int killStreak = 0;
    int shotStreak = 0;
    bool missedShot = true;
    
    [Header("HUD")]
    public Text ammo;
    [SerializeField] GameObject bulletImpact;
    [SerializeField] RawImage[] dots;
    public bool[] dotsActive = new bool[3];

    PlayerInput m_InputHandler;
    PlayerController m_PlayerController;
    [SerializeField] HudManager m_HudManager; //this one needs to be set in the inspector

    [SerializeField] Animator RevolverAnimator;

    public bool isAiming { get; private set; }
    public bool wasAiming { get; private set; }

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_PlayerController = GetComponent<PlayerController>();
        setFOV(MenuManager.getFov());
        updateHUD();
        ammo.text = currentAmmo + "";
        aimingFOV = MenuManager.getFov() / 2;
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
            missedShot = true;
            int prevKillStreak = killStreak;
            handleShoot();

            if (missedShot)
            {
                killStreak = 0;
                shotStreak = 0;
            }
            else
                shotStreak++;

            if (killStreak > prevKillStreak + 1)
                collateral();

            /*
            if (killStreak != 0 && killStreak % 3 == 0)
                tripleKill();
            */
            if (shotStreak != 0 && shotStreak % 3 == 0)
            {
                tripleKill();
            }
            else
            {
                currentAmmo -= 1;
                ammo.text = currentAmmo + "";
                updateHUD();
            }

            AkSoundEngine.PostEvent("GunFire", gameObject);

            RevolverAnimator.SetTrigger("shoot");

            return true;
        }
        return false;
    }

    void handleShoot() //Actually fires the gun
    {
        RaycastHit[] hits;
        hits = Physics.RaycastAll(m_PlayerController.PlayerCamera.transform.position, 
            m_PlayerController.PlayerCamera.transform.forward, Mathf.Infinity); //TODO: make this not infinity to boost performance.

        //RaycastAll returns an unsorted array, so we need to sort by distance from gun
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); //Honestly i'm too scared to look up how poorly optimized this is

        for (int k = 0; k < hits.Length; k++)
        {
            RaycastHit h = hits[k];

            if (h.collider.CompareTag("Target") && !h.collider.GetComponentInParent<TargetMovement>().isHit) //If bullet collides with a target and target hasn't been hit
            {
                h.collider.GetComponentInParent<TargetMovement>().MoveToHitPosition(); //Play knock down animation
                print("target hit: " + h.collider.name);
                missedShot = false;
                killStreak++;
            }
            else
            {
                print("hit: " + h.collider.name);
                return; //Return to just ignore everything else in the array
            }
        }  
    }

    void tripleKill()
    {
        //Display triple kill graphic on HUD
        print("Triple Kill");
        m_HudManager.DisplayTripleKill();

        //Special reload
        currentAmmo = 3;
        ammo.text = currentAmmo + "";
        RevolverAnimator.SetTrigger("special reload");
        updateHUD();
    }

    void collateral()
    {
        //Display collateral graphic on HUD
        print("Collateral");
        m_HudManager.DisplayCollateral();
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
        RevolverAnimator.SetTrigger("reload");
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

    void updateHUD() //Refresh HUD ammo icons to match currentAmmo
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
