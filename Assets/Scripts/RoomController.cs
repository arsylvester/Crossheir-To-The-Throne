using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public GameObject Target;
    public List<TargetSetController> targetSets;

    int activeSet = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startRoom()
    {
        activeSet = 0;
        targetSets[activeSet].activateSet();
    }

    public void nextSet()
    {
        activeSet++;
        targetSets[activeSet].activateSet();
    }
}
