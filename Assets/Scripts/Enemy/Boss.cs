using System;
using UnityEngine;
using UnityEngine.Serialization;

public class Boss : Enemy
{
    public string bossName;
    
    private bool isAngry;
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }

    private void Start()
    {
        EventHandler.CallShowBossNameEvent(bossName);
    }

    protected override void Update()
    {
        base.Update();
        CheckDistanceOfPlayer();
        CheckBossState();
        FlipEnemy();
    }

    private void CheckDistanceOfPlayer()
    {
        var animStateInfo = anim.GetCurrentAnimatorStateInfo(0);
        if (!(Mathf.Abs(transform.position.x - player.position.x) <= 8) ||
            !(Mathf.Abs(transform.position.y - player.position.y) >= 2) || 
            !(Mathf.Abs(transform.position.y - player.position.y) <= 3))
        {
            return;
        }
        if (!animStateInfo.IsName("BossCleave") && !animStateInfo.IsName("BossSuperAttack"))
        {
            anim.SetTrigger(!isAngry ? "Cleave" : "SuperCleave");
        }
    }

    private void CheckBossState()
    {
        if (health <= 150 && !isAngry)
        {
            isAngry = true;
            anim.SetTrigger("BecomeAngry");
            enemy.speed += 2;
        }
    }

    protected override void FlipEnemy()
    {
        transform.localScale = player.position.x > transform.position.x ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
    }
}
