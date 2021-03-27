using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    // Fair warning: I'm taking a lot of this initial code from the unity fps tutorial

    public static float lookSensitivity = 1f;
    public static bool InvertYAxis = false;
    public bool fireInputWasDown;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;
    }

    public bool GetFireInputDown()
    {
        return Input.GetButton("Fire1") && CanProcessInput();
    }

    public bool GetFireInputReleased()
    {
        return !GetFireInputDown() && fireInputWasDown && CanProcessInput();
    }

    public bool GetReloadButtonDown()
    {
        return Input.GetButtonDown("Reload") && CanProcessInput();
    }

    public bool GetAimInputDown()
    {
        return Input.GetButton("Fire2") && CanProcessInput();
    }

    public Vector3 GetMoveInput()
    {
        if (CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f,
                Input.GetAxisRaw("Vertical"));

            // constrain move input to a maximum magnitude of 1, otherwise diagonal movement might exceed the max move speed defined
            move = Vector3.ClampMagnitude(move, 1);

            return move;
        }

        return Vector3.zero;
    }

    public float GetLookInputsHorizontal()
    {
        return GetMouseLookAxis("Mouse X");
    }

    public float GetLookInputsVertical()
    {
        return GetMouseLookAxis("Mouse Y");
    }

    float GetMouseLookAxis(string mouseInputName)
    {
        if (CanProcessInput())
        {
            float i = Input.GetAxisRaw(mouseInputName);

            // handle inverting vertical input
            if (!InvertYAxis && mouseInputName == "Mouse Y")
                i *= -1f;

            // apply sensitivity multiplier
            i *= lookSensitivity;
            
            return i;
        }

        return 0f;
    }
}
