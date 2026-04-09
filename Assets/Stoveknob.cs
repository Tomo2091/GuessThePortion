using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StoveKnob : MonoBehaviour
{
    public float rotateAngle = 90f;
    public float speed = 3f;
    public Light stoveLight;
    private bool isOn = false;
    private float currentAngle = 0f;
    private float targetAngle = 0f;

    void Start()
    {
        // ª‘¥‰øµÕπ‡√‘Ë¡‡°¡
        if (stoveLight != null)
            stoveLight.enabled = false;

        var interactable = GetComponent<XRSimpleInteractable>();
        interactable.selectEntered.AddListener(Toggle);
    }

    void Toggle(SelectEnterEventArgs args)
    {
        isOn = !isOn;
        targetAngle = isOn ? rotateAngle : 0f;

        // ‡ª‘¥ª‘¥‰ø
        if (stoveLight != null)
            stoveLight.enabled = isOn;
    }

    void Update()
    {
        if (Mathf.Abs(currentAngle - targetAngle) > 0.01f)
        {
            float delta = Mathf.Lerp(currentAngle, targetAngle,
                          Time.deltaTime * speed) - currentAngle;
            transform.Rotate(Vector3.forward, delta);
            currentAngle += delta;
        }
    }
}