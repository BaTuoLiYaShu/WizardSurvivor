using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackRadius : MonoBehaviour
{
    public float radius;
    
    private Collider2D nearestEnemy;
    private Collider2D[] enemiesInTrigger;
    
    public Collider2D GetNearestEnemy()
    {
        enemiesInTrigger = new Collider2D[50];
        Collider2D currentNearestEnemy = null;
        Vector3 currentPosition = transform.position;
        float minSqrDistance = Mathf.Infinity;
        
        Physics2D.OverlapCircleNonAlloc(currentPosition, radius, enemiesInTrigger, 1 << LayerMask.NameToLayer("Enemy"));

        if (enemiesInTrigger[0] == null)
        {
            return null;
        }
            
        foreach (var enemy in enemiesInTrigger)
        {
            if (enemy == null)
            {
                break;
            }
            float sqrDistanceToPlayer = (currentPosition - enemy.transform.position).sqrMagnitude;
            
            if (sqrDistanceToPlayer < minSqrDistance)
            {
                minSqrDistance = sqrDistanceToPlayer;
                currentNearestEnemy = enemy;
            }
        }

        return currentNearestEnemy;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
