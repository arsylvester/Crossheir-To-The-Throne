using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTrigger : MonoBehaviour
{
    public GameObject room;

    private void OnTriggerEnter(Collider other)
    {
        room.GetComponent<RoomController>().startRoom();
        TimeMaster.checkPoint();
    }
}
