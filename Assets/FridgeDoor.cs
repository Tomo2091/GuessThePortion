using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class FridgeDoor : MonoBehaviour
{
    public Transform pivotPoint;
    public float openAngle = 90f;
    public float speed = 2f;
    private bool isOpen = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    void Start()
    {
        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(Toggle);
    }

    void Toggle(SelectEnterEventArgs args)
    {
        isOpen = !isOpen;
        targetAngle = isOpen ? openAngle : 0f;
    }

    void Update()
    {
        if (Mathf.Abs(currentAngle - targetAngle) > 0.01f)
        {
            float angleDelta = Mathf.Lerp(currentAngle, targetAngle,
                               Time.deltaTime * speed) - currentAngle;
            transform.RotateAround(pivotPoint.position, Vector3.up, angleDelta);
            currentAngle += angleDelta;
        }
    }
}