using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerStatus : Singleton<PlayerStatus>,IStatus
{
    public int level { get; set; }
    public string playerName { get; set; }
    public float hp { get; set; }
    public float curruntHp
    {
        get => _currentHp;
        set
        {
            _currentHp = value;
            OnHealthChanged?.Invoke(_currentHp / (float)maxExp);
        }
    }

    private float _currentHp;
    public float mp { get; set; }
    public float curruntMp { 
        get => _currentMp;
        set
        {
            _currentMp = value;
            OnMpChanged?.Invoke(_currentMp/(float)mp);
        } 
    }
    private float _currentMp;
    public float speed { get; set; }
    public float attackTime { get; set; }
    
    public float attackRange { get; set; }
    public float extraAttackPower { get; set; }
    public float baseDamage { get; set; }
    public float defense { get; set; }
    public int curruntExp
    {
        get => _currentExp;
        set
        {
            _currentExp = value;
            OnExpChanged?.Invoke(_currentExp/(float)maxExp);
        }
    }

    private int _currentExp;
    public int maxExp { get; set; }

    public event Action<int> OnLevelUp;
    public event Action<float> OnExpChanged;
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMpChanged;
    
    private IReadOnlyDictionary<int, PlayerDataSO> levelUpData;

    public void Initialize(IReadOnlyDictionary<int, PlayerDataSO> playerDataSos_)
    {
        level = playerDataSos_[1001].level;
        playerName = playerDataSos_[1001].name;
        hp = playerDataSos_[1001].hp;
        speed = 3f;
        curruntHp = hp;
        baseDamage = playerDataSos_[1001].attack;
        extraAttackPower = 50;
        mp = playerDataSos_[1001].mp;
        curruntMp = mp;
        attackTime = 0.5f;
        curruntExp = 0;
        maxExp = 1000;
        defense = playerDataSos_[1001].defense;
        levelUpData = playerDataSos_;
        Debug.Log("초기화");
    }

    private void Update()
    {
        if (curruntExp >= maxExp)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        
        playerName = levelUpData[1001 + level].name;
        hp = levelUpData[1001+level].hp;
        curruntHp = hp;
        baseDamage = levelUpData[1001 + level].attack;
        mp = levelUpData[1001 + level].mp;
        curruntMp = mp;
        attackTime = 0.5f;
        curruntExp -= maxExp;
        defense = levelUpData[1001 + level].defense;
        level = levelUpData[1001 + level].level;
        maxExp = level * 2 *1000;
        OnLevelUp?.Invoke(level);
        OnExpChanged?.Invoke(curruntExp);
    }
}