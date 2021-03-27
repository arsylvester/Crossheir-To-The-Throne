using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter(Collider other)
    {
        TimeMaster.startTimer();
        room.GetComponent<RoomController>().startRoom();
    }
}
