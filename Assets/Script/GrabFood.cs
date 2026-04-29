using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;

public class GraFood : MonoBehaviour
{
    public Transform leftHand;
    public InputActionReference confirmButton;
    public InputActionReference placeButton;

    public Transform OvenPoint;
    private bool isPlaced = false;

    

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
        if (placeButton != null)
            placeButton.action.Enable();
    }

     void PlaceOnCounter()
    {
        if (OvenPoint == null)
        {
            Debug.Log("ยังไม่ได้ตั้ง counter point!");
            return;
        }

        isPlaced = true;
        transform.position = OvenPoint.position;
        transform.rotation = OvenPoint.rotation;

        Debug.Log("วางตะกร้าบน counter แล้ว!");

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

        if (isAttached && !isPlaced && leftHand != null)
        {
            transform.position = leftHand.position + leftHand.forward * 0.7f + leftHand.up * -0.7f + leftHand.right * -0.2f;
            transform.rotation = leftHand.rotation;
        }

        if (isAttached && !isPlaced && placeButton != null && placeButton.action.WasPressedThisFrame())
        {
            PlaceOnCounter();
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

        Debug.Log("พิซซ่าติดมือแล้ว");
    }
}