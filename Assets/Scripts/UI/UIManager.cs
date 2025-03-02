using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    public event Action<int> OnGoldChanged;
    public event Action<int> OnCashChanged;

    private int gold;
    private int cash;
    public string userName;
    private bool isInitialized = false;
    
    public void Init()
    {
        if (!isInitialized)
        {
            Gold = 1000;
            Cash = 500;
            userName = "Codex of Ruin";
            isInitialized = true;
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

    //골드 및 캐쉬 추가 및 감소 코드
    /*public void AddGold(int amount)
    {
        gold += amount;
        OnGoldChanged?.Invoke(gold); // UI 업데이트
    }

    public void SubtractGold(int amount)
    {
        if (gold >= amount) // 잔액 체크
        {
            gold -= amount;
            OnGoldChanged?.Invoke(gold);
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
        }
    }

    //  캐시 변경 메서드 추가
    public void AddCash(int amount)
    {
        cash += amount;
        OnCashChanged?.Invoke(cash);
    }

    public void SubtractCash(int amount)
    {
        if (cash >= amount)
        {
            cash -= amount;
            OnCashChanged?.Invoke(cash);
        }
        else
        {
            Debug.Log("캐시가 부족합니다!");
        }
    }*/
}
