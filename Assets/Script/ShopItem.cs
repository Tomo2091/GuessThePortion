using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ShopItem : MonoBehaviour
{
    public string itemName;
    public float price;

    void Start()
    {
        var grab = GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            grab.selectEntered.AddListener(OnGrab);
            grab.selectExited.AddListener(OnRelease);
        }
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.isKinematic = false;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true; // ปล่อยแล้วตกลงตะกร้า
        }
    }
}