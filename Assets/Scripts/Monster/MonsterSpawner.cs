using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    private GameObject player;
    public float minRadius = 8f;
    public float maxRadius = 10f;

    [Header("Spawning Parameters")]
    public float spawnInterval = 1f;
    public float spawnIncreaseInterval = 5f;
    private int currentSpawnCount = 1;
    private int totalSpawnedMonsters = 0;
    private int killedMonsters = 0;

    [Header("Monster Spawn Limit")]
    public int maxMonsters = 2000;

    private bool isSpawning = false;
    private bool bossSpawned = false;
    private Coroutine spawnCoroutine;
    private Coroutine increaseSpawnCoroutine;

    public delegate void MonsterKilledHandler();
    public event MonsterKilledHandler OnMonsterKilled;

    public delegate void BossKilledHandler();
    public event BossKilledHandler OnBossKilled;

    public delegate void StageClearHandler();
    public event StageClearHandler OnStageClear;

    private IReadOnlyDictionary<int, MonsterDataSO> monsterDataSos;
    private List<int> monsterIdList;
    private CharacterDataSO bossDataSO;

    public void Initialize(GameObject playerObject, List<int> monsterIdList, int[] spawnCount,
        IReadOnlyDictionary<int, MonsterDataSO> monsterDataSos_, CharacterDataSO characterDataSO, int maxSpawnCount)
    {
        player = playerObject;
        maxMonsters = maxSpawnCount;

        this.monsterIdList = monsterIdList;
        monsterDataSos = monsterDataSos_;
        bossDataSO = characterDataSO;
    }

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        totalSpawnedMonsters = 0;
        killedMonsters = 0;

        spawnCoroutine = StartCoroutine(SpawnRoutine());
        increaseSpawnCoroutine = StartCoroutine(IncreaseSpawnCountRoutine());
    }

    public void StopSpawning()
    {
        if (!isSpawning) return;

        isSpawning = false;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }

        if (increaseSpawnCoroutine != null)
        {
            StopCoroutine(increaseSpawnCoroutine);
        }
    }

    private IEnumerator SpawnRoutine()
    {
        while (isSpawning && !bossSpawned)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (totalSpawnedMonsters < maxMonsters)
            {
                SpawnMonsters(currentSpawnCount);
            }
            else if (!bossSpawned)
            {
                SpawnBoss();
            }
        }
    }

    private IEnumerator IncreaseSpawnCountRoutine()
    {
        while (isSpawning && !bossSpawned)
        {
            yield return new WaitForSeconds(spawnIncreaseInterval);
            currentSpawnCount += 5;
        }
    }

    private void SpawnMonsters(int count)
    {
        if (monsterIdList.Count == 0) return;

        int monsterId = monsterIdList[0];
        if (!monsterDataSos.TryGetValue(monsterId, out MonsterDataSO monsterData))
        {
            Debug.LogError($"몬스터 데이터가 존재하지 않습니다: ID {monsterId}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>($"Prefebs/Monsters/{monsterData.monsterName}");
        if (prefab == null)
        {
            Debug.LogError($"몬스터 프리팹을 찾을 수 없습니다: {monsterData.monsterName}");
            return;
        }

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject monsterObject = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
            Monster monster = monsterObject.GetComponent<Monster>();
            MonsterData monData = new MonsterData(1, monsterData.name, monsterData.maxHp,1f,0,0, monsterData.attack,null,10, monsterData.defense);
            if (monster == null)
            {
                Debug.LogError($"Monster 컴포넌트를 찾을 수 없습니다: {monsterData.name}");
                Destroy(monsterObject);
                continue;
            }

            monster.Initialize(monData, player.transform);
            monster.OnMonsterKilled += HandleMonsterKilled;
            totalSpawnedMonsters++;

            if (totalSpawnedMonsters >= maxMonsters)
            {
                return;
            }
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true;
        ClearAllMonsters();

        GameObject prefab = Resources.Load<GameObject>($"Prefebs/Monster/{bossDataSO.name}");
        if (prefab == null)
        {
            Debug.LogError($"보스 몬스터 프리팹을 찾을 수 없습니다: {bossDataSO.name}");
            return;
        }

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject bossObject = Instantiate(prefab, spawnPosition, Quaternion.identity, transform);
        Boss boss = bossObject.GetComponent<Boss>();
        if (boss == null)
        {
            Debug.LogError($"Boss 컴포넌트를 찾을 수 없습니다: {bossDataSO.name}");
            Destroy(bossObject);
            return;
        }
        MonsterData bossData = new MonsterData(bossDataSO.level, bossDataSO.name, bossDataSO.maxHP, 2f,1f,5f, bossDataSO.attack, null,100, bossDataSO.defense);
        boss.Initialize(bossData, player.transform);
        boss.OnBossKilled += HandleBossKilled;
        Debug.Log("보스 몬스터가 스폰되었습니다!");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return player.transform.position + (Vector3)randomDirection * randomDistance;
    }

    private void HandleMonsterKilled(Monster monster)
    {
        killedMonsters++;
        Destroy(monster.gameObject);
        OnMonsterKilled?.Invoke();

        if (killedMonsters >= maxMonsters)
        {
            SpawnBoss();
        }
    }

    private void HandleBossKilled(Boss boss)
    {
        Destroy(boss.gameObject);
        OnBossKilled?.Invoke();
        OnStageClear?.Invoke();
        StopSpawning();
    }

    public void ClearAllMonsters()
    {
        Monster[] activeMonsters = GetComponentsInChildren<Monster>();
        foreach (var monster in activeMonsters)
        {
            Destroy(monster.gameObject);
        }
    }
}