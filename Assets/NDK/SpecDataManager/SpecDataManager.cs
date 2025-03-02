using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecDataManager : Singleton<SpecDataManager>
{
    private List<Dictionary<int, SpecDataSO>> _specDatas;
    public bool isLoaded { private set; get; } = false;
    
    protected override void Awake()
    {
        base.Awake();
        Init();
    }

    public void Init()
    {
        if (isLoaded) return;
        isLoaded = true;
        InitData();
    }

    private void InitData()
    {
        int length = (int)Enums.SPEC_DATA_TYPE.EMax;
        _specDatas = new List<Dictionary<int, SpecDataSO>>(length);
        for (int i = 0; i <length; i++)
        {
            _specDatas.Add(new Dictionary<int, SpecDataSO>());
        }
        
        //NONE = 0, STAGE_DATA, MONSTER_DATA, CHARACTER_DATA, PLAYER_SKILL_DATA, FEDERATION_SKILL_DATA, FEDERATION_DATA, PLAYER_DATA, ITEM_DATA,
        SpecDataSO[] stageDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.STAGE_DATA_PATH);
        SpecDataSO[] monsterDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.MONSTER_DATA_PATH);
        SpecDataSO[] characterDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.CHARACTER_DATA_PATH);
        SpecDataSO[] playerSkillDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.PLAYER_SKILL_DATA_PATH);
        SpecDataSO[] federationSkillDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.FEDERATION_SKILL_DATA_PATH);
        SpecDataSO[] federationDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.FEDERATION_DATA_PATH);
        SpecDataSO[] playerDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.PLAYER_DATA_PATH);
        SpecDataSO[] itemDataSO = Resources.LoadAll<SpecDataSO>(Const.String.Path.ITEM_DATA_PATH);
        
        AddDataToDic(Enums.SPEC_DATA_TYPE.STAGE_DATA,stageDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.MONSTER_DATA,monsterDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.CHARACTER_DATA,characterDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.PLAYER_SKILL_DATA,playerSkillDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.FEDERATION_SKILL_DATA,federationSkillDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.FEDERATION_DATA,federationDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.PLAYER_DATA,playerDataSO);
        AddDataToDic(Enums.SPEC_DATA_TYPE.ITEM_DATA,itemDataSO);
    }

    private void AddDataToDic(Enums.SPEC_DATA_TYPE type, SpecDataSO[] datas)
    {
        Dictionary<int, SpecDataSO> dicData = _specDatas[(int)type];
        for (int i = 0; i < datas.Length; i++)
        {
            if (!dicData.TryAdd(datas[i].id, datas[i]))
            {
                Debug.Log($"{datas[i].id}가 겹치는 친구가 있습니다. 확인해주세요");
            }
        }
    }

    public bool GetData<T>(int id,out T data) where T : ScriptableObject
    {
        if (!isLoaded)
            Init();
        
        Type t = typeof(T);
        
        Enums.SPEC_DATA_TYPE specDataType = GetSpecDataType(t);
        bool isExist = _specDatas[(int)specDataType].TryGetValue(id,out SpecDataSO value);
        data = value as T; 
        
        return isExist;
    }

    public IReadOnlyDictionary<int,SpecDataSO> GetDataList<T>() where T : SpecDataSO
    {
        if(!isLoaded)
            Init();
        Type t = typeof(T);
        Enums.SPEC_DATA_TYPE specDataType = GetSpecDataType(t);
        return _specDatas[(int)specDataType];
    }

    private Enums.SPEC_DATA_TYPE GetSpecDataType(Type t)
    {
        //NONE = 0, STAGE_DATA, MONSTER_DATA, CHARACTER_DATA, PLAYER_SKILL_DATA, FEDERATION_SKILL_DATA, FEDERATION_DATA, PLAYER_DATA, ITEM_DATA,
        if (t == typeof(StageDataSO))
        {
            return Enums.SPEC_DATA_TYPE.STAGE_DATA;
        }else if (t == typeof(MonsterDataSO))
        {
            return Enums.SPEC_DATA_TYPE.MONSTER_DATA;
        }else if (t == typeof(CharacterDataSO))
        {
            return Enums.SPEC_DATA_TYPE.CHARACTER_DATA;
        }else if (t == typeof(PlayerSkillSO))
        {
            return Enums.SPEC_DATA_TYPE.PLAYER_SKILL_DATA;
        }else if (t == typeof(FederationSkillSO))
        {
            return Enums.SPEC_DATA_TYPE.FEDERATION_SKILL_DATA;   
        }else if (t == typeof(FederationDataSO))
        {
            return Enums.SPEC_DATA_TYPE.FEDERATION_DATA;
        }else if (t == typeof(PlayerDataSO))
        {
            return Enums.SPEC_DATA_TYPE.PLAYER_DATA;
        }else if (t == typeof(ItemDataSO))
        {
            return Enums.SPEC_DATA_TYPE.ITEM_DATA;
        }
        // if (t == typeof(CharacterData))
        // {
        //     return Enums.SPEC_DATA_TYPE.CharacterSpecData;
        // }else if (t == typeof(IngameLevelDataSO))
        // {
        //     return Enums.SPEC_DATA_TYPE.IngameLevelData;
        // }else if (t == typeof(OutGameGradeDataSO))
        // {
        //     return Enums.SPEC_DATA_TYPE.OutGameGradeData;
        // }else if (t == typeof(StageInfoSO))
        // {
        //     return Enums.SPEC_DATA_TYPE.StageInfo;
        // }else if (t == typeof(AchievementInfoSO))
        // {
        //     return Enums.SPEC_DATA_TYPE.AchievementInfo;
        // }else if (t == typeof(EquipmentLevelData))
        // {
        //     return Enums.SPEC_DATA_TYPE.EquipmentLevelData;
        // }else if (t == typeof(Skill))
        // {
        //     return Enums.SPEC_DATA_TYPE.Skill;
        // }
        // else if (t == typeof(UpgradeSpecDataSO))
        // {
        //     return Enums.SPEC_DATA_TYPE.UpgradeSpecDataSO;
        // }
        return Enums.SPEC_DATA_TYPE.NONE;
    }
}
