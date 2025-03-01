using System;
using System.Collections.Generic;
using UnityEngine;

public class SpecDataManager : MonoBehaviour
{
    private List<Dictionary<string, SpecDataSO>> _specDatas;
    public bool isLoaded { private set; get; } = false;
    
    protected void Awake()
    {
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
        _specDatas = new List<Dictionary<string, SpecDataSO>>(length);
        for (int i = 0; i <length; i++)
        {
            _specDatas.Add(new Dictionary<string, SpecDataSO>());
        }
        
        // SpecDataSO[] CharacterDataSos = Resources.LoadAll<SpecDataSO>(Path.CharacterDataSOInResources);
        // SpecDataSO[] IngameLevelDataSos = Resources.LoadAll<SpecDataSO>(Path.IngameLevelSOInResources);
        // SpecDataSO[] outGameLevelDataSos = Resources.LoadAll<SpecDataSO>(Path.OutGameLevelDataSOInResources);
        // SpecDataSO[] stageInfoSos = Resources.LoadAll<SpecDataSO>(Path.StageInfoSOInResources);
        // SpecDataSO[] AchievementInfoSos = Resources.LoadAll<SpecDataSO>(Path.AchievementInfo);
        // SpecDataSO[] EquipmentLevelDataSos = Resources.LoadAll<SpecDataSO>(Path.EquipmentLevelDataResources);
        // SpecDataSO[] Skill = Resources.LoadAll<SpecDataSO>(Path.Skill);
        // SpecDataSO[] upgradeSpecDataSos = Resources.LoadAll<SpecDataSO>(Path.UpgradeSpecData);
        //
        // AddDataToDic(Enums.SpecDataType.CharacterSpecData,CharacterDataSos);
        // AddDataToDic(Enums.SpecDataType.IngameLevelData,IngameLevelDataSos);
        // AddDataToDic(Enums.SpecDataType.OutGameGradeData,outGameLevelDataSos);
        // AddDataToDic(Enums.SpecDataType.StageInfo,stageInfoSos);
        // AddDataToDic(Enums.SpecDataType.AchievementInfo,AchievementInfoSos);
        // AddDataToDic(Enums.SpecDataType.EquipmentLevelData,EquipmentLevelDataSos);
        // AddDataToDic(Enums.SpecDataType.Skill,Skill);
        // AddDataToDic(Enums.SpecDataType.UpgradeSpecDataSO,upgradeSpecDataSos);
    }

    private void AddDataToDic(Enums.SPEC_DATA_TYPE type, SpecDataSO[] datas)
    {
        Dictionary<string, SpecDataSO> dicData = _specDatas[(int)type];
        for (int i = 0; i < datas.Length; i++)
        {
            if (!dicData.TryAdd(datas[i].id, datas[i]))
            {
                Debug.Log($"{datas[i].id}가 겹치는 친구가 있습니다. 확인해주세요");
            }
        }
    }

    public bool GetData<T>(string id,out T data) where T : ScriptableObject
    {
        if (!isLoaded)
            Init();
        
        Type t = typeof(T);
        
        Enums.SPEC_DATA_TYPE specDataType = GetSpecDataType(t);
        bool isExist = _specDatas[(int)specDataType].TryGetValue(id,out SpecDataSO value);
        data = value as T; 
        
        return isExist;
    }

    public IReadOnlyDictionary<string,SpecDataSO> GetDataList<T>() where T : SpecDataSO
    {
        if(!isLoaded)
            Init();
        Type t = typeof(T);
        Enums.SPEC_DATA_TYPE specDataType = GetSpecDataType(t);
        return _specDatas[(int)specDataType];
    }

    private Enums.SPEC_DATA_TYPE GetSpecDataType(Type t)
    {
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
