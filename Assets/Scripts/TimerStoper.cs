using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerStoper : MonoBehaviour
{
    [SerializeField] GameObject scoreBoard;

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
        TimeMaster.endTimer(1);
        scoreBoard.GetComponent<BoardScript>().drawStats();
    }
}
