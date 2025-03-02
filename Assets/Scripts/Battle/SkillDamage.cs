using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDamage : MonoBehaviour
{
    private Coroutine damageCoroutine;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            damageCoroutine = StartCoroutine(ApplyDamageOverTime(collision));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    private IEnumerator ApplyDamageOverTime(Collider2D target)
    {
        while (true)
        {
            Monster monster = target.GetComponent<Monster>();
            Boss boss = target.GetComponent<Boss>();
            if (monster != null)
            {
                monster.TakeDamage(PlayerStatus.Instance.level * 40);
            }else if (boss != null)
            {
                boss.TakeDamage(PlayerStatus.Instance.level * 40);
            }

            yield return new WaitForSeconds(1f); // 1초마다 데미지 적용
        }

    }
}
