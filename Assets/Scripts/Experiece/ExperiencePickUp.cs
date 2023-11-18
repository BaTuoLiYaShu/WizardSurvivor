using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ExperiencePickUp : MonoBehaviour
{
    public int expValue;
    public int speed;

    public ObjectPool<GameObject> expPool;

    private bool isMoving;
    private Player player;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        GoToPlayer();
    }

    private void OnEnable()
    {
        isMoving = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventHandler.CallPlayerGetExp(expValue);
            
            expPool.Release(gameObject);
        }
    }

    private void GoToPlayer()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else
        {
            if (!(Vector3.Distance(transform.position, player.transform.position) <= player.pickRange))
            {
                isMoving = false;
                return;
            }
            isMoving = true;
            speed += player.speed + 8;
        }
    }
}
