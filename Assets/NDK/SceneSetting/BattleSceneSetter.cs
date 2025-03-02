using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSceneSetter : SceneSetter
{
    [SerializeField] private GameObject _stageManager;
    [SerializeField] private GameObject _playerHud;
    [SerializeField] private GameObject _map;
    protected override void Start()
    {
        base.Start();
        
        Debug.Assert(_stageManager);
        Debug.Assert(_playerHud);
        Debug.Assert(_map);

        GameObject stageManagerObj = Instantiate(_stageManager);
        SpecDataManager specDataManager = SpecDataManager.Instance;
        
        PlayerStatus.Instance.Initialize(specDataManager.GetDataDictionary<PlayerDataSO>());
        if (stageManagerObj.TryGetComponent(out StageManager stageManager))
        {
            var monsterDataSos = specDataManager.GetDataDictionary<MonsterDataSO>();
            var characterDataSos = specDataManager.GetDataDictionary<CharacterDataSO>();
            specDataManager.GetData(301,out StageDataSO stageDataSo);
            var playerDataSos = specDataManager.GetDataDictionary<PlayerDataSO>();
            stageManager.Initialize(monsterDataSos,characterDataSos,stageDataSo,playerDataSos);
        }

        GameObject playerHudObj = Instantiate(_playerHud);
        playerHudObj.SetActive(true);
        if (_playerHud.TryGetComponent(out SkillUI skillUI))
        {
            PlayerStatus.Instance.OnLevelUp -= skillUI.Unlock;
            PlayerStatus.Instance.OnLevelUp += skillUI.Unlock;
            PlayerStatus.Instance.OnHealthChanged += skillUI.OnHealthChanged;
            PlayerStatus.Instance.OnMpChanged += skillUI.OnMPChanged;
            PlayerStatus.Instance.OnExpChanged += skillUI.OnExpChanged;
            
            PlayerStatus.Instance.curruntHp += 0;
            PlayerStatus.Instance.curruntMp += 0;
            PlayerStatus.Instance.curruntExp += 0;
        }

        GameObject mapObj = Instantiate(_map);
        
    }
}
