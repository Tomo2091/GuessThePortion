using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KnifeSlice : MonoBehaviour
{
    public Material sliceMaterial;
    public float sliceForce = 2f;
    public MissionManager missionManager;
    private bool canSlice = false;

    void Start()
    {
        Invoke("EnableSlice", 1f);
    }

    void EnableSlice()
    {
        canSlice = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!canSlice) return;
        if (other.gameObject.CompareTag("Sliceable"))
        {
            var grab = GetComponent<XRGrabInteractable>();
            if (grab.isSelected)
            {
                Slice(other.gameObject);
                // ✅ ติ๊ก Mission 2 เมื่อหั่นสำเร็จ
                missionManager.CompleteMission2();
            }
        }
    }

    void Slice(GameObject target)
    {
        SlicedHull hull = target.Slice(
            transform.position,
            transform.up,
            sliceMaterial
        );

        if (hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target, sliceMaterial);
            SetupSlicedPiece(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, sliceMaterial);
            SetupSlicedPiece(lowerHull);

            Destroy(target);
        }
    }

    void SetupSlicedPiece(GameObject piece)
    {
        piece.AddComponent<BoxCollider>();
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        piece.tag = "Sliceable";
    }
}