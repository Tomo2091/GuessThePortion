using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class SauceApplicator : MonoBehaviour
{
    [Header("=== References ===")]
    public MissionManager missionManager;
    public GameObject doughObject;
    public SauceVisual sauceVisual;

    [Header("=== Settings ===")]
    public float applyRange = 0.5f;
    public float holdTime = 2f;

    private XRGrabInteractable _grab;
    private float _holdTimer = 0f;
    private bool _applied = false;

    void Start()
    {
        _grab = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (_applied) return;
        if (_grab == null || !_grab.isSelected) return;
        if (doughObject == null) return;

        float dist = Vector3.Distance(transform.position, doughObject.transform.position);
        if (dist <= applyRange)
        {
            _holdTimer += Time.deltaTime;
            Debug.Log("[Sauce] Applying: " + _holdTimer.ToString("F1") + "s");
            if (_holdTimer >= holdTime)
            {
                _applied = true;
                sauceVisual?.ApplySauce();
                missionManager?.CompleteApplySauce();
                Debug.Log("[Sauce] Sauce applied!");
            }
        }
        else
        {
            _holdTimer = 0f;
        }
    }
}