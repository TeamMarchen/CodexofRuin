using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    private Enums.SceneType _curSceneType = Enums.SceneType.Unknown;  // 현재 Scene
    private Enums.SceneType _nextSceneType = Enums.SceneType.Unknown; // 현재 Scene이 LoadingScene일 경우 다음에 호출 될 Scene

    public Enums.SceneType CurrentSceneType() => _curSceneType;

    public Enums.SceneType NextSceneType() => _nextSceneType;
    

    // Scene 전환
    // LoadingScene으로 전환 시 이후에 나올 Scene도 함께 전달
    // Global/Scenes 안에 불러올 Scene 타입을 정의해야 호출 가능
    // 예시 : SceneManagerEx.Instance.LoadScene(Scenes.MainScene);
    public void LoadScene(Enums.SceneType sceneType, Enums.SceneType nextSceneType = Enums.SceneType.Unknown)
    {
        SceneChangeAction();
        
        _curSceneType = sceneType;
        if (_curSceneType == Enums.SceneType.LoadingScene) _nextSceneType = nextSceneType;
        SceneManager.LoadScene(_curSceneType.ToString());
        SaveNLoadManager.Instance.Save();
    }

    // UI_LoadingScene에서 호출되는 함수
    public AsyncOperation LoadSceneAsync(Enums.SceneType sceneType)
    {
        SceneChangeAction();
        
        _curSceneType = sceneType;
        return SceneManager.LoadSceneAsync(_curSceneType.ToString());
    }

    private void SceneChangeAction()
    {
        // GameObject.FindWithTag("SceneSetter").TryGetComponent(out SceneSetter setter);
        // setter.ClearScene();
        Time.timeScale = 1;
    }
}
