using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneSetter : SceneSetter
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Debug.Log("LobbyScene");
        MoveScene(Enums.SCENE_TYPE.BattleScene);
    }

}
