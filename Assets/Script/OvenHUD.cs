using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OvenHUD : MonoBehaviour
{
    [Header("=== UI References ===")]
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI statusText;
    public Image progressBar;

    [Header("=== Colors ===")]
    public Color idleColor = Color.white;
    public Color bakingColor = new Color(1f, 0.5f, 0f);
    public Color doneColor = Color.green;

    void OnEnable()
    {
        OvenController.OnTimerUpdate += UpdateTimer;
        OvenController.OnBakingDone += ShowDone;
        OvenController.OnStateChanged += UpdateStatus;
    }

    void OnDisable()
    {
        OvenController.OnTimerUpdate -= UpdateTimer;
        OvenController.OnBakingDone -= ShowDone;
        OvenController.OnStateChanged -= UpdateStatus;
    }

    void Start()
    {
        SetHUDVisible(false);
    }

    void UpdateTimer(float elapsed, float total)
    {
        SetHUDVisible(true);
        float remaining = total - elapsed;
        int minutes = Mathf.FloorToInt(remaining / 60f);
        int seconds = Mathf.FloorToInt(remaining % 60f);
        if (timerText != null)
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        if (progressBar != null)
            progressBar.fillAmount = elapsed / total;
    }

    void ShowDone()
    {
        if (timerText != null) { timerText.text = "00:00"; timerText.color = doneColor; }
        if (statusText != null) { statusText.text = "Pizza is ready!"; statusText.color = doneColor; }
    }

    void UpdateStatus(OvenController.OvenState state)
    {
        if (statusText == null) return;
        switch (state)
        {
            case OvenController.OvenState.Idle:
                statusText.text = "Hold dough and turn the knob to bake";
                statusText.color = idleColor;
                SetHUDVisible(false);
                break;
            case OvenController.OvenState.Baking:
                statusText.text = "Baking...";
                statusText.color = bakingColor;
                if (timerText != null) timerText.color = bakingColor;
                SetHUDVisible(true);
                break;
            case OvenController.OvenState.Done:
                break;
        }
    }

    void SetHUDVisible(bool visible)
    {
        foreach (Transform child in transform)
            child.gameObject.SetActive(visible);
    }
}