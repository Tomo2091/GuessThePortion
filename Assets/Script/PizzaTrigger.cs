using UnityEngine;

public class PizzaTrigger : MonoBehaviour
{
    [Header("=== References ===")]
    public OvenController ovenController;

    public string pizzaTag = "Pizza";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(pizzaTag))
        {
            Debug.Log("[PizzaTrigger] Pizza placed inside oven!");
            ovenController?.SetPizzaInside(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(pizzaTag))
        {
            Debug.Log("[PizzaTrigger] Pizza removed from oven");
            ovenController?.SetPizzaInside(false);
        }
    }
}
