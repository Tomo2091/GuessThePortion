using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using TMPro;
using System.Collections.Generic;

public class Basket : MonoBehaviour
{
    public TextMeshProUGUI basketListText;
    public GameObject basketCanvas;

    private Dictionary<string, int> itemCounts = new Dictionary<string, int>();
    private Dictionary<string, float> itemPrices = new Dictionary<string, float>();
    private float totalPrice = 0f;
    private int totalItems = 0;
    private GrabBasket grabBasket;

    void Start()
    {
        grabBasket = GetComponent<GrabBasket>();

        if (basketCanvas != null)
            basketCanvas.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        var item = other.GetComponent<ShopItem>();
        if (item == null) return;

        var grab = other.GetComponent<XRGrabInteractable>();
        if (grab != null && grab.isSelected) return;

        var rb = other.GetComponent<Rigidbody>();
        if (rb == null || rb.isKinematic) return;

        // นับจำนวน
        if (itemCounts.ContainsKey(item.itemName))
        {
            itemCounts[item.itemName]++;
        }
        else
        {
            itemCounts[item.itemName] = 1;
            itemPrices[item.itemName] = item.price;
        }

        totalPrice += item.price;
        totalItems++;

        if (basketCanvas != null)
            basketCanvas.SetActive(true);

        UpdateUI();

        if (grab != null) grab.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;

        other.transform.SetParent(transform);
        other.transform.localScale = Vector3.one * 0.2f;
        other.transform.localRotation = Quaternion.identity;

        float radius = 0.05f;
        float angle = (totalItems - 1) * 72f;
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float y = 0.05f + (Mathf.Floor((totalItems - 1) / 5f) * 0.02f);
        other.transform.localPosition = new Vector3(x, y, z);

        Debug.Log("ใส่ตะกร้า: " + item.itemName + " x" + itemCounts[item.itemName]);
    }

    void UpdateUI()
    {
        if (basketListText != null)
        {
            string list = "";
            foreach (var pair in itemCounts)
            {
                string name = pair.Key;
                int count = pair.Value;
                float price = itemPrices[name] * count;
                list += name + " x" + count + "  £" + price.ToString("F2") + "\n";
            }
            list += "\n--------------------";
            list += "\nTotal: £" + totalPrice.ToString("F2");
            list += "\n" + totalItems + " items";
            basketListText.text = list;
        }
    }

    public string GetSummary()
    {
        string summary = "";
        foreach (var pair in itemCounts)
        {
            string name = pair.Key;
            int count = pair.Value;
            float price = itemPrices[name] * count;
            summary += name + " x" + count + "  £" + price.ToString("F2") + "\n";
        }
        summary += "\nTotal: £" + totalPrice.ToString("F2");
        summary += "\n" + totalItems + " items";
        return summary;
    }

    public float GetTotalPrice()
{
    return totalPrice;
}
}