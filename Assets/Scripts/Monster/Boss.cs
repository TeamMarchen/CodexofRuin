using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public MonsterData data;
    [SerializeField]
    private Transform target;
    private SpriteRenderer spriteRenderer;

    [Header("Health Bar Settings")]
    public GameObject healthBarPrefab;
    private Image healthBarFill;
    private GameObject healthBar;

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    public delegate void BossKilledHandler(Boss boss);
    public event BossKilledHandler OnBossKilled;

    public void Initialize(MonsterData monsterData, Transform playerTarget)
    {
        data = monsterData;
        target = playerTarget;
        data.hp = monsterData.hp; // 체력을 초기화

        if (healthBar == null)
        {
            CreateHealthBar();
        }
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
        data.hp -= damage - data.defense;
        if (data.hp <= 0)
        {
            Die();
        }
        UpdateHealthBar();
    }

    private void Die()
    {
        gameObject.SetActive(false);
        if (healthBar != null)
        {
            Destroy(healthBar);
        }
        OnBossKilled?.Invoke(this);
    }

    private void CreateHealthBar()
    {
        if (healthBarPrefab == null)
        {
            Debug.LogError("Health bar prefab is not assigned.");
            return;
        }
        healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);
        healthBar.transform.localPosition = new Vector3(0, -0.5f, 0); // 보스 하단에 위치
        healthBarFill = healthBar.transform.Find("Fill").GetComponent<Image>();
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