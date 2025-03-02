using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : IMonster
{
    public int level { get; set; }
    public string name { get; set; }
    public float curruntHp { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float attackTime { get; set; }
    public float attackRange { get; set; }
    public float baseDamage { get; set; }
    public float defense { get; set; }
    public List<string> dropItemID { get; set; } = new List<string>();
    public int exp { get; set; }

    public MonsterData(int level,string name, float hp, float speed, float attackTime, float attackRange, float baseDamage, List<string> dropItemID, int exp, float defense)
    {
        this.level = level;
        this.name = name;
        this.hp = hp;
        curruntHp = hp;
        this.speed = speed;
        this.attackTime = attackTime;
        this.attackRange = attackRange;
        this.baseDamage = baseDamage;
        this.dropItemID = dropItemID;
        this.exp = exp;
        this.defense = defense;
    }
}
