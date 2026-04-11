using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.XR.CoreUtils;

public class LockCamera : MonoBehaviour
{
    [Header("Look Limits")]
    public float maxYaw = 90f;
    public float maxPitch = 35f;

    [Header("Scene Filter")]
    public bool lockOnlyInMainMenu = true;
    public string mainMenuSceneName = "Mainmenu";

    private Quaternion initialRot;
    private Transform yawTarget;
    private float initialYaw;

    void Start()
    {
        initialRot = transform.localRotation;

        if (!lockOnlyInMainMenu || SceneManager.GetActiveScene().name == mainMenuSceneName)
        {
            var xrOrigin = FindFirstObjectByType<XROrigin>();
            if (xrOrigin != null)
            {
                yawTarget = xrOrigin.transform;
                initialYaw = yawTarget.eulerAngles.y;
            }
        }
    }

    void LateUpdate()
    {
        if (lockOnlyInMainMenu && SceneManager.GetActiveScene().name != mainMenuSceneName)
            return;

        Quaternion current = transform.localRotation;
        Quaternion delta = Quaternion.Inverse(initialRot) * current;

        Vector3 euler = delta.eulerAngles;

        float yaw = Mathf.DeltaAngle(0f, euler.y);
        float pitch = Mathf.DeltaAngle(0f, euler.x);

        yaw = Mathf.Clamp(yaw, -maxYaw, maxYaw);
        pitch = Mathf.Clamp(pitch, -maxPitch, maxPitch);

        transform.localRotation = initialRot * Quaternion.Euler(pitch, yaw, 0f);

        // Clamp XR Origin yaw so simulator cannot rotate 360 around the player.
        if (yawTarget != null)
        {
            float currentYawDelta = Mathf.DeltaAngle(initialYaw, yawTarget.eulerAngles.y);
            float clampedYaw = Mathf.Clamp(currentYawDelta, -maxYaw, maxYaw);
            yawTarget.rotation = Quaternion.Euler(0f, initialYaw + clampedYaw, 0f);
        }
    }

    
}