using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera;
    public float RotationSpeed = 1f; // not sure why we need rotation speed and input sens
    public float AimingRotationMultiplier = 0.5f; // only used for ADS
    float m_CameraVerticalAngle = 0f;
    public float MaxSpeedOnGround = 10f;
    public float MovementSharpnessOnGround = 15; //"Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite"
    public float GravityModifier = 10;
    public float jumpForce = 10;
    public float footStepInterval = 1;

    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }

    public static bool isGamePaused;

    PlayerInput m_InputHandler;
    CharacterController m_Controller;
    WeaponManager m_WeaponManager;
    [SerializeField] HudManager m_HudManager;
    bool hasJumped;
    float footstepDistanceCounter;

    public float RotationMultiplier
    {
        get
        {
            if (m_WeaponManager.isAiming)
            {
                return AimingRotationMultiplier;
            }

            return 1f;
        }
    }

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_Controller = GetComponent<CharacterController>();
        m_WeaponManager = GetComponent<WeaponManager>();

        m_Controller.enableOverlapRecovery = true; //no idea what this does lmao

        isGamePaused = false;
    }

    void Update()
    {
        if (!isGamePaused)
        {
            manageAiming();
            manageMovement();

            if (m_InputHandler.GetCancelInputDown())
                pauseGame();
        }
        else
        {
            if (m_InputHandler.GetCancelInputDown())
                unpauseGame();
        }
            
    }
    
    void manageAiming()
    {
        // horizontal character rotation
        transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier), 0f), Space.Self); //Original code that allows for ADS
        //transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed), 0f), Space.Self);

        // vertical camera rotation
        m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;
        //m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed;

        // limit the camera's vertical angle to min/max
        m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

        // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
        PlayerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0, 0);
    }

    void manageMovement()
    {
        float speedModifier = 1f; //TODO: add a check that changes this if the player is reloading

        // converts move input to a worldspace vector based on our character's transform orientation
        Vector3 worldspaceMoveInput = transform.TransformVector(m_InputHandler.GetMoveInput());

        // calculate the desired velocity from inputs, max speed, and current slope
        Vector3 targetVelocity = worldspaceMoveInput * MaxSpeedOnGround * speedModifier;
         
        // smoothly interpolate between our current velocity and the target velocity based on acceleration speed
        CharacterVelocity = Vector3.Lerp(CharacterVelocity, targetVelocity, MovementSharpnessOnGround * Time.deltaTime);

        if (!IsGrounded && m_Controller.isGrounded && hasJumped)
        {
            AkSoundEngine.PostEvent("Land", gameObject);
        }
        IsGrounded = m_Controller.isGrounded;



        // keep track of distance traveled for footsteps sound
        footstepDistanceCounter += CharacterVelocity.magnitude * Time.deltaTime;

        // jumping
        if (IsGrounded)
        {
            hasJumped = false;
            if (m_InputHandler.GetJumpInputDown())
            {
                // start by canceling out the vertical component of our velocity
                CharacterVelocity = new Vector3(CharacterVelocity.x, 0f, CharacterVelocity.z);

                // then, add the jumpSpeed value upwards
                CharacterVelocity += Vector3.up * jumpForce;

                // play sound
                AkSoundEngine.PostEvent("JumpStart", gameObject);

                // remember last time we jumped because we need to prevent snapping to ground for a short time
               // m_LastTimeJumped = Time.time;
                //HasJumpedThisFrame = true;

                // Force grounding to false
                IsGrounded = false;
                hasJumped = true;
            }
            else
            {
                // footsteps sound
                if (footstepDistanceCounter >= 1f / footStepInterval)
                {
                    footstepDistanceCounter = 0f;
                    AkSoundEngine.PostEvent("FootStep", gameObject);
                }
            }
        }
        else
        {
            // apply the gravity to the velocity
            CharacterVelocity += Vector3.down * GravityModifier * Time.deltaTime;
        }

        m_Controller.Move(CharacterVelocity * Time.deltaTime);
    }

    public void unpauseGame()
    {
        isGamePaused = false;
        Time.timeScale = 1f;
        m_HudManager.showHud(true);
        m_HudManager.showPauseMenu(false);
        m_HudManager.showOptionsMenu(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        EventFeed.instance.FadeOutAll();
    }

    public void pauseGame()
    {
        isGamePaused = true;
        Time.timeScale = 0f;
        m_HudManager.showHud(false);
        m_HudManager.showPauseMenu(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
