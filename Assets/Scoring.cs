using UnityEngine;

public class Scoring : MonoBehaviour
{
    public int FoodScore;
    public int MoneyAmount;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
        
    {
       if(collision.gameObject.tag == "Food")
            {
            FoodScore -= MoneyAmount;
           
            
        }
    }

}
