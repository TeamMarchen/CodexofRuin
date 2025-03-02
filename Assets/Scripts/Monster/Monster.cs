using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public MonsterData data;
    [SerializeField]
    private Transform target;
    private SpriteRenderer spriteRenderer;

    [Header("Health Bar Settings")]
    public Image healthBarFill;

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    public bool IsBoss { get; private set; } = false;

    public delegate void MonsterKilledHandler(Monster monster);
    public event MonsterKilledHandler OnMonsterKilled;

    public void Initialize(MonsterData monsterData, Transform playerTarget)
    {
        data = monsterData;
        target = playerTarget;
        data.hp = monsterData.hp; // 체력을 초기화
        UpdateHealthBar();
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

        Vector3 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * data.speed * Time.deltaTime, Space.World);

        UpdateHealthBar();
    }

    public void TakeDamage(float damage)
    {
        data.hp -= damage;
        if (data.hp <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        PlayerStatus.Instance.curruntExp += 30;
        OnMonsterKilled?.Invoke(this);
    }

    private void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = data.curruntHp / data.hp;
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