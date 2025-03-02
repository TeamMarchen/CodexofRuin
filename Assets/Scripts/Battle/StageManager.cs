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
    private MonsterSpawner monsterSpawner;

    private int currentStage = 0;
    private bool isResting = false;
    private float remainingTime = 0f;

    [SerializeField]
    private GameObject player;
    private GameObject playerObject;
    private PlayerController playerController;

    [SerializeField]
    private GameObject spawner;
    private GameObject spawnerObject;

    [SerializeField]
    private CameraManager camera;

    private int totalMonstersToKill = 2000;
    private int killedMonsters = 0;
    private bool bossDefeated = false;

    private bool isStageCleared = false;
    private bool isStageFailed = false;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        playerObject = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        spawnerObject = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity);

        monsterSpawner = spawnerObject.GetComponent<MonsterSpawner>();
        monsterSpawner.OnMonsterKilled += HandleMonsterKilled;
        monsterSpawner.OnBossKilled += HandleBossKilled;

        playerController = player.GetComponent<PlayerController>();
        camera.Setting(playerObject);

        StartCoroutine(StageRoutine());
    }

    private IEnumerator StageRoutine()
    {
        while (currentStage < totalStages && !isStageFailed)
        {
            currentStage++;
            killedMonsters = 0;
            bossDefeated = false;
            isStageCleared = false;
            isStageFailed = false;

            monsterSpawner.Initialize(playerObject);
            Debug.Log($"Stage {currentStage} ����!");

            yield return new WaitForSeconds(0.3f);

            monsterSpawner.StartSpawning();

            remainingTime = STAGETIME;
            while (remainingTime > 0 && !isStageCleared && !isStageFailed)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI(remainingTime);

                if (remainingTime <= 0 && (!isStageCleared || !bossDefeated))
                {
                    HandleStageFailed();
                }

                yield return null;
            }

            if (isStageCleared)
            {
                Debug.Log($"Stage {currentStage} Ŭ����!");
            }

            monsterSpawner.StopSpawning();
            monsterSpawner.ClearAllMonsters();

            if (isStageFailed)
            {
                Debug.Log("���� ����: ��ǥ�� �޼����� ���߽��ϴ�.");
                break;
            }

            isResting = true;
            Debug.Log($"�޽� �ð� ����: {RESTTIME}��");
            yield return new WaitForSeconds(RESTTIME);
            isResting = false;
        }

        if (!isStageFailed)
        {
            Debug.Log("��� ���������� �Ϸ�Ǿ����ϴ�!");
            UpdateTimerUI(0);
        }
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

    private void HandleMonsterKilled()
    {
        killedMonsters++;
        Debug.Log($"���� óġ: {killedMonsters}/{totalMonstersToKill}");

        if (killedMonsters >= totalMonstersToKill && bossDefeated)
        {
            isStageCleared = true;
        }
    }

    private void HandleBossKilled()
    {
        bossDefeated = true;
        Debug.Log("���� ���͸� óġ�߽��ϴ�!");

        if (killedMonsters >= totalMonstersToKill && bossDefeated)
        {
            isStageCleared = true;
        }
    }

    private void HandleStageFailed()
    {
        isStageFailed = true;
        monsterSpawner.StopSpawning();
        monsterSpawner.ClearAllMonsters();
        Debug.Log("�������� ����: �ð� ���� ��ǥ�� �޼����� ���߽��ϴ�!");
        UpdateTimerUI(0);

        Time.timeScale = 0;
    }
}
