
using System.IO;
using UnityEngine;

public class PlayerDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        PlayerDataSO playerDataSo = GetSO<PlayerDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        playerDataSo.id = int.Parse(data[0]);
        playerDataSo.playerName = data[1];
        playerDataSo.level = int.Parse(data[2]);
        playerDataSo.hp = int.Parse(data[3]);
        playerDataSo.attack = int.Parse(data[4]);
        playerDataSo.defense = int.Parse(data[5]);
        playerDataSo.mp = int.Parse(data[6]);

        return playerDataSo;
    }
}
