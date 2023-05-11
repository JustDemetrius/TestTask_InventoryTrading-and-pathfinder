using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemNameTMP;
    [SerializeField] private TextMeshProUGUI _itemCostAmmountTMP;
    [SerializeField] private Image _itemImage;

    public int ItemPrice { get; private set; }
    public ItemScriptableData ItemData { get; private set; }

    public void InitItem(ItemScriptableData data, float priceMupliplier = 0f)
    {
        if (data == null)
        {
            Debug.LogError($"Item data is NULL for object:  {gameObject.name}");
            return;
        }

        ItemData = data;
        ItemPrice = ItemData.ItemCost;
        _itemNameTMP.text = ItemData.ItemName;
        _itemCostAmmountTMP.text = $"{ItemPrice} g";
        _itemImage.sprite = ItemData.ItemIcon;

        if (priceMupliplier != 0f)
            UpdateItemCost(priceMupliplier);
    }

    // its will be used tu multiply item cost when selling to player while relan item cost won't be changing
    public void UpdateItemCost(float valueMultiplier)
    {
        float updatedPrice = valueMultiplier != 0f ? ItemData.ItemCost * valueMultiplier : ItemData.ItemCost;

        if (updatedPrice >= 0f)
        {
            ItemPrice = Mathf.FloorToInt(updatedPrice);
            _itemCostAmmountTMP.text = $"{ItemPrice} g";
        }
        else
            Debug.LogError($"Price can't be negative:  {gameObject.name}");

    }
}
