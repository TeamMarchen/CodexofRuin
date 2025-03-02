using UnityEngine;

public class LoadingSetter : SceneSetter
{
    private LoadingControlller _loadingCtrl;
    
    private void Awake()
    {
        GetComponents();
    }

    private void GetComponents()
    {
        TryGetComponent(out _loadingCtrl);
    }

    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    private void Initialize()
    {
        _loadingCtrl.Initialize();
        
        //LoadUI 연결
        //
        // uiLoading.Initialize(_loadingCtrl);
        // uiLoading.OpenUI();
    }
}
