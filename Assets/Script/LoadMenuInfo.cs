using UnityEngine;
using TMPro;

public class LoadMenuInfo : MonoBehaviour
{
    public TextMeshProUGUI menuText;

    void Start()
    {
        string menu = PlayerPrefs.GetString("menuName", "No menu");
        int customers = PlayerPrefs.GetInt("customerCount", 0);

        if (menuText != null)
            menuText.text = menu + "\n" + customers + " Customers";
    }
}