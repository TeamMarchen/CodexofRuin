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
        // Rigidbody2D ����
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // Collider ���� (Trigger Ȱ��ȭ)
        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    // �ʱ�ȭ �޼��� (�߻� ���� ����)
    public void Initialize(Vector2 shootDirection, float damageAmount)
    {
        direction = shootDirection.normalized;
        damage = damageAmount;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        // �߻�� �������� �̵�
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // "Monster" �±׿� �浹 �� ��Ȱ��ȭ
        if (collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Debug.Log($"���Ϳ��� {damage} ���ظ� �������ϴ�.");
            }

            gameObject.SetActive(false);
        }
    }
}
