using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneSetter : SceneSetter
{
    [SerializeField] private GameObject _stageManager;
    [SerializeField] private GameObject _PlayerHud;

    protected override void Start()
    {
        base.Start();

        GameObject stageManagerObj = Instantiate(_stageManager);
        SpecDataManager specDataManager = SpecDataManager.Instance;
        if (stageManagerObj.TryGetComponent(out StageManager stageManager))
        {
            var monsterDataSos = specDataManager.GetDataDictionary<MonsterDataSO>();
            var characterDataSos = specDataManager.GetDataDictionary<CharacterDataSO>();
            specDataManager.GetData(301,out StageDataSO stageDataSo);
            var playerDataSos = specDataManager.GetDataDictionary<PlayerDataSO>();
            stageManager.Initialize(monsterDataSos,characterDataSos,stageDataSo,playerDataSos);
        }

        GameObject playerHudObj = Instantiate(_PlayerHud);
        playerHudObj.SetActive(true);
    }
}
