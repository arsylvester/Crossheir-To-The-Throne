using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Doors")]
    [SerializeField] GameObject leftPivot;
    [SerializeField] GameObject rightPivot;

    public bool isOpen = false;

    public void openDoors()
    {
        if (!isOpen)
        {
            isOpen = true;
            leftPivot.GetComponent<DoorMovement>().MoveToOpenPosition();
            rightPivot.GetComponent<DoorMovement>().MoveToOpenPosition();
        }
    }

    public void closeDoors()
    {
        if (isOpen)
        {
            isOpen = false;
            leftPivot.GetComponent<DoorMovement>().MoveToClosedPosition();
            rightPivot.GetComponent<DoorMovement>().MoveToClosedPosition();
        }
    }
}
