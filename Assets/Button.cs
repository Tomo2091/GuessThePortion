using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class Button : MonoBehaviour
{
    [SerializeField] private string SelectDish = "SelectDish";
    public void NewGameButton()
    {
        SceneManager.LoadScene(SelectDish);
    }


}