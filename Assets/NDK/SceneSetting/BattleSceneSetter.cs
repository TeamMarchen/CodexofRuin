using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneSetter : SceneSetter
{
    [SerializeField] private GameObject _stageManager;
    [SerializeField] private GameObject _playerHud;
    [SerializeField] private GameObject _map;

    private SkillUI SkillUI;
    protected override void Start()
    {
        base.Start();
        
        Debug.Assert(_stageManager);
        Debug.Assert(_playerHud);
        Debug.Assert(_map);

        GameObject stageManagerObj = Instantiate(_stageManager);
        SpecDataManager specDataManager = SpecDataManager.Instance;
        PlayerStatus.Instance.Initialize(specDataManager.GetDataDictionary<PlayerDataSO>());
        
        
        GameObject playerHudObj = Instantiate(_playerHud);
        if (playerHudObj.TryGetComponent(out SkillUI skillUI))
        {
            skillUI.Initialize();
            
            PlayerStatus.Instance.OnLevelUp += skillUI.Unlock;
            //PlayerStatus.Instance.OnHealthChanged += skillUI.OnHealthChanged;
            PlayerStatus.Instance.OnMpChanged += skillUI.OnMPChanged;
            PlayerStatus.Instance.OnExpChanged += skillUI.OnExpChanged;
            
            PlayerStatus.Instance.curruntMp += 0;
            PlayerStatus.Instance.curruntExp += 0;
        }
        
       
        if (stageManagerObj.TryGetComponent(out StageManager stageManager))
        {
            var monsterDataSos = specDataManager.GetDataDictionary<MonsterDataSO>();
            var characterDataSos = specDataManager.GetDataDictionary<CharacterDataSO>();
            specDataManager.GetData(301,out StageDataSO stageDataSo);
            var playerDataSos = specDataManager.GetDataDictionary<PlayerDataSO>();
            stageManager.Initialize(monsterDataSos,characterDataSos,stageDataSo,playerDataSos,
                skillUI._skill1CoolTime,skillUI._skill2CoolTime,skillUI._skill3CoolTime,skillUI._healthGauge
            );
        }

        GameObject mapObj = Instantiate(_map);
        
    }

    private void OnDisable()
    {
        PlayerStatus.Instance.OnLevelUp -= SkillUI.Unlock;
        PlayerStatus.Instance.OnMpChanged -= SkillUI.OnMPChanged;
        PlayerStatus.Instance.OnExpChanged -= SkillUI.OnExpChanged;
    }
}
