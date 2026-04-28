using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RollingPinKnead : MonoBehaviour
{
    public MissionManager missionManager;
    public GameObject doughObject;
    public float applyRange = 0.5f;
    public float holdTime = 3f;
    public float flatScaleY = 0.2f;

    private XRGrabInteractable _grab;
    private float _holdTimer = 0f;
    private bool _done = false;
    private Vector3 _doughOriginalScale;

    void Start()
    {
        _grab = GetComponent<XRGrabInteractable>();
        if (doughObject != null)
            _doughOriginalScale = doughObject.transform.localScale;
    }

    void Update()
    {
        if (_done) return;
        if (_grab == null || !_grab.isSelected) return;
        if (doughObject == null) return;

        float dist = Vector3.Distance(transform.position, doughObject.transform.position);
        if (dist <= applyRange)
        {
            _holdTimer += Time.deltaTime;
            float progress = Mathf.Clamp01(_holdTimer / holdTime);
            float targetY = Mathf.Lerp(_doughOriginalScale.y, flatScaleY, progress);
            Vector3 s = doughObject.transform.localScale;
            float expand = 1f + progress * 1.5f;
            doughObject.transform.localScale = new Vector3(
                _doughOriginalScale.x * expand,
                targetY,
                _doughOriginalScale.z * expand
            );
            if (_holdTimer >= holdTime)
            {
                _done = true;
                missionManager?.CompleteKneadDough();
                Debug.Log("[RollingPin] Done!");
            }
        }
        else
        {
            _holdTimer = 0f;
            doughObject.transform.localScale = _doughOriginalScale;
        }
    }
}