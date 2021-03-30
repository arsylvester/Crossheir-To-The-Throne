using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class WeaponManager : MonoBehaviour
{
    // This class handles all the gun logic

    //[Header("FOV")]
    float defaultFOV = 60f;
    float playerFOV = 60f;
    float aimingFOV = 30f;

    
    bool isReloading = false;
    int currentAmmo = 3;
    int killStreak = 0;
    int shotStreak = 0;
    bool missedShot = true;
    float LastTimeShot = 0;


    public static int shotsTaken, shotsHit, maxKillstreak, maxShotstreak;

    [Header("Gun")]
    [SerializeField] float ShotDelay = .2f;
    [SerializeField] float ReloadTime = 3f;


    //[Header("HUD")]
    //[SerializeField] GameObject bulletImpact;
    //[SerializeField] RawImage[] dots;
    //public bool[] dotsActive = new bool[3];

    
    PlayerInput m_InputHandler;
    PlayerController m_PlayerController;
    [Header("Not Gun")]
    [SerializeField] HudManager m_HudManager; //this one needs to be set in the inspector

    [SerializeField] Animator RevolverAnimator;
    [SerializeField] VisualEffect MuzzleFlash;
    [SerializeField] GameObject PoolBulletHoleVFX;
    private int PoolIndex = 0;

    public bool isAiming { get; private set; }
    public bool wasAiming { get; private set; }

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_PlayerController = GetComponent<PlayerController>();
        setFOV(MenuManager.getFov());
        //updateHUD();
        playerFOV = MenuManager.getFov();
        aimingFOV = MenuManager.getFov() / 2;

        shotsTaken = 0;
        shotsHit = 0;
        maxKillstreak = 0;
        maxShotstreak = 0;
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
            shotsTaken++;

            handleShoot();

            if (missedShot)
            {
                killStreak = 0;
                shotStreak = 0;
            }
            else
            {
                shotStreak++;
                shotsHit++;
            }

            if (killStreak > prevKillStreak + 1)
                collateral();

            if (killStreak > maxKillstreak)
                maxKillstreak = killStreak;

            if (shotStreak > maxShotstreak)
                maxShotstreak = shotStreak;

            if (shotStreak != 0 && shotStreak % 3 == 0)
            {
                tripleKill();
            }
            else
            {
                currentAmmo -= 1;
                updateHUD();
            }

            AkSoundEngine.PostEvent("GunFire", gameObject);
            MuzzleFlash.Play();
            RevolverAnimator.SetTrigger("shoot");

            return true;
        }
        return false;
    }

    void handleShoot() //Actually fires the gun
    {
        LastTimeShot = Time.time;
        
        RaycastHit[] hits;
        hits = Physics.RaycastAll(m_PlayerController.PlayerCamera.transform.position, 
            m_PlayerController.PlayerCamera.transform.forward, Mathf.Infinity, -1, QueryTriggerInteraction.Ignore); //TODO: make this not infinity to boost performance.

        //RaycastAll returns an unsorted array, so we need to sort by distance from gun
        System.Array.Sort(hits, (x, y) => x.distance.CompareTo(y.distance)); //Honestly i'm too scared to look up how poorly optimized this is

        for (int k = 0; k < hits.Length; k++)
        {
            RaycastHit h = hits[k];

            if (h.collider.CompareTag("Target") && !h.collider.GetComponentInParent<TargetMovement>().isHit) //If bullet collides with a target and target hasn't been hit
            {
                AkSoundEngine.PostEvent("TargetHit", gameObject);
                h.collider.GetComponentInParent<TargetMovement>().MoveToHitPosition(); //Play knock down animation
                print("target hit: " + h.collider.name);
                missedShot = false;
                killStreak++;
                //PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.GetComponent<VisualEffect>().SetBool("isTargetImpact", true);
                spawnBulletHoleTarget(h.point, h.normal, h.transform);
            }
            else
            {
                print("hit: " + h.collider.name);
                PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.GetComponent<VisualEffect>().SetBool("isTargetImpact", false);
                PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.GetComponent<VisualEffect>().SetVector3("SparkPos", h.point);
                spawnBulletHole(h.point, h.normal);
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
        if (LastTimeShot + ShotDelay >= Time.time)
            return true;

        return false;
    }

    void reload()
    {
        isReloading = true;
        //add more code to make this a real reload
        RevolverAnimator.SetTrigger("reload");
        StartCoroutine(waitForReload());
        waitForReload();
        //updateHUD();
    }

    IEnumerator waitForReload()
    {
        yield return new WaitForSecondsRealtime(ReloadTime);
        currentAmmo = 3;
        isReloading = false;
        updateHUD();
    }

    public void softReload()
    {
        currentAmmo = 3;
        updateHUD();
    }

    void spawnBulletHole(Vector3 pos, Vector3 norm)
    {
        PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.transform.position = pos;
        PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.transform.rotation = Quaternion.LookRotation(norm);
        PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.GetComponent<VisualEffect>().SetBool("isTargetImpact", false);
        PoolBulletHoleVFX.transform.GetChild(PoolIndex).gameObject.GetComponent<VisualEffect>().Play();

        if(++PoolIndex == PoolBulletHoleVFX.transform.childCount) { PoolIndex = 0; }
    }

    void spawnBulletHoleTarget(Vector3 pos, Vector3 norm, Transform ImpactParent)
    {
        GameObject Impact = Instantiate(PoolBulletHoleVFX.transform.GetChild(0).gameObject, pos, Quaternion.LookRotation(norm), ImpactParent);
        Impact.GetComponent<VisualEffect>().SetVector3("SparkPos", pos);
        Impact.GetComponent<VisualEffect>().SetBool("isTargetImpact", true);
        Impact.GetComponent<VisualEffect>().Play();
        Destroy(Impact, 30.0f);
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
            if (m_PlayerController.PlayerCamera.fieldOfView < playerFOV)
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
        m_HudManager.updateAmmo(currentAmmo);
    }

    public static void resetStats()
    {
        maxKillstreak = 0;
        maxShotstreak = 0;
        shotsTaken = 0;
        shotsHit = 0;
    }
}
