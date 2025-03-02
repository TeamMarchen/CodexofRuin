using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EbdSceneUI : SceneSetter
{
    protected override void Start()
    {
        base.Start();
        SoundManager.Instance.Play("SFX_Scifi_Computer_Loading_Glitch",Enums.AUDIO_TYPE.BGM,true);
    }

}
