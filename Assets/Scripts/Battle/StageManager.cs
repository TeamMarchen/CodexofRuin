using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageManager : Singleton<StageManager>
{
    [Header("Stage Settings")]
    public int totalStages = 3;
    private float STAGETIME = 150f;
    private float BOSSTIME = 150f;
    private float RESTTIME = 10f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    [Header("Dependencies")]
    private MonsterSpawner monsterSpawner;

    private int currentStage = 0;
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
    private bool isResting = false;

    private CharacterDataSO bossData;
    private List<int> monsterId;
    private IReadOnlyDictionary<int, MonsterDataSO> monsterData;

    public void Initialize(IReadOnlyDictionary<int, MonsterDataSO> monsterDataSos_, IReadOnlyDictionary<int, CharacterDataSO> characterDataSos_, 
        StageDataSO stageDataSos_, IReadOnlyDictionary<int, PlayerDataSO> playerDataSos_)
    {
        playerObject = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        spawnerObject = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity);

        monsterSpawner = spawnerObject.GetComponent<MonsterSpawner>();
        monsterSpawner.OnMonsterKilled += HandleMonsterKilled;
        monsterSpawner.OnBossKilled += HandleBossKilled;

        // bossData = characterDataSos_[stageDataSos_.bossType];
        // monsterId = stageDataSos_.monsterType
        monsterData = monsterDataSos_;
        STAGETIME = stageDataSos_.timeLimit;
        BOSSTIME = stageDataSos_.bossClearTime;
        totalMonstersToKill = stageDataSos_.maxMonsterSpawnCount;

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

            monsterSpawner.Initialize(playerObject, monsterId, monsterData, bossData);
            Debug.Log($"Stage {currentStage} !");

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
            }

            monsterSpawner.StopSpawning();
            monsterSpawner.ClearAllMonsters();

            if (isStageFailed)
            {
                break;
            }

            isResting = true;
            yield return new WaitForSeconds(RESTTIME);
            isResting = false;
        }

        if (!isStageFailed)
        {
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

        if (killedMonsters >= totalMonstersToKill && bossDefeated)
        {
            isStageCleared = true;
        }
    }

    private void HandleBossKilled()
    {
        bossDefeated = true;

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
        UpdateTimerUI(0);

        Time.timeScale = 0;
    }
}
