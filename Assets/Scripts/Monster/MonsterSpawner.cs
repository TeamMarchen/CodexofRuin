using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSpawner: MonoBehaviour
{
    [Header("Spawn Settings")]
    public Transform player;
    public float minRadius = 5f;
    public float maxRadius = 10f;

    [Header("Monster Prefab")]
    public Monster monsterPrefab;

    private ObjectPool<Monster> monsterPool;

    [Header("Spawning Parameters")]
    public int initialSpawnCount = 10;
    public float spawnInterval = 2f;
    public float spawnIncreaseInterval = 10f;
    private int currentSpawnCount;

    private bool isSpawning = false;
    private Coroutine spawnCoroutine;
    private Coroutine increaseSpawnCoroutine;

    private void Start()
    {
        monsterPool = new ObjectPool<Monster>(monsterPrefab, initialSpawnCount * 10, transform);
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
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnMonsters(currentSpawnCount);
        }
    }

    private IEnumerator IncreaseSpawnCountRoutine()
    {
        while (isSpawning)
        {
            yield return new WaitForSeconds(spawnIncreaseInterval);
            currentSpawnCount += 10;
            Debug.Log($"스폰 수 증가: 현재 스폰 수 = {currentSpawnCount}마리");
        }
    }

    private void SpawnMonsters(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Monster monster = monsterPool.Get(spawnPosition, Quaternion.identity);
            monster.Initialize(GetMonsterData(), player);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float randomDistance = Random.Range(minRadius, maxRadius);
        return player.position + (Vector3)randomDirection * randomDistance;
    }

    private MonsterData GetMonsterData()
    {
        return new MonsterData(
            level: 1,
            hp: 100f,
            speed: 2f,
            attackTime: 1f,
            attackRange: 1.5f,
            baseDamage: 10f,
            dropItemID: new List<string> { "item_01"},
            exp: 50
        );
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
