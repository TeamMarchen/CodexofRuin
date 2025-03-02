using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneSetter : SceneSetter {
    // Start is called before the first frame update

    protected override void Start()
    {
        base.Start();
        if (SpecDataManager.Instance.GetData<ItemDataSO>(1100,out ItemDataSO itemDataSo))
        {
            Debug.Log(itemDataSo.itemName);
        }

        var data = SpecDataManager.Instance.GetDataList<ItemDataSO>();
        foreach (KeyValuePair<int,SpecDataSO> keyValuePair in data)
        {
            ItemDataSO test = keyValuePair.Value as ItemDataSO;
            
            Debug.Log($"{test.itemName} {test.description}");
        }
    }
}
