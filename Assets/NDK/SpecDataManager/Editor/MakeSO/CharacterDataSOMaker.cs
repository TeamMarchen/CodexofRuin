
using System.IO;
using UnityEngine;

public class CharacterDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        CharacterDataSO characterDataSo = GetSO<CharacterDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        characterDataSo.id = int.Parse(data[0]);
        characterDataSo.characterName = data[1];
        characterDataSo.faction = data[2];
        characterDataSo.role = data[3];
        characterDataSo.level = int.Parse(data[4]);
        characterDataSo.maxHP = int.Parse(data[5]);
        characterDataSo.attack = int.Parse(data[6]);
        characterDataSo.defense = int.Parse(data[7]);
        characterDataSo.maxMP = int.Parse(data[8]);
        characterDataSo.imagePath = data[9];
        
        return characterDataSo;
    }
}
