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

    [SerializeField] GameObject movingPart;

    public bool isHit = false;
    float currentTrackLerp = 0;
    float downRotation;
    Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        downRotation = movingPart.transform.eulerAngles.z + hitRotation;
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && isHit)
        {
            MoveToReadyPosition();
        }

        if (moveOnTrack && !isHit)
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
            GetComponentInChildren<Collider>().enabled = false;
            StartCoroutine(RotateOvertime(downRotation));
            if(this.GetComponentInParent<TargetSetController>() != null)
                this.GetComponentInParent<TargetSetController>().checkIfDefeated();
        }
    }

    public void MoveToReadyPosition()
    {
        if (isHit)
        {
            isHit = false;
            currentTrackLerp = 0;
            GetComponentInChildren<Collider>().enabled = true;
            StartCoroutine(RotateOvertime(movingPart.transform.eulerAngles.z - hitRotation));
        }
    }

    IEnumerator RotateOvertime(float newX)
    {
        float startRotation = movingPart.transform.eulerAngles.z;
        float scaledSpeed = 0;

        while (newX != movingPart.transform.eulerAngles.z)
        {
            movingPart.transform.eulerAngles = new Vector3(movingPart.transform.eulerAngles.x, movingPart.transform.eulerAngles.y, Mathf.LerpAngle(startRotation, newX, scaledSpeed));
            scaledSpeed += rotateSpeed * Time.deltaTime;
            //print(scaledSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
