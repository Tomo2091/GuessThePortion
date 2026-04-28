using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using TMPro;

public class ShowMenuOnGrab : MonoBehaviour
{
    public GameObject menuCanvas;
    public InputActionReference confirmButton;

    public GameObject missionSign;
    public TextMeshProUGUI missionText;

    [Header("Mission Info")]
    public string menuName;
    public int customerCount;

    private bool isGrabbed = false;
    private Transform playerBody;

    private static GameObject sharedMissionSign;
    private static TextMeshProUGUI sharedMissionText;
    private static GameObject previousScroll;
    private static Vector3 previousScrollPos;
    private static Quaternion previousScrollRot;
    private static Transform previousScrollParent;

    private Vector3 originalPos;
    private Quaternion originalRot;
    private Transform originalParent;

    void Start()
    {
        menuCanvas.SetActive(false);

        originalPos = transform.position;
        originalRot = transform.rotation;
        originalParent = transform.parent;

        if (missionSign != null) sharedMissionSign = missionSign;
        if (missionText != null) sharedMissionText = missionText;

        if (sharedMissionSign != null)
            sharedMissionSign.SetActive(false);

        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        if (confirmButton != null && confirmButton.action != null)
        {
            confirmButton.action.Enable();
        }
    }

    void OnDisable()
    {

    }

    void OnGrab(SelectEnterEventArgs args)
    {
        menuCanvas.SetActive(true);
        isGrabbed = true;

        if (confirmButton != null && confirmButton.action != null)
            confirmButton.action.Enable();
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

        if (menuCanvas.activeSelf)
        {
            Vector3 direction = playerBody.position - menuCanvas.transform.position;
            if (direction.sqrMagnitude > 0.001f)
            {
                menuCanvas.transform.rotation = Quaternion.LookRotation(-direction);
            }
        }

        if (isGrabbed && confirmButton != null && confirmButton.action.WasPressedThisFrame())
        {
            CollectItem();
        }
    }

    void CollectItem()
    {
        PlayerPrefs.SetString("menuName", menuName);
        PlayerPrefs.SetInt("customerCount", customerCount);
        PlayerPrefs.Save();
        // วางอันเก่าคืน
        if (previousScroll != null && previousScroll != gameObject)
        {
            previousScroll.SetActive(true);
            previousScroll.transform.SetParent(previousScrollParent);
            previousScroll.transform.position = previousScrollPos;
            previousScroll.transform.rotation = previousScrollRot;

            // reset rigidbody
            var oldRb = previousScroll.GetComponent<Rigidbody>();
            if (oldRb != null)
            {
                oldRb.linearVelocity = Vector3.zero;
                oldRb.angularVelocity = Vector3.zero;
                oldRb.isKinematic = true;
            }

            // เปิด grab กลับ
            var oldGrab = previousScroll.GetComponent<XRGrabInteractable>();
            if (oldGrab != null) oldGrab.enabled = true;

            // เปิด menu canvas กลับให้พร้อมใช้
            var oldScript = previousScroll.GetComponent<ShowMenuOnGrab>();
            if (oldScript != null) oldScript.menuCanvas.SetActive(false);

             
        }

        // จำตัวนี้ไว้
        previousScroll = gameObject;
        previousScrollPos = originalPos;
        previousScrollRot = originalRot;
        previousScrollParent = originalParent;

        // ปิด grab + ซ่อน
        var grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;
        menuCanvas.SetActive(false);
        isGrabbed = false;
        gameObject.SetActive(false);

        // อัพเดทป้าย mission
        if (sharedMissionSign != null)
            sharedMissionSign.SetActive(true);

        if (sharedMissionText != null)
            sharedMissionText.text = menuName + "\n" + customerCount + " Customers";

        Debug.Log("เลือกเมนู: " + menuName);
    }
}