using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    private GameObject player;
    public float minRadius = 5f;
    public float maxRadius = 10f;

    [Header("Monster Prefabs")]
    public List<Monster> monsterPrefabs; // 여러 종류의 몬스터 프리팹
    public Monster bossPrefab; // 보스 몬스터 프리팹

    private Dictionary<Monster, int> monsterSpawnCount = new Dictionary<Monster, int>();
    private ObjectPool<Monster> monsterPool;

    [Header("Spawning Parameters")]
    public int initialSpawnCount = 10;
    public float spawnInterval = 2f;
    public float spawnIncreaseInterval = 10f;

    private int currentSpawnCount;
    private int totalSpawnedMonsters = 0; // 현재 스폰된 몬스터 수
    private int killedMonsters = 0; // 처치된 몬스터 수

    [Header("Monster Spawn Limit")]
    public int maxMonsters = 2000; // 최대 몬스터 스폰 수

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

    public void Initialize(GameObject playerObject)
    {
        player = playerObject;
        monsterPool = new ObjectPool<Monster>(bossPrefab, maxMonsters + 1, transform); // 보스 포함하여 +1

        // 초기 몬스터 스폰 설정
        monsterSpawnCount.Clear();
        foreach (var prefab in monsterPrefabs)
        {
            monsterSpawnCount[prefab] = maxMonsters / monsterPrefabs.Count; // 종류별 몬스터 스폰 수 균등 분배
        }
    }

    public void StartSpawning()
    {
        if (isSpawning) return;

        isSpawning = true;
        currentSpawnCount = initialSpawnCount;

        SpawnMonsters(currentSpawnCount);

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
            currentSpawnCount = Mathf.Min(currentSpawnCount + 10, maxMonsters - totalSpawnedMonsters);
            Debug.Log($"스폰 수 증가: 현재 스폰 수 = {currentSpawnCount}마리");
        }
    }

    private void SpawnMonsters(int count)
    {
        int spawnCount = Mathf.Min(count, maxMonsters - totalSpawnedMonsters);

        foreach (var prefab in monsterPrefabs)
        {
            int maxSpawnForType = Mathf.Min(spawnCount, monsterSpawnCount[prefab]);

            for (int i = 0; i < maxSpawnForType; i++)
            {
                Vector3 spawnPosition = GetRandomSpawnPosition();
                Monster monster = monsterPool.Get(spawnPosition, Quaternion.identity);
                monster.Initialize(GetMonsterData(prefab), player.transform);

                monster.OnMonsterKilled += HandleMonsterKilled;
                totalSpawnedMonsters++;
                monsterSpawnCount[prefab]--;

                if (totalSpawnedMonsters >= maxMonsters)
                {
                    return;
                }
            }
        }
    }

    private void SpawnBoss()
    {
        bossSpawned = true;
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Monster boss = monsterPool.Get(spawnPosition, Quaternion.identity);
        boss.Initialize(GetBossData(), player.transform);

        boss.OnMonsterKilled += HandleBossKilled;

        Debug.Log("보스 몬스터 스폰 완료!");
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return player.transform.position + (Vector3)randomDirection * randomDistance;
    }

    private MonsterData GetMonsterData(Monster prefab)
    {
        return new MonsterData(
            level: 1,
            name : "DiDi",
            hp: 50f,
            speed: 2f,
            attackTime: 1f,
            attackRange: 1.5f,
            baseDamage: 10f,
            dropItemID: new List<string> { "item_01" },
            exp: 10
        );
    }

    private MonsterData GetBossData()
    {
        return new MonsterData(
            level: 10,
            name: "DiDi",
            hp: 1000f,
            speed: 1f,
            attackTime: 2f,
            attackRange: 2.5f,
            baseDamage: 50f,
            dropItemID: new List<string> { "boss_item_01" },
            exp: 500
        );
    }

    private void HandleMonsterKilled(Monster monster)
    {
        killedMonsters++;
        monsterPool.Release(monster);
        OnMonsterKilled?.Invoke();
    }

    private void HandleBossKilled(Monster boss)
    {
        monsterPool.Release(boss);
        OnBossKilled?.Invoke();
        OnStageClear?.Invoke(); // 스테이지 클리어 이벤트 호출
        StopSpawning();
        Debug.Log("보스 몬스터 처치! 스테이지 클리어!");
    }

    public void ClearAllMonsters()
    {
        Monster[] activeMonsters = GetComponentsInChildren<Monster>();
        foreach (var monster in activeMonsters)
        {
            monsterPool.Release(monster);
        }

        Debug.Log("모든 몬스터를 제거했습니다.");
    }
}
