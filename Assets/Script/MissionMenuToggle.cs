using UnityEngine;
using UnityEngine.InputSystem;

public class MissionMenuToggle : MonoBehaviour
{
    public GameObject missionPanel;
    public Transform cameraTransform;
    public float distanceFromCamera = 1.5f;
    public float heightOffset = -0.2f;

    private bool _isOpen = false;
    private bool _wasPressed = false;

    void Start()
    {
        if (missionPanel != null)
            missionPanel.SetActive(false);
    }

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            ToggleMenu();
            return;
        }

        bool pressed = IsSecondaryButtonPressed();
        if (pressed && !_wasPressed) ToggleMenu();
        _wasPressed = pressed;
    }

    bool IsSecondaryButtonPressed()
    {
        bool result = false;
        var right = new System.Collections.Generic.List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, right);
        foreach (var d in right)
        {
            bool v;
            if (d.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out v) && v) result = true;
        }
        var left = new System.Collections.Generic.List<UnityEngine.XR.InputDevice>();
        UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, left);
        foreach (var d in left)
        {
            bool v;
            if (d.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out v) && v) result = true;
        }
        return result;
    }

    void ToggleMenu()
    {
        _isOpen = !_isOpen;
        Debug.Log("[MissionMenu] " + (_isOpen ? "Open" : "Close"));
        if (missionPanel != null)
        {
            missionPanel.SetActive(_isOpen);
            if (_isOpen) PositionMenu();
        }
    }

    void PositionMenu()
    {
        if (cameraTransform == null) return;
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();
        missionPanel.transform.position = cameraTransform.position + forward * distanceFromCamera + Vector3.up * heightOffset;
        missionPanel.transform.rotation = Quaternion.LookRotation(missionPanel.transform.position - cameraTransform.position);
    }
}