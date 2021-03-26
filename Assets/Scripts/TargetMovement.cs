using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    [SerializeField] float hitRotation = 90;
    [SerializeField] float rotateSpeed = .1f;
    bool isHit = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) && !isHit)
        {
            MoveToHitPosition();
        }
        if(Input.GetKeyDown(KeyCode.S) && isHit)
        {
            MoveToReadyPosition();
        }
    }

    public void MoveToHitPosition()
    {
        isHit = true;
        StartCoroutine(RotateOvertime(transform.eulerAngles.x + hitRotation));
    }

    public void MoveToReadyPosition()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x - hitRotation, transform.eulerAngles.y, transform.eulerAngles.z);
        isHit = false;
    }

    IEnumerator RotateOvertime(float newX)
    {
        float startRotation = transform.eulerAngles.x;
        float scaledSpeed = 0;

        while (newX != transform.eulerAngles.x)
        {
            transform.eulerAngles = new Vector3(Mathf.LerpAngle(startRotation, newX, scaledSpeed), transform.eulerAngles.y, transform.eulerAngles.z);
            scaledSpeed += rotateSpeed * Time.deltaTime;
            print(scaledSpeed);
            yield return new WaitForEndOfFrame();
        }
    }
}
