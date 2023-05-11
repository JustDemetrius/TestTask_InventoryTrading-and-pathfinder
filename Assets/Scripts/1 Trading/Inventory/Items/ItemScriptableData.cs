using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newItem", menuName = "Item")]
public class ItemScriptableData : ScriptableObject
{
    [SerializeField] private string _itemName;
    [SerializeField] private uint _itemCost;
    [SerializeField] private Sprite _itemSprite;

    public string ItemName => _itemName;
    public int ItemCost => (int)_itemCost;
    public Sprite ItemIcon => _itemSprite;
}
