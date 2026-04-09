using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    public Toggle pickupKnifeToggle;
    public Toggle sliceAppleToggle;
    public Toggle placeOnPlateToggle;

    public void CompleteMission1()
    {
        Debug.Log("Mission 1 Complete!");
        pickupKnifeToggle.isOn = true;
    }

    public void CompleteMission2()
    {
        Debug.Log("Mission 2 Complete!");
        sliceAppleToggle.isOn = true;
    }

    public void CompleteMission3()
    {
        Debug.Log("Mission 3 Complete!");
        placeOnPlateToggle.isOn = true;
    }
}