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

        Collider2D collider = GetComponent<Collider2D>();
        collider.isTrigger = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (state == State.Single && collision.CompareTag("Monster"))
        {
            speed = 0;
            animator.SetTrigger("Hit");
            Monster monster = collision.GetComponent<Monster>();
            if (monster != null)
            {
                monster.TakeDamage(damage);
            }
            damage = 0;
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
        else if (state == State.Range && collision.CompareTag("Monster"))
        {
            speed = 0;
            animator.SetTrigger("Hit");
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, range);
            foreach (Collider2D hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Monster"))
                {
                    Monster monster = hitCollider.GetComponent<Monster>();
                    if (monster != null)
                    {
                        monster.TakeDamage(damage);
                    }
                }
            }
            damage = 0;
            StartCoroutine(DeactivateAfterDelay(0.5f));
        }
    }

    private IEnumerator DeactivateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameObject.SetActive(false);
    }
}