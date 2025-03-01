
using System.IO;
using UnityEngine;

public class StageDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        BossDataSO bossDataSo = GetSO<BossDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
        
        

        return bossDataSo;
    }
}
