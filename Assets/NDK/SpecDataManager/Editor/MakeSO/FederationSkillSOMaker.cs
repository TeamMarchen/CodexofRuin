
using System.IO;
using UnityEngine;

public class FederationSkillSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        FederationSkillSO bossDataSo = GetSO<FederationSkillSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
        
        

        return bossDataSo;
    }
}
