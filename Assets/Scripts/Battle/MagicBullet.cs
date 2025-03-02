using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Single,
    Range
}

public class MagicBullet : MonoBehaviour
{
    
    private float speed = 10f;
    private Vector2 direction;
    private float damage;
    private State state = State.Single;
    private float range = 1f;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
    }

    public void Initialize(Vector2 shootDirection, float damageAmount, State state)
    {
        direction = shootDirection.normalized;
        damage = damageAmount;
        gameObject.SetActive(true);
        this.state = state;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Single && collision.CompareTag("Monster"))
        {
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
                Debug.Log($"몬스터에게 {damage} 피해를 입혔습니다.");
            }
            gameObject.SetActive(false);
        }
        else if (state == State.Range && collision.CompareTag("Monster"))
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damage);
                        Debug.Log($"범위 내 몬스터에게 {damage} 피해를 입혔습니다.");
                    }
                }
            }
            gameObject.SetActive(false);
        }
    }
}
