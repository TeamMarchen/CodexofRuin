
using System.IO;
using UnityEngine;

public class PlayerSkillSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        PlayerSkillSO bossDataSo = GetSO<PlayerSkillSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
        

        return bossDataSo;
    }
}
