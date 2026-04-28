using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{
    public TextMeshProUGUI moneyText;
    public float startingMoney = 50f;

    void Start()
    {
        // ถ้ายังไม่เคยตั้งค่า ให้ใช้ startingMoney
        if (!PlayerPrefs.HasKey("money"))
        {
            PlayerPrefs.SetFloat("money", startingMoney);
            PlayerPrefs.Save();
        }

        UpdateMoneyDisplay();
    }

    public void UpdateMoneyDisplay()
    {
        float money = PlayerPrefs.GetFloat("money", startingMoney);

        if (moneyText != null)
            moneyText.text = "£ " + money.ToString("F2");
    }

    // เรียกตอนอยากรีเซ็ตเงิน
    public void ResetMoney()
    {
        PlayerPrefs.SetFloat("money", startingMoney);
        PlayerPrefs.Save();
        UpdateMoneyDisplay();
    }
}