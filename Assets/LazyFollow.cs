using UnityEngine;

public class LazyFollowUI : MonoBehaviour
{
    [Header("Target")]
    public Transform cameraTransform; // ลาก Main Camera มาใส่

    [Header("Position Settings")]
    public float followDistance = 2f;     // ระยะห่างจากกล้อง
    public float heightOffset = 0f;       // ขยับสูงต่ำ
    public float positionSmoothSpeed = 5f; // ความนุ่มนวลของการเลื่อนตาม

    [Header("Rotation Settings")]
    public float rotationSmoothSpeed = 3f;  // ความนุ่มนวลของการหมุน
    public float deadZoneDegrees = 20f;     // องศาที่ยอมให้หมุนก่อน UI จะตาม

    private Quaternion targetRotation;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        // ตั้งตำแหน่งเริ่มต้น
        transform.position = GetTargetPosition();
        targetRotation = GetFacingRotation();
        transform.rotation = targetRotation;
    }

    void Update()
    {
        // === ตำแหน่ง: เลื่อนตามกล้องแบบ smooth ===
        Vector3 targetPos = GetTargetPosition();
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * positionSmoothSpeed);

        // === การหมุน: ตามเมื่อหันออกนอก deadzone ===
        Quaternion desiredRotation = GetFacingRotation();
        float angleDiff = Quaternion.Angle(targetRotation, desiredRotation);

        if (angleDiff > deadZoneDegrees)
        {
            targetRotation = desiredRotation;
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothSpeed);
    }

    Vector3 GetTargetPosition()
    {
        return cameraTransform.position
             + cameraTransform.forward * followDistance
             + Vector3.up * heightOffset;
    }

    Quaternion GetFacingRotation()
    {
        // UI หันหน้าเข้าหากล้องเสมอ
        Vector3 dirToCamera = cameraTransform.position - transform.position;
        return Quaternion.LookRotation(-dirToCamera);
    }
}