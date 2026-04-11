using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;

public class GrabBag : MonoBehaviour
{
    public Transform leftHand;
    public InputActionReference confirmButton;

    private bool isGrabbed = false;
    private bool isAttached = false;

    void Start()
    {
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        if (confirmButton != null)
            confirmButton.action.Enable();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (!isAttached)
            isGrabbed = false;
    }

    void Update()
    {
        if (isGrabbed && !isAttached && confirmButton != null && confirmButton.action.WasPressedThisFrame())
        {
            AttachToLeftHand();
        }

        if (isAttached && leftHand != null)
        {
            transform.position = leftHand.position
                + leftHand.forward * 0.3f
                + leftHand.up * -0.2f
                + leftHand.right * -0.3f;
            transform.rotation = leftHand.rotation;
        }
    }

    void AttachToLeftHand()
    {
        isAttached = true;

        var grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("ถุงติดมือซ้ายแล้ว!");
    }
}