using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : Component
{
    private readonly T prefab;
    private readonly List<T> pool = new List<T>();
    private readonly Transform parent;

    public ObjectPool(T prefab, int initialSize = 10, Transform parent = null)
    {
        this.prefab = prefab;
        this.parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            var obj = CreateNewObject();
            pool.Add(obj);
        }
    }

    private T CreateNewObject()
    {
        T newObj = Object.Instantiate(prefab, parent);
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    public T Get(Vector3 position, Quaternion rotation)
    {
        // 비활성화된 오브젝트를 우선적으로 찾음
        foreach (var obj in pool)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                PrepareObject(obj, position, rotation);
                return obj;
            }
        }

        // 비활성화된 오브젝트가 없다면 새 오브젝트 생성
        T newObj = CreateNewObject();
        pool.Add(newObj);
        PrepareObject(newObj, position, rotation);
        return newObj;
    }

    private void PrepareObject(T obj, Vector3 position, Quaternion rotation)
    {
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);
    }

    public void Release(T obj)
    {
        obj.gameObject.SetActive(false);
    }
}