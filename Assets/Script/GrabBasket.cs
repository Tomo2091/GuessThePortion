using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;

public class GrabBasket : MonoBehaviour
{
    public InputActionReference confirmButton;
    public InputActionReference placeButton;
    public Transform leftHand;
    public Transform counterPoint;
    public GameObject bagPrefab;
    public GameObject npc;
    public Material happyFaceMaterial;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI floatingText;
    public GameObject basketCanvas;

    private bool isGrabbed = false;
    private bool isAttached = false;
    private bool isPlaced = false;

    void Start()
    {
        var grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrab);
        grab.selectExited.AddListener(OnRelease);
    }

    void OnEnable()
    {
        if (confirmButton != null)
            confirmButton.action.Enable();
        if (placeButton != null)
            placeButton.action.Enable();
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }

    void OnRelease(SelectExitEventArgs args)
    {
        if (!isAttached)
            isGrabbed = false;
    }

    void Update()
    {
        if (isGrabbed && !isAttached && confirmButton != null && confirmButton.action.WasPressedThisFrame())
        {
            AttachToLeftHand();
        }

        if (isAttached && !isPlaced && leftHand != null)
        {
            transform.position = leftHand.position + leftHand.forward * 0.7f + leftHand.up * -0.7f + leftHand.right * -0.2f;
            transform.rotation = leftHand.rotation;
        }

        if (isAttached && !isPlaced && placeButton != null && placeButton.action.WasPressedThisFrame())
        {
            PlaceOnCounter();
        }
    }

    void AttachToLeftHand()
    {
        isAttached = true;
        transform.localScale = Vector3.one * 2f;

        var grab = GetComponent<XRGrabInteractable>();
        grab.enabled = false;

        var rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log("ตะกร้าติดมือซ้ายแล้ว!");
    }

    void PlaceOnCounter()
    {
        if (counterPoint == null)
        {
            Debug.Log("ยังไม่ได้ตั้ง counter point!");
            return;
        }

        isPlaced = true;
        transform.position = counterPoint.position;
        transform.rotation = counterPoint.rotation;

        Debug.Log("วางตะกร้าบน counter แล้ว!");

        StartCoroutine(SwapToBag());
    }

    IEnumerator SwapToBag()
{
    yield return new WaitForSeconds(1f);

    // เปิดถุง + ซ่อนตะกร้าพร้อมกัน
    if (bagPrefab != null)
    {
        bagPrefab.SetActive(true);

        var grabBag = bagPrefab.GetComponent<GrabBag>();
        if (grabBag != null)
        {
            grabBag.leftHand = leftHand;
            grabBag.confirmButton = confirmButton;
        }
    }

    // ซ่อน mesh ตะกร้าทันที แต่ยัง active เพื่อให้ coroutine ทำงานต่อ
    foreach (var r in GetComponentsInChildren<Renderer>())
        r.enabled = false;

    // ซ่อน basket canvas
    if (basketCanvas != null)
        basketCanvas.SetActive(false);

    // หักเงิน
    var basket = GetComponent<Basket>();
    if (basket != null && moneyText != null)
    {
        float currentMoney = PlayerPrefs.GetFloat("money", 200f);
        float spent = basket.GetTotalPrice();
        float remaining = currentMoney - spent;

        PlayerPrefs.SetFloat("money", remaining);
        PlayerPrefs.Save();

        moneyText.text = "£ " + remaining.ToString("F2");

        if (floatingText != null)
        {
            floatingText.text = "- £" + spent.ToString("F2");
            floatingText.gameObject.SetActive(true);
        }

        Debug.Log("หักเงิน £" + spent + " เหลือ £" + remaining);
    }

    // เปลี่ยนหน้า NPC
    if (npc != null && happyFaceMaterial != null)
    {
        var renderer = npc.GetComponentInChildren<SkinnedMeshRenderer>();
        var meshRenderer = npc.GetComponentInChildren<MeshRenderer>();

        if (renderer != null)
        {
            var mats = renderer.materials;
            mats[0] = happyFaceMaterial;
            renderer.materials = mats;
        }
        else if (meshRenderer != null)
        {
            var mats = meshRenderer.materials;
            mats[0] = happyFaceMaterial;
            meshRenderer.materials = mats;
        }
    }

    Debug.Log("เปลี่ยนเป็นถุงแล้ว!");

    // floating text animation
    if (floatingText != null)
    {
        Vector3 startPos = floatingText.transform.position;
        Color startColor = floatingText.color;
        float duration = 1.5f;
        float timer = 0f;
        float bounceHeight = 0.8f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            float yOffset = bounceHeight * Mathf.Sin(t * Mathf.PI);
            float xShake = Mathf.Sin(timer * 20f) * 0.02f * (1f - t);

            floatingText.transform.position = startPos + new Vector3(xShake, yOffset, 0);

            float alpha = t > 0.7f ? 1f - ((t - 0.7f) / 0.3f) : 1f;
            floatingText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            float scale = t < 0.2f ? Mathf.Lerp(1f, 3f, t / 0.2f) : Mathf.Lerp(3f, 2f, (t - 0.2f) / 0.8f);
            floatingText.transform.localScale = Vector3.one * scale;

            yield return null;
        }

        floatingText.gameObject.SetActive(false);
        floatingText.color = startColor;
        floatingText.transform.localScale = Vector3.one;
    }

    gameObject.SetActive(false);
}

    public bool IsAttached()
    {
        return isAttached;
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }
}