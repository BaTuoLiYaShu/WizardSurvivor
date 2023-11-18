using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class ExperienceSpawner : MonoBehaviour
{
    public GameObject expBallPrefab;
    private ObjectPool<GameObject> experiencePool;

    private Transform experienceParent;
    
    private void Awake()
    {
        experienceParent = GameObject.FindWithTag("Experience Parent").transform;
    }

    private void Start()
    {
        experiencePool = new ObjectPool<GameObject>
        (
            () => Instantiate(expBallPrefab, experienceParent),
            e => { e.SetActive(true); },
            e => { e.SetActive(false); },
            Destroy,
            true,
            100
        );
    }

    private void OnEnable()
    {
        EventHandler.EnemyDropExp += AtEnemyDropExp;
    }

    private void OnDisable()
    { 
        EventHandler.EnemyDropExp -= AtEnemyDropExp;
    }

    private void AtEnemyDropExp(Vector3 pos)
    {
        var randomNum = Random.Range(1, 4);

        for (int i = 0; i < randomNum; i++)
        {
            var direction = Random.insideUnitCircle.normalized;
            var obj = experiencePool.Get();
            obj.transform.position = pos;
            obj.GetComponent<ExperiencePickUp>().expPool = experiencePool;
            obj.GetComponent<Rigidbody2D>().AddForce(direction * 10, ForceMode2D.Impulse);
        }
    }
}
