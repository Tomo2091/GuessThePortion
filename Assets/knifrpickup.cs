using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class KnifePickup : MonoBehaviour
{
    public MissionManager missionManager;

    void Start()
    {
        Debug.Log("KnifePickup Start");
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnPickup);
    }

    void OnPickup(SelectEnterEventArgs args)
    {
        Debug.Log("Knife Picked Up!");
        if (missionManager != null)
        {
            missionManager.CompleteMission1();
        }
        else
        {
            Debug.Log("MissionManager is NULL!");
        }
    }
}