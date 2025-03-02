using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;


public class Boss : MonoBehaviour
{
    public MonsterData data;
    public float hp;
    [SerializeField]
    private Transform target;
    private SpriteRenderer spriteRenderer;
    NavMeshAgent agent;

    [Header("Health Bar Settings")]
    public Image healthBarPrefab;

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    public delegate void BossKilledHandler(Boss boss);
    public event BossKilledHandler OnBossKilled;

    public void Initialize(MonsterData monsterData, Transform playerTarget)
    {
        data = monsterData;
        target = playerTarget;
        data.hp = monsterData.hp; // 체력을 초기화
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (target == null) return;

        if (target.position.x > transform.position.x)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }
        hp = data.curruntHp;
        agent.SetDestination(target.position);

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        data.curruntHp -= damage;
        if (data.curruntHp <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        OnBossKilled?.Invoke(this);
    }

    private void UpdateHealthBar()
    {
        if (healthBarPrefab != null)
        {
            healthBarPrefab.fillAmount = data.curruntHp / data.hp;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time < lastDamageTime + damageCooldown) return;

        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage(data.baseDamage);
                lastDamageTime = Time.time;
            }
        }
    }
}