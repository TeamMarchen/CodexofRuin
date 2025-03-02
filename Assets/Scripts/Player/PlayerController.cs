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
        public Image skillFirCooldownFill;
        public Image skillSecCooldownFill;
        public Image skillThrCooldownFill;

        private Vector2 moveInput;
        private Rigidbody2D rb;
        public MagicBullet magicBullet;
        public GameObject fireSkill;
        public GameObject fireSkill2;
        private ObjectPool<MagicBullet> magicBulletPool;
        private bool isSkillFirOnCooldown = false;
        private bool isSkillSecOnCooldown = false;
        private float skillFirCooldownTime = 15f;
        public float remainingFirCooldownTime = 0f;
        private float skillSecCooldownTime = 30f;
        private float remainingSecCooldownTime = 0f;
        private Coroutine attackCoroutine;
        private SpriteRenderer spriteRenderer;

        public void Initialize(Image skillFirCool, Image skillSecCool, Image skillThrCool,Image hp)
        {
            skillFirCooldownFill = skillFirCool;
            skillSecCooldownFill = skillSecCool;
            skillThrCooldownFill = skillThrCool;
            spriteRenderer = GetComponent<SpriteRenderer>();

            skillThrCooldownFill.fillAmount = 0f;
            moveSpeed = PlayerStatus.Instance.speed;
            magicBulletPool = new ObjectPool<MagicBullet>(magicBullet, 5, transform);
            rb = GetComponent<Rigidbody2D>();
            attackCoroutine = StartCoroutine(AutoAttackRoutine());

            healthBarFill = hp;
        }

        private void Update()
        {
            moveSpeed = PlayerStatus.Instance.speed;
            HandleInput();
            UpdateHealthAndManaUI();
            UpdateFirSkillCooldownUI();
            UpdateSecSkillCooldownUI();
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
            if (Input.GetKey(keySettings.skillSecKey))
                StartCoroutine(SkillSec());
            if (Input.GetKey(keySettings.skillThrKey))
                StartCoroutine(SkillThr());

            moveInput = moveInput.normalized;
        }

        private void MovePlayer()
        {
            Vector2 movement = moveInput * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + movement);
            if (moveInput.x > 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
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
            if(PlayerStatus.Instance.level >= 1){
                if (isSkillFirOnCooldown) yield break;

                if (PlayerStatus.Instance.curruntMp < 20)
                {
                    yield break;
                }

                PlayerStatus.Instance.curruntMp -= 20;
                isSkillFirOnCooldown = true;
                SoundManager.Instance.Play("SFX_PenClick");
                remainingFirCooldownTime = skillFirCooldownTime;
                fireSkill.SetActive(true);
                yield return new WaitForSeconds(2f);
                fireSkill.SetActive(false);

                while (remainingFirCooldownTime > 0)
                {
                    remainingFirCooldownTime -= Time.deltaTime;
                    UpdateFirSkillCooldownUI();
                    yield return null;
                }
                isSkillFirOnCooldown = false;
            }
        }

        private IEnumerator SkillSec()
        {
            if (PlayerStatus.Instance.level >= 1)
            {
                if (isSkillSecOnCooldown) yield break;

                if (PlayerStatus.Instance.curruntMp < 20)
                {
                    yield break;
                }

                PlayerStatus.Instance.curruntMp -= 20;
                isSkillSecOnCooldown = true;
                SoundManager.Instance.Play("SFX_PenClick");
                remainingSecCooldownTime = skillSecCooldownTime;
                fireSkill2.SetActive(true);
                yield return new WaitForSeconds(5f);
                fireSkill2.SetActive(false);

                while (remainingSecCooldownTime > 0)
                {
                    remainingSecCooldownTime -= Time.deltaTime;
                    UpdateSecSkillCooldownUI();
                    yield return null;
                }
                yield return new WaitForSeconds(30f);
                isSkillSecOnCooldown = false;
            }
        }

        private IEnumerator SkillThr()
        {
            if (skillThrCooldownFill.fillAmount != 1)
            {
                SoundManager.Instance.Play("SFX_PenClick");
                skillThrCooldownFill.fillAmount = 1;
                PlayerStatus.Instance.extraAttackPower += 150;
                yield return null;
            }
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

        private void UpdateFirSkillCooldownUI()
        {
            if (skillFirCooldownFill != null)
            {
                skillFirCooldownFill.fillAmount = remainingFirCooldownTime / skillFirCooldownTime;
            }
        }
        private void UpdateSecSkillCooldownUI()
        {
            if (skillSecCooldownFill != null)
            {
                skillSecCooldownFill.fillAmount = remainingSecCooldownTime / skillSecCooldownTime;
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
