using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryHolder : MonoBehaviour, IDropHandler
{
    public event Action<ItemObject> OnTryingToTradeItem;

    private RectTransform holderContent;
    private List<ItemObject> inventotyItems;
    private float priceMultiplierValue;
    private ItemObject itemPrefab;

    public void InitInventoryHolder(List<ItemScriptableData> itemsData, ItemObject prefab, float priceMultiplier = 0f)
    {
        holderContent = GetComponent<ScrollRect>().content;
        priceMultiplierValue = priceMultiplier;
        itemPrefab = prefab;

        if (inventotyItems != null && inventotyItems.Count > 0)
            ClearPreviousItems();

        inventotyItems = new List<ItemObject>();
        var workingCanvas = Container.Get<TradingController>().TradingCanvas;
        Vector2 cellSize = holderContent.GetComponent<GridLayoutGroup>().cellSize;

        for (int i = 0; i < itemsData.Count; i++)
        {
            // creating cell to holding item in grid
            var objectForSpot = new GameObject();
            var spot = objectForSpot.AddComponent<RectTransform>();
            spot.SetParent(holderContent);

            // creating item object and attaching to cell
            var itemObj = Instantiate(itemPrefab, spot.transform);
            itemObj.GetComponent<RectTransform>().sizeDelta = cellSize;
            itemObj.InitItem(itemsData[i], priceMultiplier);

            // getting draging module on item and initialising it
            var dragDropModule = itemObj.GetComponent<DragDropModule>();
            dragDropModule.InitDragingModule(workingCanvas);

            inventotyItems.Add(itemObj);
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && eventData.pointerDrag.TryGetComponent(out ItemObject item))
        {
            if (!inventotyItems.Contains(item))
                OnTryingToTradeItem?.Invoke(item);
        }
            
    }
    public void HandleItem(ItemObject itemToHandle)
    {
        // we are taking of dragable itemp spot and changin parent

        itemToHandle.transform.parent.SetParent(holderContent);
        itemToHandle.UpdateItemCost(priceMultiplierValue);
        inventotyItems.Add(itemToHandle);
    }
    public void UnHandleItem(ItemObject itemToUnHandle)
    {
        var itemToUnhandle = inventotyItems.Find(some => some.Equals(itemToUnHandle));
        inventotyItems.Remove(itemToUnhandle);
    }
    private void ClearPreviousItems()
    {
        // for preventing phantom items

        foreach (Transform slot in holderContent)
        {
            Destroy(slot.gameObject);
        }
    }
}
