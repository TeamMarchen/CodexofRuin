using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBullet : MonoBehaviour
{
    public float speed = 10f;
    private Vector2 direction;
    private float damage;

    private void Awake()
    {
        // Rigidbody2D 설정
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Collider 설정 (Trigger 활성화)
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    // 초기화 메서드 (발사 방향 설정)
    public void Initialize(Vector2 shootDirection, float damageAmount)
    {
        direction = shootDirection.normalized;
        damage = damageAmount;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // 발사된 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Monster" 태그와 충돌 시 비활성화
        if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Debug.Log($"몬스터에게 {damage} 피해를 입혔습니다.");
            }

            gameObject.SetActive(false);
        }
    }
}
