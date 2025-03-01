
using System.IO;
using UnityEngine;

public class FederationDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        FederationDataSO bossDataSo = GetSO<FederationDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));
        

        return bossDataSo;
    }
}
