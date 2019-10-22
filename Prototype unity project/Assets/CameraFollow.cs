using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public Vector3 lookAtOffset;

    private float initZ;

    public float panOutZ;

    public playermover player;

    void Start()
    {
        initZ = offset.z;
    }

    void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        transform.LookAt(target.position + lookAtOffset);

        if (player.stuckToWall)
        {
            offset.z = Mathf.Lerp(offset.z, initZ - panOutZ, 0.125f);
        }
        else
        {
            offset.z = Mathf.Lerp(offset.z, initZ, 0.125f);
        }
    }

}