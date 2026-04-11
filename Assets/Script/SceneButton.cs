using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class Button : MonoBehaviour
{
    [SerializeField] private string SceneName ;
    public void NewGameButton()
    {
        SceneManager.LoadScene(SceneName);
    }
}