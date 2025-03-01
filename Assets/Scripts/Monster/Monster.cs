using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour 
{
    public MonsterData data;
    [SerializeField]
    private Transform target;
    private SpriteRenderer spriteRenderer;

    private float damageCooldown = 1f;
    private float lastDamageTime = 0f;
    public void Initialize(MonsterData monsterData, Transform playerTarget)
    {
        data = monsterData;
        target = playerTarget;
    }

    private void Awake()
    {
        // SpriteRenderer 컴포넌트 참조
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer가 Monster 오브젝트에 없습니다!");
        }
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
        data.hp -= damage;
        if (data.hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
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
                Debug.Log($"플레이어에게 {data.baseDamage} 데미지를 입혔습니다.");
            }
        }
    }
}
