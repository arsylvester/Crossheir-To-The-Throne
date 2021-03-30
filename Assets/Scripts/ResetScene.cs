using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScene : MonoBehaviour
{
    [Tooltip("Add a transform if you want to assign a start position. Leave blanck if you want to use the current player position.")]
    [SerializeField] Transform startPositionTransform;
    [SerializeField] Transform bulletHoles;
    Vector3 startPosition;
    Quaternion startRotation;
    GameObject playerObject;
    RoomController[] rooms;
    WeaponManager weapon;
    TimeMaster timer;

    void Start()
    {
        playerObject = FindObjectOfType<PlayerController>().gameObject;
        rooms = FindObjectsOfType<RoomController>();
        weapon = playerObject.GetComponentInChildren<WeaponManager>();

        if(startPositionTransform == null)
        {
            startPosition = playerObject.transform.position;
            startRotation = playerObject.transform.rotation;
        }
        else
        {
            startPosition = startPositionTransform.position;
            startRotation = startPositionTransform.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            ResetRun();
        }
    }

    void ResetRun()
    {
        print("Reseting run: " + playerObject);
        //Room reset
        foreach (RoomController room in rooms)
        {
            room.resetRoom();
        }

        //Player reset
        playerObject.GetComponent<CharacterController>().enabled = false;
        playerObject.transform.position = startPosition;
        playerObject.transform.rotation = startRotation;
        playerObject.GetComponent<CharacterController>().enabled = true;

        //Reset Gun
        weapon.softReload();

        //Stop Time
        TimeMaster.endTimer(-1);

        //Reset Bullet holes
        foreach(Transform hole in bulletHoles)
        {
            hole.localPosition = Vector3.zero;
        }
    }
}
