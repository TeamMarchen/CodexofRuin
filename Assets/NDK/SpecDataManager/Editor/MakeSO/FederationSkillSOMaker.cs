
using System.IO;
using UnityEngine;

public class FederationSkillSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        FederationSkillSO federationSkillSo = GetSO<FederationSkillSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        federationSkillSo.id = int.Parse(data[0]);
        federationSkillSo.factionID = int.Parse(data[1]);
        federationSkillSo.attack = int.Parse(data[2]);
        federationSkillSo.defense = int.Parse(data[3]);
        federationSkillSo.description = data[4];

        return federationSkillSo;
    }
}
