using UnityEngine;
using UnityEngine.InputSystem;

public class ChecklistManager : MonoBehaviour
{
    public GameObject checklistPanel;
    public InputActionProperty toggleButton;

    void Start()
    {
        checklistPanel.SetActive(false);
    }

    void OnEnable()
    {
        toggleButton.action.performed += ToggleChecklist;
        toggleButton.action.Enable();
    }

    void OnDisable()
    {
        toggleButton.action.performed -= ToggleChecklist;
    }

    void ToggleChecklist(InputAction.CallbackContext ctx)
    {
        checklistPanel.SetActive(!checklistPanel.activeSelf);
    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
        {
            checklistPanel.SetActive(!checklistPanel.activeSelf);
        }
    }
}