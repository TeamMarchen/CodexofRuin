using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneSetter : MonoBehaviour
{
    protected static bool IsSingletonInitialze;

    protected virtual void Start()
    {
        if (!IsSingletonInitialze)
        {
            SingletonInit();
        }
    }

    private void SingletonInit()
    {
        SpecDataManager.Instance.Init();
        SoundManager.Instance.GetAudio += ResourceManager.Instance.LoadResource<AudioClip>;
        
    }
}
