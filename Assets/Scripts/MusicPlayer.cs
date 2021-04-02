using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    static GameObject instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = gameObject;
        AkSoundEngine.PostEvent("StartMusic", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void MoveToRoom(Vector3 roomPosition)
    {
        instance.transform.position = roomPosition;
    }
}
