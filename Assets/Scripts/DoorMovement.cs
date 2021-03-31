using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorMovement : MonoBehaviour
{
    [Header("Door Rotation")]
    [SerializeField] float doorRotation = 90;
    [SerializeField] float rotateSpeed = 10f;

    public bool isOpen = false;
    float openRotation;
    float closedRotation;

    // Start is called before the first frame update
    void Start()
    {
        openRotation = transform.eulerAngles.y + doorRotation;
        closedRotation = transform.eulerAngles.y;
    }

    public void MoveToOpenPosition()
    {
        if (!isOpen)
        {
            isOpen = true;
            GetComponentInChildren<Collider>().enabled = false;
            StartCoroutine(RotateOvertime(openRotation));
        }
    }

    public void MoveToClosedPosition()
    {
        if (isOpen)
        {
            isOpen = false;
            GetComponentInChildren<Collider>().enabled = true;
            StartCoroutine(RotateOvertime(closedRotation));
        }
    }

    IEnumerator RotateOvertime(float newY)
    {
        float startRotation = transform.eulerAngles.y;
        float scaledSpeed = 0;

        while (newY != transform.eulerAngles.y)
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, Mathf.LerpAngle(startRotation, newY, scaledSpeed), transform.eulerAngles.z);
            scaledSpeed += rotateSpeed * Time.deltaTime;
            //print(scaledSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
