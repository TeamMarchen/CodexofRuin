using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public MonsterData data;
    [SerializeField]
    private Transform target;
    private SpriteRenderer spriteRenderer;

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;

    public delegate void BossKilledHandler(Boss boss);
    public event BossKilledHandler OnBossKilled;

    public void Initialize(MonsterData monsterData, Transform playerTarget)
    {
        data = monsterData;
        target = playerTarget;
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
    }

    public void TakeDamage(float damage)
    {
        data.hp -= damage - data.defense;
        if (data.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);

        OnBossKilled?.Invoke(this);
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
