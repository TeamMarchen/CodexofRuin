using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button exitButton;

    public void Init(UnityAction startButtonEvent)
    {
        Debug.Assert(startButton);
        Debug.Assert(exitButton);
        
        startButton.onClick.AddListener(startButtonEvent);
        exitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            
        });        
        
    }
    
}
