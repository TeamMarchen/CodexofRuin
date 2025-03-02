
using System;
using System.IO;
using UnityEngine;

public class PlayerSkillSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        PlayerSkillSO playerSkillSo = GetSO<PlayerSkillSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        playerSkillSo.id = int.Parse(data[0]);
        playerSkillSo.skillDescription = data[1];
        playerSkillSo.element = Enum.Parse<Enums.ELEMENT>(data[2]);
        playerSkillSo.playerRequireLevel = int.Parse(data[3]);
        playerSkillSo.skillLevel = int.Parse(data[4]);
        playerSkillSo.attack = int.Parse(data[5]);
        playerSkillSo.cooldown = int.Parse(data[6]);

        return playerSkillSo;
    }
}
