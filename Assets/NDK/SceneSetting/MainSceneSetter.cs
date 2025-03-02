using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneSetter : SceneSetter {
    // Start is called before the first frame update

    [SerializeField] private GameObject MainUI;
    
    protected override void Start()
    {
        base.Start();
        if (SpecDataManager.Instance.GetData<ItemDataSO>(1100,out ItemDataSO itemDataSo))
        {
            Debug.Log(itemDataSo.itemName);
        }

        var data = SpecDataManager.Instance.GetDataDictionary<ItemDataSO>();
        foreach (KeyValuePair<int,ItemDataSO> keyValuePair in data)
        {
            Debug.Log($"{keyValuePair.Value.itemName} {keyValuePair.Value.description}");
        }
    }
}
