using System.Collections.Generic;
using UnityEngine;

namespace TestPlayer
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private List<ItemScriptableData> _playerItems;

        public int GoldAmmount { get; private set; }

        private GoldCoinsController goldController;

        // for test we will simulate player interaction with shop\trader\npc
        private void Start()
        {
            Container.Get<TradingController>().InitTradingSystem(_playerItems);

            goldController = Container.Get<GoldCoinsController>();

            goldController.OnGoldAmmountChanged += value => GoldAmmount = value;
        }
        private void OnDestroy()
        {
            goldController.OnGoldAmmountChanged -= value => GoldAmmount = value;
        }
    }
}