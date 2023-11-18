using System;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EventHandler.CallDamagePlayerEvent(damage);
        }
    }
}
