
using System;
using System.IO;
using UnityEngine;

public class FederationDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        FederationDataSO federationDataSo = GetSO<FederationDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        federationDataSo.id = int.Parse(data[0]);
        federationDataSo.federationName = data[1];
        _columnDataInput(data[2],out federationDataSo.playerSkillList);
        federationDataSo.element = Enum.Parse<Enums.ELEMENT>(data[3]);
        
        return federationDataSo;
    }
}
