using UnityEngine;
using TMPro;

public class GoldUiStatsTrading : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _goldAmmountValueTMP;

    private GoldCoinsController goldController;

    private void Start()
    {
        goldController = Container.Get<GoldCoinsController>();

        goldController.OnGoldAmmountChanged += OnHandleAmmountChanges;

        _goldAmmountValueTMP.text = goldController.GoldAmmount.ToString();
    }
    private void OnHandleAmmountChanges(int newAmmount)
    {
        _goldAmmountValueTMP.text = newAmmount.ToString();
    }
    private void OnDestroy()
    {
        goldController.OnGoldAmmountChanged -= OnHandleAmmountChanges;
    }
}
