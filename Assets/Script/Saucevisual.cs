using UnityEngine;

public class SauceVisual : MonoBehaviour
{
    [Header("=== References ===")]
    [Tooltip("SauceLayer object (Cylinder แดงๆ บนแป้ง) ซ่อนไว้ตอนเริ่ม")]
    public GameObject sauceLayer;

    void Start()
    {
        if (sauceLayer != null)
            sauceLayer.SetActive(false);
    }

    public void ApplySauce()
    {
        if (sauceLayer != null)
            sauceLayer.SetActive(true);
        Debug.Log("[SauceVisual] Sauce applied!");
    }
}