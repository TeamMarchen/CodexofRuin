using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : Singleton<PlayerStatus>,IStatus
{
    public int level { get; set; }
    public string playerName { get; set; }
    public float hp { get; set; }
    public float curruntHp { get; set; }
    public float mp { get; set; }
    public float curruntMp { get; set; }
    public float speed { get; set; }
    public float attackTime { get; set; }
    public float attackRange { get; set; }
    public float baseDamage { get; set; }
    public int curruntExp { get; set; }
    public int maxExp { get; set; }

    private IReadOnlyDictionary<int, PlayerDataSO> levelUpData;

    private void Initialize(IReadOnlyDictionary<int, PlayerDataSO> playerDataSos_)
    {
        level = playerDataSos_[1001].level;
        playerName = playerDataSos_[1001].name;
        hp = playerDataSos_[1001].hp;
        curruntHp = hp;
        baseDamage = playerDataSos_[1001].attack;
        mp = playerDataSos_[1001].mp;
        curruntMp = mp;
        attackTime = 0.5f;
        curruntExp = 0;
        maxExp = 1000;
        levelUpData = playerDataSos_;
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
        curruntExp = 0;
        maxExp = 1000;
        level = levelUpData[1001 + level].level;
    }
}