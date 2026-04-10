using UnityEngine;

public class ToggleTabWhenLookUp : MonoBehaviour
{
    [Header("References")]
    public Transform xrCamera;
    public GameObject tabUI;
    public GameObject gameMenuCanvas; // ลาก gamplay-panel ใส่

    [Header("Angles")]
    public float openAngle = 30f;
    public float closeAngle = 15f;

    [Header("Timing")]
    public float holdTime = 0.2f;

    private float lookTimer = 0f;
    private bool isOpen = false;

    void Start()
    {
        if (tabUI != null)
            tabUI.SetActive(false);
    }

    void Update()
    {
        if (xrCamera == null || tabUI == null)
            return;

        float pitch = xrCamera.eulerAngles.x;

        if (pitch > 180f)
            pitch -= 360f;

        bool isLookingUp = pitch <= -openAngle;
        bool isLookingForwardEnoughToClose = pitch >= -closeAngle;

        if (!isOpen)
        {
            if (isLookingUp)
            {
                lookTimer += Time.deltaTime;

                if (lookTimer >= holdTime)
                {
                    tabUI.SetActive(true);
                    isOpen = true;
                    lookTimer = 0f;

                    // ซ่อน game menu
                    if (gameMenuCanvas != null)
                        gameMenuCanvas.SetActive(false);
                }
            }
            else
            {
                lookTimer = 0f;
            }
        }
        else
        {
            if (isLookingForwardEnoughToClose)
            {
                tabUI.SetActive(false);
                isOpen = false;

                // เปิด game menu กลับ
                if (gameMenuCanvas != null)
                    gameMenuCanvas.SetActive(true);
            }
        }
    }
}