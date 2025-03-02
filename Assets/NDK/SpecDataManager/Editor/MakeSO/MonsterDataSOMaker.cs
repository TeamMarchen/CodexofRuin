
using System;
using System.IO;
using UnityEngine;

public class MonsterDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        MonsterDataSO monsterDataSo = GetSO<MonsterDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        monsterDataSo.id = int.Parse(data[0]);
        monsterDataSo.monsterName = data[1];
        monsterDataSo.element = Enum.Parse<Enums.ELEMENT>(data[2]);
        monsterDataSo.maxHp = int.Parse(data[3]);
        monsterDataSo.attack = int.Parse(data[4]);
        monsterDataSo.defense = int.Parse(data[5]);
        monsterDataSo.maxMP = int.Parse(data[6]);

        return monsterDataSo;
    }
}
