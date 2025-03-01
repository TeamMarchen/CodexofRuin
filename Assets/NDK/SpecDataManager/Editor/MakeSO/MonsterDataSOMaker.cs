
using System.IO;
using UnityEngine;

public class MonsterDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        MonsterDataSO bossDataSo = GetSO<MonsterDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
        
        

        return bossDataSo;
    }
}
