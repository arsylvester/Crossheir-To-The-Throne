using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("StartMusic", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
