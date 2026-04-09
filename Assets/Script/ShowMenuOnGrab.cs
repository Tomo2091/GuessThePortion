// using UnityEngine;
// using UnityEngine.XR.Interaction.Toolkit;
// using UnityEngine.XR.Interaction.Toolkit.Interactables;
// public class ShowMenuOnGrab : MonoBehaviour
// {
//     public GameObject menuCanvas;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
//         menuCanvas.SetActive(false);

//         var grab = GetComponent<XRGrabInteractable>();
//         grab.selectEntered.AddListener(OnGrab);
//         grab.selectExited.AddListener(OnRelease);
//     }

//     void OnGrab(SelectEnterEventArgs args)
//     {
//         menuCanvas.SetActive(true);
//     }

//     void OnRelease(SelectExitEventArgs args)
//     {
//         menuCanvas.SetActive(false);
//     }
//     // Update is called once per frame
//     void Update()
//     {
//         if (menuCanvas.activeSelf)
//         {
//             menuCanvas.transform.LookAt(Camera.main.transform);
//             menuCanvas.transform.Rotate(0, 180, 0);
//         }
//     }
// }

using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ShowMenuOnGrab : MonoBehaviour
{
    public GameObject menuCanvas;
    public InputActionReference confirmButton; // ปุ่ม B

    private bool isGrabbed = false;
    private Transform playerBody;

    public static List<string> inventory = new List<string>();
    public static bool alreadyCollected = false;

    void Start()
    {
        menuCanvas.SetActive(false);

        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        if (confirmButton != null)
            confirmButton.action.Enable();
    }

    void OnDisable()
    {
        if (confirmButton != null)
            confirmButton.action.Disable();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        menuCanvas.SetActive(true);
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        menuCanvas.SetActive(false);
        isGrabbed = false;
    }

    void Update()
    {
        if (playerBody == null)
        {
            if (Camera.main != null)
                playerBody = Camera.main.transform;
            else
                return;
        }

        // หัน Canvas หาผู้เล่น
        if (menuCanvas.activeSelf)
        {
            Vector3 direction = playerBody.position - menuCanvas.transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                menuCanvas.transform.rotation = Quaternion.LookRotation(-direction);
            }
        }

        // กดปุ่ม B ขณะหยิบอยู่ → confirm
        if (isGrabbed && confirmButton != null && confirmButton.action.WasPressedThisFrame())
        {
            if (alreadyCollected)
            {
                Debug.Log("คุณเลือกเมนูไปแล้ว!");
            }
            else
            {
                CollectItem();
            }
        }
    }

    void CollectItem()
    {
        alreadyCollected = true;

        var grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        inventory.Add(gameObject.name);
        Debug.Log("เลือกเมนู: " + gameObject.name);
        menuCanvas.SetActive(false);
        isGrabbed = false;
        gameObject.SetActive(false);
    }
}