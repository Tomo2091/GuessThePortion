using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using System.Collections;

public class GrabBasket : MonoBehaviour
{
    public InputActionReference confirmButton;
    public InputActionReference placeButton;
    public Transform leftHand;
    public Transform counterPoint;
    public GameObject bagPrefab;

    private bool isGrabbed = false;
    private bool isAttached = false;
    private bool isPlaced = false;

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
        transform.localScale = Vector3.one * 2f;

        var grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("ตะกร้าติดมือซ้ายแล้ว!");
    }

    void PlaceOnCounter()
    {
        if (counterPoint == null)
        {
            Debug.Log("ยังไม่ได้ตั้ง counter point!");
            return;
        }

        isPlaced = true;
        transform.position = counterPoint.position;
        transform.rotation = counterPoint.rotation;

        Debug.Log("วางตะกร้าบน counter แล้ว!");

        StartCoroutine(SwapToBag());
    }

    IEnumerator SwapToBag()
    {
        yield return new WaitForSeconds(2f);

        if (bagPrefab != null)
        {
            bagPrefab.SetActive(true);

            var grabBag = bagPrefab.GetComponent<GrabBag>();
            if (grabBag != null)
            {
                grabBag.leftHand = leftHand;
                grabBag.confirmButton = confirmButton;
            }
        }

        gameObject.SetActive(false);

        Debug.Log("เปลี่ยนเป็นถุงแล้ว!");
    }

    public bool IsAttached()
    {
        return isAttached;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}