using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothFollow : MonoBehaviour
{
    public Transform target;
    public float fDistance = 10.0f;
    public float fHeight = 5.0f;
    public float fHeightDamping = 2.0f;
    public float fRotationDamping = 3.0f;

    [AddComponentMenu("Camera-Control/Smooth Follow")]

    void LateUpdate()
    {
        if (!target)
            return;

        float wantedRotationAngle = target.eulerAngles.y;
        float wantedHeight = target.position.y + fHeight;

        float currentRotationAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, fRotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, fHeightDamping * Time.deltaTime);

        var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

        transform.position = target.position;
        transform.position -= currentRotation * Vector3.forward * fDistance;

        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.rotation = target.transform.rotation;
        //transform.LookAt(target);
    }
}
