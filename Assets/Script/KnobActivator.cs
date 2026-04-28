using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KnobActivator : MonoBehaviour
{
    [Header("=== References ===")]
    public OvenController ovenController;

    [Header("=== Trigger Mode ===")]
    public TriggerMode triggerMode = TriggerMode.OnRotationThreshold;
    public float rotationThreshold = 45f;

    public enum TriggerMode { OnGrab, OnRelease, OnRotationThreshold }

    private XRGrabInteractable _grabInteractable;
    private Quaternion _initialRotation;
    private bool _activated = false;

    void Start()
    {
        _grabInteractable = GetComponent<XRGrabInteractable>();
        _initialRotation = transform.localRotation;

        if (_grabInteractable != null)
        {
            if (triggerMode == TriggerMode.OnGrab)
                _grabInteractable.selectEntered.AddListener(_ => TriggerOven());
            if (triggerMode == TriggerMode.OnRelease)
                _grabInteractable.selectExited.AddListener(_ => TriggerOven());
        }
    }

    void Update()
    {
        if (triggerMode == TriggerMode.OnRotationThreshold && !_activated)
        {
            float angle = Quaternion.Angle(_initialRotation, transform.localRotation);
            if (angle >= rotationThreshold)
                TriggerOven();
        }
    }

    void TriggerOven()
    {
        if (_activated) return;
        _activated = true;
        if (ovenController != null)
            ovenController.OnKnobTurned();
        else
            Debug.LogWarning("[KnobActivator] OvenController not assigned!");
    }

    public void ResetActivation()
    {
        _activated = false;
        transform.localRotation = _initialRotation;
    }
}
