using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Camera))]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    [Range(0, 30)]
    public float distance = 3;
    [Range(1, 89)]
    public float angle = 45;
    [Range(1, 89)]
    public float angleRadius = 20;
    [SerializeField, Min(0f)]
    float focusRadius = 1f; //how far camera can move from it's expected position (from focusPoint)

    //Point where our camera is looking
    Vector3 focusPoint;

    void Start()
    {
        if (target == transform)
            target = null;
        if (target == null)
            target = transform.parent;
    }
    
    void LateUpdate()
    {
        if (focusRadius > 0f)
        {
            float distance = Vector3.Distance(target.position, focusPoint);
            if (distance > focusRadius)
            {
                focusPoint = Vector3.Lerp(target.position, focusPoint, focusRadius / distance);
            }
        }
        else
            focusPoint = target.position;

        Trace(focusPoint, target.forward);
    }


    void Trace(Vector3 point, Vector3 forward)
    {
        Vector3 shift = new Vector3();
        float x = distance * Mathf.Sin((Mathf.PI / 180) * angle);
        float y = distance * Mathf.Cos((Mathf.PI / 180) * angle);
        shift = -forward * x;
        shift.y += Mathf.Abs(y);
        transform.position = point + shift;

        float diff = Vector3.Angle(transform.forward, focusPoint);

        transform.LookAt(focusPoint);

    }
}
