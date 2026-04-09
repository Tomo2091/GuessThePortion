using UnityEngine;


public class LockCamera : MonoBehaviour
{
    public float maxYaw = 15f;
    public float maxPitch = 10f;

    private Quaternion initialRot;

    void Start()
    {
        initialRot = transform.localRotation;
    }

    void LateUpdate()
    {
        Quaternion current = transform.localRotation;
        Quaternion delta = Quaternion.Inverse(initialRot) * current;

        Vector3 euler = delta.eulerAngles;

        float yaw = Mathf.DeltaAngle(0f, euler.y);
        float pitch = Mathf.DeltaAngle(0f, euler.x);

        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        transform.localRotation = initialRot * Quaternion.Euler(pitch, yaw, 0f);
    }
}