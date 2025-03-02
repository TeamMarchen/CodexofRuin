using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class SpecDataPaths
{
    public List<Enums.SPEC_DATA_TYPE> SpecDataTypes;
    public List<string> urlPath;
    public List<string> savePath;
}

[CreateAssetMenu(fileName = "NewSpecDataPaths", menuName = "Custom/SpecDataPaths")]
public class SpecDataPathSO : ScriptableObject
{
    public SpecDataPaths specDataPaths;
}

[CustomEditor(typeof(SpecDataPathSO))]
public class SpecDataPathSOEditorScript : Editor
{
    private SpecDataPathSO _specDataPathSO; 
    public override void OnInspectorGUI()
    {
        _specDataPathSO = target as SpecDataPathSO;
        

        if (GUILayout.Button("행 추가"))
        {
            _specDataPathSO.specDataPaths.SpecDataTypes.Add(Enums.SPEC_DATA_TYPE.NONE);
            _specDataPathSO.specDataPaths.urlPath.Add(string.Empty);
            _specDataPathSO.specDataPaths.savePath.Add(string.Empty);
            
        }
        
        if (GUILayout.Button("행 삭제") && _specDataPathSO.specDataPaths.SpecDataTypes.Count > 0)
        {
            int last = _specDataPathSO.specDataPaths.SpecDataTypes.Count - 1;
            _specDataPathSO.specDataPaths.SpecDataTypes.RemoveAt(last);
            _specDataPathSO.specDataPaths.urlPath.RemoveAt(last);
            _specDataPathSO.specDataPaths.savePath.RemoveAt(last);
            
        }

       
        for (int i = 0; i < _specDataPathSO.specDataPaths.SpecDataTypes.Count; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            _specDataPathSO.specDataPaths.SpecDataTypes[i] =
                (Enums.SPEC_DATA_TYPE)EditorGUILayout.EnumPopup(_specDataPathSO.specDataPaths.SpecDataTypes[i],GUILayout.Width(130));
            EditorGUILayout.LabelField("URL Path : ", GUILayout.Width(50));
                
            _specDataPathSO.specDataPaths.urlPath[i] =
                EditorGUILayout.TextField(_specDataPathSO.specDataPaths.urlPath[i],GUILayout.Width(200));
            
            EditorGUILayout.LabelField("Save Path : ",GUILayout.Width(50));
            _specDataPathSO.specDataPaths.savePath[i] =
                EditorGUILayout.TextField(_specDataPathSO.specDataPaths.savePath[i],GUILayout.Width(200));
            EditorGUILayout.EndHorizontal();
        }
        
        if(GUILayout.Button("Path저장"))
        {
            EditorUtility.SetDirty(_specDataPathSO);
            AssetDatabase.SaveAssets();
            serializedObject.ApplyModifiedProperties();
        }

    }
}