using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStarter : MonoBehaviour
{
    public GameObject room;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        TimeMaster.startTimer();
        room.GetComponent<RoomController>().startRoom();
    }
}
