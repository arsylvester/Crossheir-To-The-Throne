using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    public GameObject room;
    public GameObject backDoor;

    private void OnTriggerEnter(Collider other)
    {
        if (!TimeMaster.timerActive)
        {
            TimeMaster.startTimer();
            room.GetComponent<RoomController>().roomStarted = false;
            room.GetComponent<RoomController>().startRoom();
            if(backDoor.GetComponent<DoorController>() != null)
            {
                backDoor.GetComponent<DoorController>().closeDoors();
            }
        }
    }
}
