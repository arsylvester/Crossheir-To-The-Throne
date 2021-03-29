using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject door;
    public List<TargetSetController> targetSets;

    int activeSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        resetRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetRoom()
    {
        for (int i = 0; i < targetSets.Count; i++)
        {
            targetSets[i].deactivateSet();
        }
        door.SetActive(true);
    }

    public void startRoom()
    {
        activeSet = 0;
        targetSets[activeSet].activateSet();
        door.SetActive(true);
    }

    public void nextSet()
    {
        activeSet++;
        if (activeSet < targetSets.Count)
            targetSets[activeSet].activateSet();
        else
            door.SetActive(false);
    }
}
