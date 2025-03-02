using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public abstract class SpecDataSOMaker
{
    public abstract ScriptableObject ProcessData(string[] data, string fullPath);

    protected T GetSO<T>(string fullPathWithName, bool DoCreate=true) where T : ScriptableObject
    {
        T so = null;
        if (File.Exists(fullPathWithName))
        {
            so = AssetDatabase.LoadAssetAtPath<T>(fullPathWithName);
        }
        else if(DoCreate)
        {
            so = ScriptableObject.CreateInstance<T>();
            Debug.Log(fullPathWithName);
            AssetDatabase.CreateAsset(so,fullPathWithName);
        }

        return so;
    }

    protected bool GetAsset<T>(string fullPathWithName, out T asset, string owner) where T : Object
    {
        asset = null;
        Type t = typeof(T);
        if (!string.IsNullOrEmpty(fullPathWithName))
        {
            asset = AssetDatabase.LoadAssetAtPath<T>(fullPathWithName);
            if(asset == null)
                Debug.Log($"{owner}의 {t.ToString()}가 존재하지 않습니다");
        }
        else
        {
            Debug.Log($"{owner}의 {t.ToString()}에 경로를 입력하지 않았습니다");
        }

        return asset != null;
    }
    
    protected void _columnDataInput(string floatData,out float[] array)
    {
        string[] splitedData = floatData.Split(';');
        array = new float[splitedData.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            if (!float.TryParse(splitedData[i], out float value))
            {
                value = 0;
            }

            array[i] = value;
        }
    }
    
    protected void _columnDataInput(string intData,out int[] array)
    {
        string[] splitedData = intData.Split(';');
        array = new int[splitedData.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            if (!int.TryParse(splitedData[i], out int value))
            {
                value = 0;
            }

            array[i] = value;
        }
    }
    
    protected void _columnDataInput(string stringData,out string[] array)
    {
        string[] splitedData = stringData.Split(';');
        array = new string[splitedData.Length];
        for (int i = 0; i < array.Length; ++i)
        {
            array[i] = splitedData[i];
        }
    }

    protected string GetAssetPath(string savePath)
    {
        return savePath + ".asset";
    }

    protected string trimString(string str)
    {
        return str.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
    }
}
