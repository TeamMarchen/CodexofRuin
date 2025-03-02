using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

public class SaveNLoadManager : Singleton<SaveNLoadManager>
{
    [SerializeField] private string mainDataFile = "data.sav";
    [SerializeField] private bool isEncrypt = true;
    private HashSet<SaveUnit> saveUnits;
    

    private StringBuilder sb;
    
    char nameSpliter = '|';
    char dataSpliter = '\n';

   
    protected override void Awake()
    {
        base.Awake();
        saveUnits = new HashSet<SaveUnit>();
        sb = new StringBuilder();
    }

    public void Clear()
    {
        if(saveUnits != null)
            saveUnits.Clear();
    }

    public void RegistSaveUnit(SaveUnit unit)
    {
        if (saveUnits == null) saveUnits = new HashSet<SaveUnit>();
        saveUnits.Add(unit);
    }

    public void RemoveSaveUnit(SaveUnit unit)
    {
        saveUnits.Remove(unit);
    }
    

    #region Save

    public async void Save()
    {
        if (saveUnits == null||saveUnits.Count <= 0) return;
        foreach (var i1 in saveUnits)
        {
            AddSaveDataToSB(i1 );
        }
        
        await Saving(sb.ToString(),mainDataFile);
        sb.Clear();
    }
    
    public async Task Saving(string saveData, string fileName)
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string tmpPath = filePath + ".tmp";
        using (StreamWriter sw = File.CreateText(tmpPath))
        {
            string data = saveData;
            if (isEncrypt)
            {
                data = EncryptManager.EncryptData(saveData);
            }

            string versionAddedData = $"{Const.String.Version.versionInfo}?{data}";
            string hashString = versionAddedData.GetHashCode().ToString();
            data = $"{versionAddedData}?{hashString}";
            await sw.WriteAsync(data);
        }

        if (File.Exists(filePath))
            File.Delete(filePath);
        File.Move(tmpPath, filePath);

#if UNITY_EDITOR
        Debug.Log($"{fileName} SaveComplete");
#endif
    }

    private void AddSaveDataToSB(SaveUnit unit)
    {
        string uniqueName;
        object data = unit.GetData(out uniqueName);
        sb.Append(uniqueName).Append(nameSpliter).Append(JsonUtility.ToJson(data)).Append(dataSpliter);
    }

   
    
    #endregion
    
    #region Load

    public void LoadMain()
    {
        LoadSaveUnits(mainDataFile);
    }
    public void LoadSaveUnits(string fileName)
    {
        if (saveUnits == null) saveUnits = new HashSet<SaveUnit>();
        string totalData = LoadJsonData(fileName);
        if (totalData == null)
            return;
        string[] dataLine = totalData.Split(dataSpliter);
        for (int i = 0; i < dataLine.Length; i++)
        {
            if(dataLine[i] == string.Empty) continue;
            string[] dataSplit = dataLine[i].Split(nameSpliter);
            SetSaveData(dataSplit);
        }
        
    }

    private void SetSaveData(string[] data)
    {
        Type type = Type.GetType(data[0]);

        SaveUnit saveUnit = JsonUtility.FromJson(data[1], type) as SaveUnit;
        
        saveUnits.Add(saveUnit);
    }


    public string LoadJsonData(string fileName)
    {
        string data = string.Empty;
        string dataPath = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(dataPath)) return String.Empty;
        using (StreamReader sr = new StreamReader(dataPath))
        {
            string encrypdata = sr.ReadToEnd();
            // 추후에 버전 체크를 할 수 있게 변형할 수 있음.
            if (encrypdata.StartsWith(Const.String.Version.versionInfo))
            {
                string[] strIntegrityCheckSplit = encrypdata.Split("?");
                if (strIntegrityCheckSplit.Length != 3)
                {
                    Debug.LogWarning("데이터의 이상");
                }
                else
                {
                    string hashString = $"{strIntegrityCheckSplit[0]}?{strIntegrityCheckSplit[1]}";

                    if (int.TryParse(strIntegrityCheckSplit[2], out int hash) && hash == hashString.GetHashCode())
                    {
                        if(isEncrypt)
                            data = EncryptManager.DecryptData(strIntegrityCheckSplit[1]);
                        else
                        {
                            data = strIntegrityCheckSplit[1];
                        }
                        
                    }
                    else
                    {
                        Debug.LogWarning("데이터의 이상");
                    }
                }
            }
            else
            {
                Debug.LogWarning("데이터의 이상");
            }
        }
        
#if UNITY_EDITOR
        Debug.Log($"{fileName} Load Complete");
#endif

        return data;
    }

    #endregion

    #region InputOutput

    public SaveUnit GetSaveData(string className)
    {
        Type type = Type.GetType(className);

        foreach (var saveUnit in saveUnits)
        {
            if (saveUnit.GetType() == type)
                return saveUnit;
        }

        return null;
    }


    #endregion
    

    public void SaveData(string key, string value)
    {
        PlayerPrefs.SetString(key, value);
    }

    public string LoadData(string key)
    {
        if (PlayerPrefs.HasKey(key)) return PlayerPrefs.GetString(key);
        else return null;
    }

    public void DeleteSavedFile()
    {
        string[] files = Directory.GetFiles(Application.persistentDataPath);
        foreach (var filename in files)
        {
            if (filename.EndsWith(".sav"))
            {
                Debug.LogWarning(filename + "is Deleted");
                File.Delete(filename);    
            }
        }
    }

    public void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            Save();
    }

    public void OnApplicationQuit()
    {
        Save();
    }
}

