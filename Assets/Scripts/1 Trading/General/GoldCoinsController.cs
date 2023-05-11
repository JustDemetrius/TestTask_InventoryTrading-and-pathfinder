using UnityEngine;
using System;

public class GoldCoinsController : BaseController
{
    [SerializeField] private uint _goldAmmountForTest;

    public event Action<int> OnGoldAmmountChanged;
    public int GoldAmmount { get; private set; }

    public override void Awake()
    {
        base.Awake();
        GoldAmmount = (int)_goldAmmountForTest;
    }
    public void ChangeGoldAmmount(int ammountToChange)
    {
        int changedGoldAmmount = GoldAmmount + ammountToChange;

        if (changedGoldAmmount >= 0)
        {
            GoldAmmount = changedGoldAmmount;
            OnGoldAmmountChanged?.Invoke(GoldAmmount);
        }
        else
            Debug.Log($"|GOLD CONTROLLER| : Gold ammount can't be negative!");
    }
    protected override void AddToContainer()
    {
        Container.Add(this);
    }

    protected override void RemoveFromContainer()
    {
        Container.Remove<GoldCoinsController>();
    }
}
