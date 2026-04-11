using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    [Header("Settings")]
    public float openAngle = 90f;
    public float openSpeed = 3f;
    public float detectRange = 2f;

    [Header("Canvas")]
    public GameObject doorCanvas; // ลาก Canvas ใส่

    private Transform player;
    private Quaternion closedRot;
    private Quaternion openRot;
    private bool isOpen = false;

    void Start()
    {
        closedRot = transform.rotation;
        openRot = closedRot * Quaternion.Euler(0, openAngle, 0);

        if (doorCanvas != null)
            doorCanvas.SetActive(false);
    }

    void Update()
    {
        if (player == null)
        {
            if (Camera.main != null)
                player = Camera.main.transform;
            else
                return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < detectRange && !isOpen)
        {
            isOpen = true;
            if (doorCanvas != null)
                doorCanvas.SetActive(true);
        }
        else if (distance >= detectRange && isOpen)
        {
            isOpen = false;
            if (doorCanvas != null)
                doorCanvas.SetActive(false);
        }

        Quaternion targetRot = isOpen ? openRot : closedRot;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * openSpeed);
    }
}