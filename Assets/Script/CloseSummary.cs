using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.InputSystem;
using System.Collections;
using TMPro;


public class CloseSummary : MonoBehaviour
{
    public InputActionReference confirmButton;

    public GameObject summaryCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmButton.action.WasPressedThisFrame())
    {
        ClosePanel();
    }
    }

    void ClosePanel()
{
    summaryCanvas.SetActive(false);
}
}
