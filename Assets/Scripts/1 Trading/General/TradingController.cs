using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Canvas))]
public class TradingController : BaseController
{
    [Header("Items to show in shop FOR TEST")]
    [SerializeField] private List<ItemScriptableData> _shopItems; // for test
    [Space(20)]
    [Header("Refs on inventory modules in canvas")]
    [SerializeField] private InventoryHolder _playerInventory;
    [SerializeField] private InventoryHolder _shopInventory;
    [Space]
    [Header("ItemPrefab")]
    [SerializeField] private ItemObject _itemPrefab;

    public Canvas TradingCanvas { get; private set; }

    private List<ItemScriptableData> playerItems;
    private GoldCoinsController goldController;

    public override void Awake()
    {
        base.Awake();
        TradingCanvas = GetComponent<Canvas>();
    }
    public void InitTradingSystem(List<ItemScriptableData> playerItems)
    {
        this.playerItems = playerItems;
        _playerInventory.InitInventoryHolder(this.playerItems, _itemPrefab);
        _shopInventory.InitInventoryHolder(_shopItems, _itemPrefab, 1.5f);

        goldController = Container.Get<GoldCoinsController>();

        _playerInventory.OnTryingToTradeItem += TryToBuyItem;
        _shopInventory.OnTryingToTradeItem += TryToSellItem;
    }
    private void TryToSellItem(ItemObject item)
    {
        if (TryToTradeItem(item, TradingType.Sell))
        {
            _shopInventory.HandleItem(item);
            _playerInventory.UnHandleItem(item);

            var data = playerItems.Find(some => some.Equals(item.ItemData));
            playerItems.Remove(data);
            _shopItems.Add(data);
        } 
    }
    private void TryToBuyItem(ItemObject item)
    {
        if (TryToTradeItem(item, TradingType.Buy))
        {
            _playerInventory.HandleItem(item);
            _shopInventory.UnHandleItem(item);

            var data = _shopItems.Find(some => some.Equals(item.ItemData));
            _shopItems.Remove(data);
            playerItems.Add(data);
        }  
    }
    private bool TryToTradeItem(ItemObject item, TradingType tradeType)
    {
        bool isSucessfullyTradet = false; // as default status
        
        if (goldController == null)
            return isSucessfullyTradet = false;

        switch (tradeType)
        {
            case TradingType.Buy:
                isSucessfullyTradet = goldController.GoldAmmount - item.ItemPrice >= 0;
                if (isSucessfullyTradet)
                    goldController.ChangeGoldAmmount(item.ItemPrice * -1);
                break;
            case TradingType.Sell:
                isSucessfullyTradet = true;
                goldController.ChangeGoldAmmount(item.ItemPrice);
                break;
        }

        return isSucessfullyTradet;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
        _playerInventory.OnTryingToTradeItem -= TryToBuyItem;
        _shopInventory.OnTryingToTradeItem -= TryToSellItem;
    }
    protected override void AddToContainer()
    {
        Container.Add(this);
    }

    protected override void RemoveFromContainer()
    {
        Container.Remove<TradingController>();
    }
}
public enum TradingType
{
    Buy = 0,
    Sell = 1,
}
