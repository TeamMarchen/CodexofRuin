using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : Singleton<StageManager>
{
    [Header("Stage Settings")]
    public int totalStages = 3;
    private float STAGETIME = 150f;
    private float RESTTIME = 20f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Dependencies")]
    public MonsterSpawner monsterSpawner;

    private int currentStage = 0;
    private bool isResting = false;
    private float remainingTime = 0f;

    private void Start()
    {
        StartCoroutine(StageRoutine());
    }

    private IEnumerator StageRoutine()
    {
        while (currentStage < totalStages)
        {
            currentStage++;
            Debug.Log($"Stage {currentStage} 시작!");

            yield return new WaitForSeconds(0.3f);

            monsterSpawner.StartSpawning();

            remainingTime = STAGETIME;
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI(remainingTime);
                yield return null;
            }

            Debug.Log($"Stage {currentStage} 클리어!");
            monsterSpawner.StopSpawning();
            monsterSpawner.ClearAllMonsters();

            isResting = true;
            Debug.Log($"휴식 시간 시작: {RESTTIME}초");
            yield return new WaitForSeconds(RESTTIME);
            isResting = false;
        }

        Debug.Log("모든 스테이지가 완료되었습니다!");
        UpdateTimerUI(0);
    }
    private void UpdateTimerUI(float time)
    {
        time = Mathf.Max(0, time);
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        if (timerText != null)
        {
            timerText.text = $"Stage {currentStage}\nTime Limit: {minutes:00}:{seconds:00}";
        }
    }
}
