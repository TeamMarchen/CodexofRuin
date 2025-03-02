using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SceneSetter : MonoBehaviour
{
    private static bool IsSingletonInitialze=false;

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
        IsSingletonInitialze = true;
    }

    public void MoveScene(Enums.SCENE_TYPE sceneType_)
    {
        SceneManagerEx.Instance.LoadScene(sceneType_);
    }
}
