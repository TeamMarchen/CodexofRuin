
using System;
using System.IO;
using UnityEngine;

public class StageDataSOMaker : SpecDataSOMaker
{
    public override ScriptableObject ProcessData(string[] data, string fullPath)
    {
        StageDataSO stageDataSO = GetSO<StageDataSO>(GetAssetPath(Path.Combine(fullPath, data[0])));

        stageDataSO.id = int.Parse(data[0]);
        stageDataSO.wave = int.Parse(data[1]);
        _columnDataInput(data[2],out stageDataSO.monsterSpawnRate);
        _columnDataInput(data[3],out stageDataSO.monsterType);
        stageDataSO.bossType = data[4];
        stageDataSO.element = Enum.Parse<Enums.ELEMENT>(data[5]);
        stageDataSO.timeLimit = int.Parse(data[6]);
        stageDataSO.maxMonsterSpawnCount = int.Parse(data[7]);
        stageDataSO.bossClearTime = int.Parse(data[8]);
        
        return stageDataSO;
    }
}
