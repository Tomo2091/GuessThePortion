using UnityEngine;
using EzySlice;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KnifeSlice : MonoBehaviour
{
    public Material sliceMaterial;
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
            var sliceable = other.gameObject.GetComponent<SliceableObject>();
            if (grab.isSelected && sliceable != null && sliceable.CanSlice())
            {
                sliceable.RegisterSlice();
                SliceEqual(other.gameObject, sliceable.sliceCount);
                missionManager?.CompleteSliceSausage();
            }
        }
    }

    void SliceEqual(GameObject target, int sliceNumber)
    {
        Vector3 sliceDirection = (sliceNumber % 2 == 1) ? Vector3.right : Vector3.forward;
        SlicedHull hull = target.Slice(target.transform.position, sliceDirection, sliceMaterial);

        if (hull != null)
        {
            GameObject upper = hull.CreateUpperHull(target, sliceMaterial);
            upper.transform.position = target.transform.position;
            SetupSlicedPiece(upper);

            GameObject lower = hull.CreateLowerHull(target, sliceMaterial);
            lower.transform.position = target.transform.position;
            SetupSlicedPiece(lower);

            Destroy(target);
        }
    }

    void SetupSlicedPiece(GameObject piece)
    {
        piece.AddComponent<BoxCollider>();
        Rigidbody rb = piece.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.linearDamping = 20f;
        rb.angularDamping = 20f;
        rb.maxLinearVelocity = 0.5f;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        piece.tag = "Sausage";
        piece.layer = LayerMask.NameToLayer("Default");
        piece.AddComponent<SliceableObject>();
        piece.AddComponent<XRGrabInteractable>();
    }
}
