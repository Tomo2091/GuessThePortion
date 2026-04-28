using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MissionManager : MonoBehaviour
{
    [Header("=== Mission Toggles (UI) ===")]
    public Toggle kneadDoughToggle;
    public Toggle applySauceToggle;
    public Toggle sliceSausageToggle;
    public Toggle placeToppingToggle;
    public Toggle bakeToggle;

    [Header("=== Status Text (optional) ===")]
    public TextMeshProUGUI statusText;

    [Header("=== Pizza Objects ===")]
    public GameObject pizzaRawObject;
    public GameObject pizzaCookedObject;

    [Header("=== Oven ===")]
    public OvenController ovenController;

    private bool _doughKneaded = false;
    private bool _sauceApplied = false;
    private bool _sausageSliced = false;
    private bool _toppingPlaced = false;

    void Start()
    {
        if (pizzaCookedObject != null) pizzaCookedObject.SetActive(false);
        if (pizzaRawObject != null) pizzaRawObject.SetActive(true);
        ResetAllToggles();
        UpdateStatus("Step 1: Knead the dough with the rolling pin");
    }

    public void CompleteKneadDough()
    {
        if (_doughKneaded) return;
        _doughKneaded = true;
        if (kneadDoughToggle != null) kneadDoughToggle.isOn = true;
        UpdateStatus("Step 2: Apply sauce to the dough");
        Debug.Log("[Mission] Dough kneaded!");
    }

    public void CompleteApplySauce()
    {
        if (!_doughKneaded || _sauceApplied) return;
        _sauceApplied = true;
        if (applySauceToggle != null) applySauceToggle.isOn = true;
        UpdateStatus("Step 3: Slice the sausage");
        Debug.Log("[Mission] Sauce applied!");
    }

    public void CompleteSliceSausage()
    {
        if (_sausageSliced) return;
        _sausageSliced = true;
        if (sliceSausageToggle != null) sliceSausageToggle.isOn = true;
        UpdateStatus("Step 4: Place sausage on the dough");
        Debug.Log("[Mission] Sausage sliced!");
    }

    public void CompletePlaceTopping()
    {
        if (!_sausageSliced || _toppingPlaced) return;
        _toppingPlaced = true;
        if (placeToppingToggle != null) placeToppingToggle.isOn = true;
        UpdateStatus("Ready! Turn the knob to start baking");
        Debug.Log("[Mission] Topping placed!");

        // Pizza ready — no need to place in oven manually
        if (ovenController != null)
            ovenController.SetPizzaInside(true);
    }

    public void CompleteBaking()
    {
        if (bakeToggle != null) bakeToggle.isOn = true;
        if (pizzaRawObject != null) pizzaRawObject.SetActive(false);
        if (pizzaCookedObject != null) pizzaCookedObject.SetActive(true);
        UpdateStatus("Pizza is ready!");
        Debug.Log("[Mission] Baking complete!");
    }

    public bool IsReadyToBake()
    {
        return _doughKneaded && _sauceApplied && _sausageSliced && _toppingPlaced;
    }

    void ResetAllToggles()
    {
        if (kneadDoughToggle != null) kneadDoughToggle.isOn = false;
        if (applySauceToggle != null) applySauceToggle.isOn = false;
        if (sliceSausageToggle != null) sliceSausageToggle.isOn = false;
        if (placeToppingToggle != null) placeToppingToggle.isOn = false;
        if (bakeToggle != null) bakeToggle.isOn = false;
    }

    void UpdateStatus(string message)
    {
        if (statusText != null) statusText.text = message;
        Debug.Log("[Mission Status] " + message);
    }
}