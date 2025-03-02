using System.Collections;
using UnityEngine;

public class LoadingControlller : MonoBehaviour
{
    private Enums.SCENE_TYPE _nextSceneType = Enums.SCENE_TYPE.Unknown;
    private readonly float _smoothTime = 0.8f;
    
    public float Progress { get; private set; }
    
    public void Initialize()
    {
        StartLoading();
    }

    private void StartLoading()
    {
        _nextSceneType = SceneManagerEx.Instance.NextSceneType();
        Time.timeScale = 1;
        if (_nextSceneType != Enums.SCENE_TYPE.Unknown) StartCoroutine(LoadSceneProcess());
    }
    
    IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManagerEx.Instance.LoadSceneAsync(_nextSceneType);
        op.allowSceneActivation = false;
    
        float timer = 0f;
        while (!op.isDone)
        {
            timer += Time.deltaTime;

            float targetProgress = Mathf.Clamp01(op.progress / 0.9f);
            Progress = Mathf.Lerp(Progress, targetProgress, _smoothTime * Time.deltaTime);
            
            if (timer > 5f)
            {
                op.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
