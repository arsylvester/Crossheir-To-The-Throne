using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [Header("Hit Rotation")]
    [SerializeField] float hitRotation = 90;
    [SerializeField] float rotateSpeed = .1f;
    [Header("Track Movement")]
    [SerializeField] bool moveOnTrack = false;
    [SerializeField] Transform trackStart;
    [SerializeField] Transform trackEnd;
    [SerializeField] float trackMoveSpeed = 1f;

    public bool isHit = false;
    float currentTrackLerp = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //FOR TESTING (REMOVE)
        if(Input.GetKeyDown(KeyCode.Z) && !isHit)
        {
            MoveToHitPosition();
        }
        if(Input.GetKeyDown(KeyCode.X) && isHit)
        {
            MoveToReadyPosition();
        }

        if(moveOnTrack && !isHit)
        {
            transform.position = Vector3.Lerp(trackStart.position, trackEnd.position, currentTrackLerp);
            currentTrackLerp += trackMoveSpeed * Time.deltaTime;

            if(currentTrackLerp > 1)
            {
                Transform temp = trackStart;
                trackStart = trackEnd;
                trackEnd = temp;
                currentTrackLerp = 0;
            }
        }
    }

    public void MoveToHitPosition()
    {
        if (!isHit)
        {
            isHit = true;
            StartCoroutine(RotateOvertime(transform.eulerAngles.x + hitRotation));
            this.GetComponentInParent<TargetSetController>().checkIfDefeated();
        }
    }

    public void MoveToReadyPosition()
    {
        isHit = false;
        StartCoroutine(RotateOvertime(transform.eulerAngles.x - hitRotation));
    }

    IEnumerator RotateOvertime(float newX)
    {
        float startRotation = transform.eulerAngles.x;
        float scaledSpeed = 0;

        while (newX != transform.eulerAngles.x)
        {
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(startRotation, newX, scaledSpeed), transform.eulerAngles.y, transform.eulerAngles.z);
            scaledSpeed += rotateSpeed * Time.deltaTime;
            //print(scaledSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
