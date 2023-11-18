using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    public int speed;
    public int currentHealth;
    public int maxHealth;
    public float invincibleTime;
    public float pickRange;

    public GameObject minSpawnPoint;
    public GameObject maxSpawnPoint;
    public GameObject gun;
    
    private float timer;
    private bool isInvincible;
    private bool isDead;
    private bool canMove;
    
    private bool isMoving;
    private float inputX;
    private float inputY;
    private Vector2 movementInput;
    
    private new Rigidbody2D rigidbody;
    private Animator anim;
    private SpriteRenderer spriteRenderer;
    
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        canMove = true;
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        SpriteDirectionChecker();
        if (canMove)
        {
            PlayerMovementInput();
        }
        else
        {
            isMoving = false;
        }
        
        SwitchAnimation();
        
        if (isInvincible)
        {
            TimerCountDown();
        }
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            Move();
        }
    }

    private void OnEnable()
    {
        EventHandler.DamagePlayerEvent += AtDamagePlayerEvent;
        EventHandler.RestorePlayerHealthEvent += AtRestorePlayerHealthEvent;
        EventHandler.AddMaxHeartEvent += AtAddMaxHeartEvent;
        EventHandler.MoveToPositionEvent += AtMoveToPositionEvent;
        EventHandler.StartNewGameEvent += AtStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.DamagePlayerEvent -= AtDamagePlayerEvent;
        EventHandler.RestorePlayerHealthEvent -= AtRestorePlayerHealthEvent;
        EventHandler.AddMaxHeartEvent -= AtAddMaxHeartEvent;
        EventHandler.MoveToPositionEvent -= AtMoveToPositionEvent;
        EventHandler.StartNewGameEvent -= AtStartNewGameEvent;
    }

    #region 事件

    /// <summary>
    /// Player接收伤害
    /// </summary>
    /// <param name="damage">伤害</param>
    private void AtDamagePlayerEvent(int damage)
    {
        if (!isInvincible)
        {
            currentHealth -= damage;
            EventHandler.CallUpdateHealthBar(currentHealth);
            EventHandler.CallCameraShakeEvent();
            
            if (currentHealth <= 0)
            {
                PlayerDie();
            }
            else
            {
                isInvincible = true;
                StartCoroutine(GetHurt());
            }
        }
    }
    
    private void AtRestorePlayerHealthEvent()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += 1;
            EventHandler.CallUpdateHealthBar(currentHealth);
        }
    }
    
    private void AtAddMaxHeartEvent()
    {
        maxHealth += 2;
    }
    
    private void AtMoveToPositionEvent(Vector3 targetPosition)
    {
        transform.position = targetPosition;
    }
    
    private void AtStartNewGameEvent()
    {
        canMove = true;
        maxHealth = 6;
        isDead = false;
        currentHealth = maxHealth;
        gameObject.layer = 7;
        gun.GetComponent<Gun>().isFiring = false;
        gun.transform.gameObject.SetActive(true);
    }
    #endregion
    
    private void PlayerMovementInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");
        movementInput = new Vector2(inputX, inputY);

        isMoving = movementInput != Vector2.zero;
        
        if (inputX != 0 && inputY != 0)
        {
            movementInput = new Vector2(inputX * 0.7f, inputY * 0.7f);
        }
    }

    private void Move()
    {
        rigidbody.MovePosition((Vector2)transform.position + movementInput * (speed * Time.deltaTime));
    }

    /// <summary>
    /// 无敌时间倒计时
    /// </summary>
    private void TimerCountDown()
    {
        timer += Time.deltaTime;
        if (timer - invincibleTime >= 0.01)
        {
            isInvincible = false;
            timer = 0;
        }
    }

    private void PlayerDie()
    {
        currentHealth = 0;
        isDead = true;
        canMove = false;
        gameObject.layer = 0;
        canMove = false;
        gun.transform.gameObject.SetActive(false);
        EventHandler.CallPlayerDieEvent();
    }

    private void SwitchAnimation()
    {
        anim.SetBool("IsMoving", isMoving);
        anim.SetBool("IsDead", isDead);
    }

    /// <summary>
    /// 翻转Sprite方向
    /// </summary>
    private void SpriteDirectionChecker()
    {
        if (inputX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (inputX > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private IEnumerator GetHurt()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
        spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        yield return new WaitForSeconds(invincibleTime - 0.15f);
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
}
