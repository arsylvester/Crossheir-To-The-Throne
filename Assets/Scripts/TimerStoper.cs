using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStoper : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard;
    [SerializeField] GameObject backDoor;
    [SerializeField] Transform speakerPosition;

    private void OnTriggerEnter(Collider other)
    {
        if (TimeMaster.timerActive)
        {
            TimeMaster.endTimer(1);
            AkSoundEngine.PostEvent("Buzzer", gameObject);
            MusicPlayer.MoveToRoom(speakerPosition.position);
            scoreBoard.GetComponent<BoardScript>().drawStats();
            if (backDoor.GetComponent<DoorController>() != null)
            {
                backDoor.GetComponent<DoorController>().openDoors();
            }
        }
    }
}
