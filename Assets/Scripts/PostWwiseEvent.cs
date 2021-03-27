using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostWwiseEvent : MonoBehaviour
{
    public AK.Wwise.Event ReloadEvent;
    public AK.Wwise.Event BulletDropEvent;
    public AK.Wwise.Event SpinEvent;

    public void PlayReloadSound()
    {
        ReloadEvent.Post(gameObject);
    }

    public void PlayBulletDropSound()
    {
        BulletDropEvent.Post(gameObject);
    }

    public void SpecialReloadSound()
    {
        SpinEvent.Post(gameObject);
    }
}
