using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform cameraTransform;
    public Vector3 offset = new Vector3(0.45f, -0.1f, 0.8f);

    void Update()
    {
        transform.position = cameraTransform.position +
                            cameraTransform.TransformDirection(offset);
        transform.rotation = cameraTransform.rotation;
    }
}