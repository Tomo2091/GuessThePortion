using UnityEngine;

public class ToppingPlacer : MonoBehaviour
{
    [Header("=== References ===")]
    public MissionManager missionManager;

    public string sausageTag = "Sausage";

    private bool _placed = false;

    void OnTriggerEnter(Collider other)
    {
        if (_placed) return;
        if (other.CompareTag(sausageTag))
        {
            _placed = true;
            missionManager?.CompletePlaceTopping();
            Debug.Log("[Topping] Sausage placed on dough!");
        }
    }
}
