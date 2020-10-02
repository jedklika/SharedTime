using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    private void FixedUpdate()
    {
        Vector3 desiedPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiedPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
