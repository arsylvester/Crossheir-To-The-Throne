using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// For controlling a group of targets
public class TargetSetController : MonoBehaviour
{
    public bool setDefeated = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING (REMOVE)
        //if (Input.GetKeyDown(KeyCode.C))
        {
            //activateSet();
        }
    }

    public void activateSet()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponentInChildren<TargetMovement>().MoveToReadyPosition();
            //print("Getting target ready");
        }
    }

    public void deactivateSet()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            child.GetComponentInChildren<TargetMovement>().MoveToHitPosition(0);
        }
    }

    public void checkIfDefeated()
    {
        bool allDown = true;

        //print("Checking for defeat");

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            if (!child.GetComponentInChildren<TargetMovement>().isHit)
                allDown = false;
        }
        setDefeated = allDown;
        if (setDefeated)
        {
            //print("next set starting");
            this.GetComponentInParent<RoomController>().nextSet();
        }
    }
}
