using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

[RequireComponent(typeof(BoxCollider2D), typeof(Animator))]
public class Enemy : MonoBehaviour
{
    public float speed;
    public bool canMove;
    private float defaultSpeed;
    
    public int health;
    private int defaultHealth;
    private Vector3 defaultLocalScale;
    public int damage;
    public int hurtForce;

    public ObjectPool<GameObject> enemyPool;
    
    private bool isDead;
    
    protected Transform player;
    protected Animator anim;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        
        defaultSpeed = speed;
        defaultHealth = health;
        canMove = true;
    }

    protected virtual void Update()
    {   
        FlipEnemy();
    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void OnEnable()
    {
        EventHandler.PlayerDieEvent += AtPlayerDieEvent;
        spriteRenderer.color = new Color(1, 1, 1, 1);
        ResetEnemySpeedAndHealth();
    }

    private void OnDisable()
    {
        EventHandler.PlayerDieEvent -= AtPlayerDieEvent;
    }

    #region 事件
    private void AtPlayerDieEvent()
    {
        speed = 0;
        anim.SetBool("IsIdle", true);
    }
    #endregion

    protected virtual void FlipEnemy()
    {
        spriteRenderer.flipX = player.position.x > transform.position.x;
    }
    
    /// <summary>
    /// 攻击玩家
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventHandler.CallDamagePlayerEvent(damage);
        }
    }
    
    public void EnemyGetHurt(int damage)
    {
        health -= damage;
        StartCoroutine(EnemyGetHurt());
        if (health <= 0)
        {
            health = 0;
            EnemyDie();
        }
    }
    
    private IEnumerator EnemyGetHurt()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        
        var direction = new Vector3(transform.position.x - player.position.x, transform.position.y - player.position.y).normalized;
        rb.AddForce(direction * hurtForce, ForceMode2D.Impulse);
        
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }

    private void EnemyDie()
    {
        isDead = true;
        anim.SetBool("IsDead", isDead);
        gameObject.layer = 0;
        speed = 0;
    }

    private void DestroyAfterAnimation()
    {
        spriteRenderer.material.SetFloat("_FlashAmount", 1);
        StartCoroutine(ResetAnimator());
        spriteRenderer.color = new Color(1, 1, 1, 0);
        EventHandler.CallEnemyDropExp(transform.position);
        enemyPool.Release(gameObject);
    }

    private IEnumerator ResetAnimator()
    {
        anim.Rebind();
        yield return new WaitForSeconds(0.5f);
    }

    public void ResetEnemySpeedAndHealth()
    {
        speed = defaultSpeed;
        health = defaultHealth;
        spriteRenderer.material.SetFloat("_FlashAmount", 0);
    }

    /// <summary>
    /// 跟随玩家
    /// </summary>
    private void FollowPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) > Mathf.Epsilon && canMove)
        {
            Vector3 pos = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            transform.position = pos;
        }
    }
}
