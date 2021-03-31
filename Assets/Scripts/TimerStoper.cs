using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStoper : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject backDoor;

    private void OnTriggerEnter(Collider other)
    {
        TimeMaster.endTimer(1);
        AkSoundEngine.PostEvent("Buzzer", gameObject);
        scoreBoard.GetComponent<BoardScript>().drawStats();
        if(backDoor.GetComponent<DoorController>() != null)
        {
            backDoor.GetComponent<DoorController>().openDoors();
        }
    }
}
