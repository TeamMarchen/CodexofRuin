using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainSceneSetter : SceneSetter {
    // Start is called before the first frame update

    [SerializeField] private GameObject MainUI;
    
    protected override void Start()
    {
        base.Start();
        GameObject mainuiObj = Instantiate(MainUI);

        if (mainuiObj.TryGetComponent(out MainSceneUI mainSceneUI))
        {
            mainSceneUI.Init(() =>
            {
                
            });
        }
    }
}
