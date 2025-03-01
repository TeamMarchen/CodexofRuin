using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : MonoBehaviour
{
    [Header("Stage Settings")]
    public int totalStages = 3;
    public float stageTime = 150f;
    public float restTime = 20f;

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
            Debug.Log($"Stage {currentStage} ����!");

            monsterSpawner.StartSpawning();

            remainingTime = stageTime;
            while (remainingTime > 0)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI(remainingTime);
                yield return null;
            }

            Debug.Log($"Stage {currentStage} Ŭ����!");
            monsterSpawner.StopSpawning();
            monsterSpawner.ClearAllMonsters();

            isResting = true;
            Debug.Log($"�޽� �ð� ����: {restTime}��");
            yield return new WaitForSeconds(restTime);
            isResting = false;
        }

        Debug.Log("��� ���������� �Ϸ�Ǿ����ϴ�!");
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
