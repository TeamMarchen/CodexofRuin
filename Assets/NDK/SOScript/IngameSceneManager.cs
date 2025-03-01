using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject Spawner;
    [SerializeField] private GameObject StageManager;
     
    public void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        //todo 다른 매니저들에게 조립 요청하기
        
    }
    
}
