using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public class GoogleSheetToSO : EditorWindow
{
    [MenuItem("DONG/GoogleSheetToSO")]
    
    public static void ShowWindow()
    {
        // 윈도우를 띄우는 코드
        GetWindow<GoogleSheetToSO>("My Custom Editor Window");
    }
    private SpecDataPathSO _specDataPathSo;
    private List<SpecDataSOMaker> _makers;
    private void OnEnable()
    {
        // 스펙 데이터 추가과정
        // SO 생성 -> SOMaker생성 -> SpecDataType에 타입 추가
        // _makers에 클래스 생성 
        // SpecDataManager에서 Load과정 추가
        // 초기화
        _makers = new List<SpecDataSOMaker>((int)Enums.SPEC_DATA_TYPE.EMax);
        _makers.Add(null);
        _makers.Add(new BossDataSOMaker());
        _makers.Add(new CharacterDataSOMaker());
        _makers.Add(new FederationDataSOMaker());
        _makers.Add(new FederationSkillSOMaker());
        _makers.Add(new MonsterDataSOMaker());
        _makers.Add(new PlayerSkillSOMaker());
        _makers.Add(new StageDataSOMaker());
        // _makers.Add(new CharacterDataSOMaker());
        // _makers.Add(new IngameLevelDataSOMaker());
        // _makers.Add(new OutGameGradeDataSOMaker());
        // _makers.Add(new StageInfoSOMaker());
        // _makers.Add(new AchievementInfoSOMaker());
        // _makers.Add(new EquipmentDataSOMaker());
        // _makers.Add(new SkillSOMaker());
        // _makers.Add(new UpgradeSpecDataSOMaker());
        // _makers.Add(new SkinDataSOMaker());

    }

    private async void OnGUI()
    {
        // ScriptableObject 선택
        
        _specDataPathSo = (SpecDataPathSO)EditorGUILayout.ObjectField("ScriptableObject", _specDataPathSo, typeof(SpecDataPathSO), false);
        if (GUILayout.Button("SO 생성"))
        {
            if (_specDataPathSo != null)
            {
                int count = _specDataPathSo.specDataPaths.SpecDataTypes.Count;
                for (int i = 0; i < count ; ++i)
                {
                    Task<string> csvText = GoogleSheetCSVReader.DownloadCSV(_specDataPathSo.specDataPaths.urlPath[i]);
                    await csvText;
                    if(csvText.IsCompletedSuccessfully)
                        ProcessData(_specDataPathSo.specDataPaths.SpecDataTypes[i],csvText.Result,_specDataPathSo.specDataPaths.savePath[i]);
                    Debug.Log($"{_specDataPathSo.specDataPaths.SpecDataTypes[i].ToString()} 로드 완료");
                }
            }
            Debug.Log("데이터 로드 완료"); 
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorUtility.FocusProjectWindow();
            
        }
        
    }
    
    public void ProcessData(Enums.SPEC_DATA_TYPE type, string csv,string folderNamePath)
    {
        string[] line = csv.Split('\n');
        
        string fullPath = Path.Combine(Const.String.Path.SO, folderNamePath);
        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
            Debug.Log($"Make {fullPath}");
        }
            
        for (int i = 3; i < line.Length; ++i)
        {
            ScriptableObject processedData = _makers[(int)type].ProcessData(line[i].Split(','),fullPath);
            EditorUtility.SetDirty(processedData);
        }
    }
}
