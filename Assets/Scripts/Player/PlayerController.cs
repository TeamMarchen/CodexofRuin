using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IDamage
{
    void TakeDamage(float damage);
}
public class PlayerController : MonoBehaviour, IDamage
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Key Settings")]
    public PlayerKeySettings keySettings = new PlayerKeySettings();

    private Vector2 moveInput;
    private Rigidbody2D rb;
    public MagicBullet magicBullet;
    private ObjectPool<MagicBullet> magicBulletPool;

    private Coroutine attackCoroutine;

    private void Start()
    {
        magicBulletPool = new ObjectPool<MagicBullet>(magicBullet, 10, transform);

        attackCoroutine = StartCoroutine(AutoAttackRoutine());
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void HandleInput()
    {
        moveInput = Vector2.zero;
        if (Input.GetKey(keySettings.upKey))
            moveInput.y += 1;
        if (Input.GetKey(keySettings.downKey))
            moveInput.y -= 1;
        if (Input.GetKey(keySettings.leftKey))
            moveInput.x -= 1;
        if (Input.GetKey(keySettings.rightKey))
            moveInput.x += 1;

        moveInput = moveInput.normalized;
    }

    private void MovePlayer()
    {
        Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    private IEnumerator AutoAttackRoutine()
    {
        while (true)
        {
            // "Monster" 태그의 적이 존재할 때만 공격
            if (HasEnemies())
            {
                Attack();
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private bool HasEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        return enemies.Length > 0;
    }

    public void Attack()
    {
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            Vector2 shootDirection = (nearestEnemy.transform.position - this.transform.position).normalized;
            MagicBullet projectile = magicBulletPool.Get(this.transform.position, Quaternion.identity);

            projectile.Initialize(shootDirection, 100);
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
        GameObject nearestEnemy = null;
        float shortestDistance = 10f;

        foreach (var enemy in enemies)
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                nearestEnemy = enemy;
            }
        }
        return nearestEnemy;
    }

    public void TakeDamage(float damage)
    {
        /*
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
        */
    }
}
