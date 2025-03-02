using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuModel
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnCashChanged;
    public event Action<string> OnUserNameChanged;

    private int gold;
    private int cash;
    private string userName;
    public string UserName
    {
        get => userName;
        set
        {
            userName = value;
            OnUserNameChanged?.Invoke(userName);
        }
    }
    public int Gold
    {
        get => gold;
        set
        {
            gold = value;
            OnGoldChanged?.Invoke(gold);
        }
    }

    public int Cash
    {
        get => cash;
        set
        {
            cash = value;
            OnCashChanged?.Invoke(cash);
        }
    }
}
