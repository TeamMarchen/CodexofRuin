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
    public Animator animator;

    private void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public void Initialize(Vector2 shootDirection, float damageAmount, State state)
    {
        animator = GetComponent<Animator>();
        speed = 10f;
        direction = shootDirection.normalized;
        damage = damageAmount;
        gameObject.SetActive(true);
        this.state = state;
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (state == State.Single && collision.collider.CompareTag("Monster"))
        {
            speed = 0;
            animator.SetTrigger("Hit");
            Monster monster = collision.collider.GetComponent<Monster>();
            Boss boss = collision.collider.GetComponent<Boss>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            } else if (boss != null)
            {
                boss.TakeDamage(damage);
            }
            damage = 0;
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
        else if (state == State.Range && collision.collider.CompareTag("Monster"))
        {
            speed = 0;
            animator.SetTrigger("Hit");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = collision.collider.GetComponent<Monster>();
                    Boss boss = collision.collider.GetComponent<Boss>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damage);
                    }
                    else if (boss != null)
                    {
                        boss.TakeDamage(damage);
                    }
                }
            }
            damage = 0;
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            speed = 0;
            damage = 0;
            animator.SetTrigger("Hit");
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}