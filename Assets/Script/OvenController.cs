using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class OvenController : MonoBehaviour
{
    [Header("=== Oven Parts ===")]
    public Transform ovenDoor;
    public GameObject[] burnerLights;
    public ParticleSystem heatEffect;

    [Header("=== Door Settings ===")]
    public Vector3 doorClosedRotation = Vector3.zero;
    public Vector3 doorOpenRotation = new Vector3(0f, 0f, -90f);
    public float doorAnimSpeed = 2f;

    [Header("=== Timer Settings ===")]
    public float bakingTime = 30f;

    [Header("=== Pizza Objects ===")]
    public GameObject pizzaRawObject;
    public GameObject pizzaCookedObject;
    public Transform pizzaSnapPoint;
    public Transform pizzaOutputPoint;
    public float snapRange = 1.5f;

    [Header("=== Audio ===")]
    public AudioSource audioSource;
    public AudioClip ovenStartSound;
    public AudioClip ovenDoneSound;

    public enum OvenState { Idle, Baking, Done }
    public OvenState currentState = OvenState.Idle;

    public static event System.Action<float, float> OnTimerUpdate;
    public static event System.Action OnBakingDone;
    public static event System.Action<OvenState> OnStateChanged;

    void Start()
    {
        SetBurners(false);
        if (heatEffect != null) heatEffect.Stop();
        if (pizzaCookedObject != null) pizzaCookedObject.SetActive(false);
    }

    public void OnKnobTurned()
    {
        if (currentState != OvenState.Idle) return;

        if (!TrySnapPizza())
        {
            Debug.Log("[Oven] Place dough closer to the oven first!");
            return;
        }

        StartCoroutine(BakingSequence());
    }

    bool TrySnapPizza()
    {
        if (pizzaRawObject == null) return false;

        float dist = Vector3.Distance(pizzaRawObject.transform.position, transform.position);
        if (dist > snapRange) return false;

        // Snap เข้า oven
        if (pizzaSnapPoint != null)
        {
            pizzaRawObject.transform.position = pizzaSnapPoint.position;
            pizzaRawObject.transform.rotation = pizzaSnapPoint.rotation;
        }

        // Disable grab
        var grab = pizzaRawObject.GetComponent<XRGrabInteractable>();
        if (grab != null) grab.enabled = false;

        Debug.Log("[Oven] Pizza snapped into oven!");
        return true;
    }

    IEnumerator BakingSequence()
    {
        // ปิดประตู
        yield return StartCoroutine(AnimateDoor(doorClosedRotation));

        // ติดไฟ
        SetBurners(true);
        if (heatEffect != null) heatEffect.Play();
        PlaySound(ovenStartSound);
        SetState(OvenState.Baking);

        // นับเวลา
        float timer = 0f;
        while (timer < bakingTime)
        {
            timer += Time.deltaTime;
            OnTimerUpdate?.Invoke(timer, bakingTime);
            yield return null;
        }

        // เสร็จ
        SetBurners(false);
        if (heatEffect != null) heatEffect.Stop();
        PlaySound(ovenDoneSound);

        // ซ่อน raw
        if (pizzaRawObject != null) pizzaRawObject.SetActive(false);

        // เปิดประตู
        yield return StartCoroutine(AnimateDoor(doorOpenRotation));

        // แสดง cooked pizza
        if (pizzaCookedObject != null)
        {
            pizzaCookedObject.SetActive(true);
            if (pizzaOutputPoint != null)
            {
                pizzaCookedObject.transform.position = pizzaOutputPoint.position;
                pizzaCookedObject.transform.rotation = pizzaOutputPoint.rotation;
            }
        }

        SetState(OvenState.Done);
        OnBakingDone?.Invoke();

        var mm = FindObjectOfType<MissionManager>();
        if (mm != null) mm.CompleteBaking();

        Debug.Log("[Oven] Baking complete!");
    }

    IEnumerator AnimateDoor(Vector3 targetRotation)
    {
        if (ovenDoor == null) yield break;
        Quaternion startRot = ovenDoor.localRotation;
        Quaternion targetRot = Quaternion.Euler(targetRotation);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * doorAnimSpeed;
            ovenDoor.localRotation = Quaternion.Lerp(startRot, targetRot, t);
            yield return null;
        }
        ovenDoor.localRotation = targetRot;
    }

    public void SetPizzaInside(bool inside) { }

    void SetBurners(bool on)
    {
        foreach (var b in burnerLights)
            if (b != null) b.SetActive(on);
    }

    void SetState(OvenState newState)
    {
        currentState = newState;
        OnStateChanged?.Invoke(newState);
    }

    void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }

    public void ResetOven()
    {
        StopAllCoroutines();
        SetBurners(false);
        if (heatEffect != null) heatEffect.Stop();
        if (ovenDoor != null) ovenDoor.localRotation = Quaternion.Euler(doorOpenRotation);
        if (pizzaRawObject != null)
        {
            pizzaRawObject.SetActive(true);
            var g = pizzaRawObject.GetComponent<XRGrabInteractable>();
            if (g != null) g.enabled = true;
        }
        if (pizzaCookedObject != null) pizzaCookedObject.SetActive(false);
        SetState(OvenState.Idle);
    }
}