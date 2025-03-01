using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : IMonster
{
    public int level { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float attackTime { get; set; }
    public float attackRange { get; set; }
    public float baseDamage { get; set; }
    public List<string> dropItemID { get; set; } = new List<string>();
    public int exp { get; set; }

    public MonsterData(int level, float hp, float speed, float attackTime, float attackRange, float baseDamage, List<string> dropItemID, int exp)
    {
        this.level = level;
        this.hp = hp;
        this.speed = speed;
        this.attackTime = attackTime;
        this.attackRange = attackRange;
        this.baseDamage = baseDamage;
        this.dropItemID = dropItemID;
        this.exp = exp;
    }
}
