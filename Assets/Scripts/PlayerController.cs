using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Camera PlayerCamera;
    public float RotationSpeed = 1f; // not sure why we need rotation speed and input sens
    //public float AimingRotationMultiplier = 1f; // only used for ADS
    float m_CameraVerticalAngle = 0f;
    public float MaxSpeedOnGround = 10f;
    public float MovementSharpnessOnGround = 15; //"Sharpness for the movement when grounded, a low value will make the player accelerate and decelerate slowly, a high value will do the opposite"

    public Vector3 CharacterVelocity { get; set; }
    public bool IsGrounded { get; private set; }

    PlayerInput m_InputHandler;
    CharacterController m_Controller;

    /*
    public float RotationMultiplier
    {
        get
        {
            if (m_WeaponsManager.IsAiming)
            {
                return AimingRotationMultiplier;
            }

            return 1f;
        }
    }
    */

    void Start()
    {
        m_InputHandler = GetComponent<PlayerInput>();
        m_Controller = GetComponent<CharacterController>();

        m_Controller.enableOverlapRecovery = true; //no idea what this does lmao
    }

    void Update()
    {
        manageAiming();
        manageMovement();
    }
    
    void manageAiming()
    {
        // horizontal character rotation
        //transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed * RotationMultiplier), 0f), Space.Self); //Original code that allows for ADS
        transform.Rotate(new Vector3(0f, (m_InputHandler.GetLookInputsHorizontal() * RotationSpeed), 0f), Space.Self);

        // vertical camera rotation
        //m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed * RotationMultiplier;
        m_CameraVerticalAngle += m_InputHandler.GetLookInputsVertical() * RotationSpeed;

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

        m_Controller.Move(CharacterVelocity * Time.deltaTime);
    }
}
