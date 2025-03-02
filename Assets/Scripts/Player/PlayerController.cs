using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamage
{
    void TakeDamage(float damage);
}

namespace Player
{
    public class PlayerController : MonoBehaviour, IDamage
    {
        [Header("Movement Settings")]
        private float moveSpeed;

        [Header("Key Settings")]
        public PlayerKeySettings keySettings = new PlayerKeySettings();

        [Header("UI Settings")]
        public Image healthBarFill;
        public Image manaBarFill;
        public Image skillCooldownFill;

        private Vector2 moveInput;
        private Rigidbody2D rb;
        public MagicBullet magicBullet;
        public GameObject fireSkill;
        private ObjectPool<MagicBullet> magicBulletPool;
        private bool isSkillFirOnCooldown = false;
        private float skillCooldownTime = 15f;
        private float remainingCooldownTime = 0f;

        private Coroutine attackCoroutine;

        private void Awake()
        {
            moveSpeed = PlayerStatus.Instance.speed;
            magicBulletPool = new ObjectPool<MagicBullet>(magicBullet, 5, transform);
            rb = GetComponent<Rigidbody2D>();
            attackCoroutine = StartCoroutine(AutoAttackRoutine());
            Debug.Log("어택 시작");
        }

        private void Update()
        {
            HandleInput();
            UpdateHealthAndManaUI();
            UpdateSkillCooldownUI();
        }

        private void FixedUpdate()
        {
            MovePlayer();
        }

        private void HandleInput()
        {
            moveInput = Vector2.zero;
            if (Input.GetKey(keySettings.upKey))
                moveInput.y += 1;
            if (Input.GetKey(keySettings.downKey))
                moveInput.y -= 1;
            if (Input.GetKey(keySettings.leftKey))
                moveInput.x -= 1;
            if (Input.GetKey(keySettings.rightKey))
                moveInput.x += 1;
            if (Input.GetKey(keySettings.skillFirKey))
                StartCoroutine(SkillFir());

            moveInput = moveInput.normalized;
        }

        private void MovePlayer()
        {
            Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            if (movement != Vector2.zero)
            {
                float angle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
            }
        }

        private IEnumerator AutoAttackRoutine()
        {
            while (true)
            {
                if (HasEnemies())
                {
                    Attack();
                }
                yield return new WaitForSeconds(0.5f);
            }
        }

        private IEnumerator SkillFir()
        {
            if (isSkillFirOnCooldown) yield break;

            if (PlayerStatus.Instance.curruntMp < 20)
            {
                yield break;
            }

            PlayerStatus.Instance.curruntMp -= 20;
            isSkillFirOnCooldown = true;
            remainingCooldownTime = skillCooldownTime;
            fireSkill.SetActive(true);
            yield return new WaitForSeconds(2f);
            fireSkill.SetActive(false);

            while (remainingCooldownTime > 0)
            {
                remainingCooldownTime -= Time.deltaTime;
                UpdateSkillCooldownUI();
                yield return null;
            }

            isSkillFirOnCooldown = false;
        }

        private void UpdateHealthAndManaUI()
        {
            if (healthBarFill != null)
            {
                healthBarFill.fillAmount = PlayerStatus.Instance.curruntHp / PlayerStatus.Instance.hp;
            }

            if (manaBarFill != null)
            {
                manaBarFill.fillAmount = PlayerStatus.Instance.curruntMp / PlayerStatus.Instance.mp;
            }
        }

        private void UpdateSkillCooldownUI()
        {
            if (skillCooldownFill != null)
            {
                if (isSkillFirOnCooldown)
                {
                    skillCooldownFill.fillAmount = remainingCooldownTime / skillCooldownTime;
                }
                else
                {
                    skillCooldownFill.fillAmount = 0f;
                }
            }
        }

        private bool HasEnemies()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
            return enemies.Length > 0;
        }

        public void Attack()
        {
            GameObject nearestEnemy = FindNearestEnemy();
            if (nearestEnemy != null)
            {
                Vector2 shootDirection = (nearestEnemy.transform.position - this.transform.position).normalized;
                MagicBullet projectile = magicBulletPool.Get(this.transform.position, Quaternion.identity);
                projectile.Initialize(shootDirection, PlayerStatus.Instance.baseDamage, State.Range);
            }
        }

        private GameObject FindNearestEnemy()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Monster");
            GameObject nearestEnemy = null;
            float shortestDistance = 10f;

            foreach (var enemy in enemies)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < shortestDistance)
                {
                    shortestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
            return nearestEnemy;
        }

        public void TakeDamage(float damage)
        {
            PlayerStatus.Instance.curruntHp -= damage - PlayerStatus.Instance.defense;
            Debug.Log(damage - PlayerStatus.Instance.defense);
            if (PlayerStatus.Instance.curruntHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            Debug.Log("플레이어가 사망했습니다.");
            gameObject.SetActive(false);
        }
    }
}
