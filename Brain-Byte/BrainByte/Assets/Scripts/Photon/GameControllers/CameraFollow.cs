
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float smoothSpeed = 0.125f;

    private void Start()
    {
        
        Vector3 desiredPostion = target.position + offset;
        transform.position = desiredPostion;
        
    }
    private void FixedUpdate()
    {
        /*
        
        //Old non-working script

        
        Vector3 desiredPostion = target.position + offset;
        //transform.LookAt(target);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
        transform.position = smoothedPosition;

        transform.rotation = target.rotation;

        transform.LookAt(target);   //camera focuses on target instead of rotating when player is moved on horizontal axis
        
    */

    }
}
