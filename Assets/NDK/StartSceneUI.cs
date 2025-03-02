using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneUI : SceneSetter
{
    [SerializeField] private Button startButton;

    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.Play("BookofDoomDEMO",Enums.AUDIO_TYPE.BGM,true);
        startButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.Play("SFX_Scifi_Computer_Loading_Glitch_Shorts2");
            SoundManager.Instance.StopBGM();
        });
    }
}
