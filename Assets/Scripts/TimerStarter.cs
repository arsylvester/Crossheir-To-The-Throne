using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter(Collider other)
    {
        if (!TimeMaster.timerActive)
        {
            TimeMaster.startTimer();
            room.GetComponent<RoomController>().roomStarted = false;
            room.GetComponent<RoomController>().startRoom();
        }
    }
}
