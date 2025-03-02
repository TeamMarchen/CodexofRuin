using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Player;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [Header("Stage Settings")]
    public int totalStages = 3;
    private float STAGETIME = 150f;
    private float BOSSTIME = 150f;
    private float RESTTIME = 10f;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject skillSelectionPanel;
    public Image characterFullImage;

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
    private bool isSkillActivated = false;

    private CharacterDataSO bossData;
    private List<int> monsterId = new List<int>();
    private int[] monsterCount;
    private IReadOnlyDictionary<int, MonsterDataSO> monsterData;

    public void Initialize(IReadOnlyDictionary<int, MonsterDataSO> monsterDataSos_, IReadOnlyDictionary<int, CharacterDataSO> characterDataSos_,
        StageDataSO stageDataSos_, IReadOnlyDictionary<int, PlayerDataSO> playerDataSos_)
    {
        playerObject = Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
        spawnerObject = Instantiate(spawner, new Vector3(0, 0, 0), Quaternion.identity);
        camera = Instantiate(camera, new Vector3(0, 0, 0), Quaternion.identity);

        monsterSpawner = spawnerObject.GetComponent<MonsterSpawner>();
        monsterSpawner.OnMonsterKilled += HandleMonsterKilled;
        monsterSpawner.OnBossKilled += HandleBossKilled;

        monsterData = monsterDataSos_;
        foreach (var data in characterDataSos_)
        {
            if (data.Value.name == stageDataSos_.bossType)
            {
                bossData = data.Value;
                break;
            }
        }
        foreach (var monsterName in stageDataSos_.monsterType)
        {
            foreach (var data in monsterDataSos_)
            {
                if (data.Value.monsterName.Equals(monsterName))
                {
                    monsterId.Add(data.Key);
                    break;
                }
            }
        }
        monsterCount = stageDataSos_.monsterSpawnRate;
        STAGETIME = stageDataSos_.timeLimit;
        BOSSTIME = stageDataSos_.bossClearTime;
        totalMonstersToKill = stageDataSos_.maxMonsterSpawnCount;

        playerController = playerObject.GetComponent<PlayerController>();

        camera.Setting(playerObject);

        StartCoroutine(StageRoutine());
        StartCoroutine(PlayerRegenerationRoutine());
    }

    private IEnumerator PlayerRegenerationRoutine()
    {
        while (!isStageFailed && !isResting)
        {
            PlayerStatus.Instance.curruntHp = Mathf.Min(PlayerStatus.Instance.curruntHp + 6, PlayerStatus.Instance.hp);
            PlayerStatus.Instance.curruntMp = Mathf.Min(PlayerStatus.Instance.curruntMp + 1, PlayerStatus.Instance.mp);

            yield return new WaitForSeconds(1f);
        }
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
            isSkillActivated = false;

            monsterSpawner.Initialize(playerObject, monsterId, monsterCount, monsterData, bossData, totalMonstersToKill);

            Debug.Log($"Stage {currentStage} 시작!");

            yield return new WaitForSeconds(0.3f);

            monsterSpawner.StartSpawning();

            remainingTime = STAGETIME;
            while (remainingTime > 0 && !isStageCleared && !isStageFailed)
            {
                remainingTime -= Time.deltaTime;
                UpdateTimerUI(remainingTime);

                if (!isSkillActivated && remainingTime <= 75f && PlayerStatus.Instance.curruntHp >= PlayerStatus.Instance.hp * 0.5f)
                {
                    ActivateSkillEvent();
                }

                if (remainingTime <= 0 && !bossDefeated)
                {
                    HandleStageFailed();
                }

                if (PlayerStatus.Instance.curruntHp < PlayerStatus.Instance.hp * 0.1f)
                {
                    Debug.Log("플레이어 체력이 10% 미만입니다. 실패 처리!");
                    HandleStageFailed();
                }

                yield return null;
            }

            if (isStageCleared)
            {
                Debug.Log($"Stage {currentStage} 클리어!");
            }

            monsterSpawner.StopSpawning();
            monsterSpawner.ClearAllMonsters();

            if (isStageFailed)
            {
                Debug.Log("스테이지 실패!");
                break;
            }

            isResting = true;
            yield return new WaitForSeconds(RESTTIME);
            isResting = false;
        }

        if (!isStageFailed)
        {
            Debug.Log("모든 스테이지가 완료되었습니다!");
            UpdateTimerUI(0);
        }
    }

    private void ActivateSkillEvent()
    {
        isSkillActivated = true;
        Time.timeScale = 0f;
        skillSelectionPanel.SetActive(true);
        Debug.Log("플레이어 고유 스킬 발동! 선택지를 표시합니다.");
    }

    public void OnSkillSelected()
    {
        skillSelectionPanel.SetActive(false);
        StartCoroutine(ShowCharacterFullImageRoutine());
        Time.timeScale = 1f;
    }

    private IEnumerator ShowCharacterFullImageRoutine()
    {
        characterFullImage.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        characterFullImage.gameObject.SetActive(false);
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

        if (killedMonsters >= totalMonstersToKill)
        {
            remainingTime = BOSSTIME;
            monsterSpawner.ClearAllMonsters();
            Debug.Log("모든 몬스터를 처치했습니다. 보스를 처치하세요!");
        }
    }

    private void HandleBossKilled()
    {
        bossDefeated = true;
        isStageCleared = true;
        Debug.Log("보스 처치 완료! 스테이지 클리어!");
    }

    private void HandleStageFailed()
    {
        isStageFailed = true;
        monsterSpawner.StopSpawning();
        monsterSpawner.ClearAllMonsters();
        UpdateTimerUI(0);

        Time.timeScale = 0;
        Debug.Log("스테이지 실패! 게임 오버.");
    }
}