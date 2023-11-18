using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public int damage;
    public int vampirePercentage;

    private void OnEnable()
    {
        Destroy(gameObject, 5f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Enemy"))
        {
            return;
        }
        Destroy(gameObject);
        other.GetComponent<Enemy>().EnemyGetHurt(damage);
        
        if (Random.Range(0, 101) < vampirePercentage)
        {
            EventHandler.CallRestorePlayerHealthEvent();
        }
    }
}
