using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class ResourceManager : Singleton<ResourceManager>
{
    private Dictionary<Type, Dictionary<string, Object>> _singleResourceCache =
        new Dictionary<Type, Dictionary<string, Object>>();
    private Dictionary<Type, Dictionary<string, Object[]>> _arrayResourceCache = 
        new Dictionary<Type, Dictionary<string, Object[]>>();
    
    /// <summary>
    /// Resource폴더 내에서 최상위 타입으로 가져오고자 할 때, 전체 경로가 아닌 하위 폴더부터 이름에 포함
    /// e.g. GameObejct, Sprite, ScriptableObject
    /// </summary>
    /// <param name="sourceName"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadResource<T>(string sourceName) where T : Object
    {
        Type sourceType = typeof(T);

        if (!_singleResourceCache.ContainsKey(sourceType))
        {
            _singleResourceCache[sourceType] = new Dictionary<string, Object>();
        }

        var resourceDictionary = _singleResourceCache[sourceType];

        if (!resourceDictionary.ContainsKey(sourceName))
        {
            string folderName = $"{sourceType.Name}s";

            T resource = Resources.Load<T>($"{folderName}/{sourceName}");

            if (resource == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{sourceType.Name} 로드 실패: {sourceName}");
#endif
                return null;
            }

            resourceDictionary[sourceName] = resource;
        }

        return (T)resourceDictionary[sourceName];
    }
    
    /// <summary>
    /// 구체적인 타입의 오브젝트를 로드하고자 할 때, Resources 다음의 전체 경로 입력 필요
    /// e.g. CharacterData, CharacterAppearenceInfo
    /// </summary>
    /// <param name="fullPath"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T LoadResourceOfPath<T>(string fullPath) where T : Object
    {
        Type sourceType = typeof(T);

        if (!_singleResourceCache.ContainsKey(sourceType))
        {
            _singleResourceCache[sourceType] = new Dictionary<string, Object>();
        }

        var resourceDictionary = _singleResourceCache[sourceType];

        if (!resourceDictionary.ContainsKey(fullPath))
        {
            T resource = Resources.Load<T>(fullPath);

            if (resource == null)
            {
#if UNITY_EDITOR
                Debug.LogError($"{sourceType.Name} 로드 실패: {fullPath}");
#endif
                return null;
            }

            resourceDictionary[fullPath] = resource;
        }

        return (T)resourceDictionary[fullPath];
    }
    

    public T[] LoadAllResources<T>(string subFolder) where T : Object
    {
        Type sourceType = typeof(T);

        if (!_arrayResourceCache.ContainsKey(sourceType))
        {
            _arrayResourceCache[sourceType] = new Dictionary<string, Object[]>();
        }

        var resourceDictionary = _arrayResourceCache[sourceType];

        if (!resourceDictionary.ContainsKey(subFolder))
        {
            string folderName = $"{sourceType.Name}s";

            T[] resources = Resources.LoadAll<T>($"{folderName}/{subFolder}");

            if (resources == null || resources.Length == 0)
            {
#if UNITY_EDITOR
                Debug.LogError($"{sourceType.Name} 로드 실패: {subFolder}");
#endif
                return null;
            }

            resourceDictionary[subFolder] = resources;
        }

        return (T[])resourceDictionary[subFolder];
    }
    
    public T[] LoadAllResourcesOfPath<T>(string fullPath) where T : Object
    {
        Type sourceType = typeof(T);

        if (!_arrayResourceCache.ContainsKey(sourceType))
        {
            _arrayResourceCache[sourceType] = new Dictionary<string, Object[]>();
        }

        var resourceDictionary = _arrayResourceCache[sourceType];

        if (!resourceDictionary.ContainsKey(fullPath))
        {

            T[] resources = Resources.LoadAll<T>(fullPath);

            if (resources == null || resources.Length == 0)
            {
#if UNITY_EDITOR
                Debug.LogError($"{sourceType.Name} 로드 실패: {fullPath}");
#endif
                return null;
            }

            resourceDictionary[fullPath] = resources;
        }

        return (T[])resourceDictionary[fullPath];
    }
}
