using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LobbySceneSetter : SceneSetter
{
    [SerializeField] private GameObject MainUI;
    // Start is called before the first frame update
    protected  override async void Start()
    {
        base.Start();
                
        Debug.Assert(MainUI);

        Instantiate(MainUI);

        //testcode
        await Task.Delay(2000);
        
        MoveScene(Enums.SCENE_TYPE.BattleScene);
    }

}
