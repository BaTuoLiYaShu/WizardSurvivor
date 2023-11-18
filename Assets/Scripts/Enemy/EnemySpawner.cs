using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    private float currentWaveTime;
    private float currentWaveSpawnCounter;
    private Transform player;

    private bool isPlayerDead;
    
    public Transform minSpawnPoint;
    public Transform maxSpawnPoint;

    public List<EnemyWave> totalWave;
    private int currentWave;
    
    private Vector3 spawnPos;
    
    private readonly List<ObjectPool<GameObject>> enemyPoolList = new List<ObjectPool<GameObject>>();
    private Transform enemyParent;

    private void Awake()
    {
        enemyParent = GameObject.FindWithTag("Item Parent").transform;
    }

    private void OnEnable()
    {
        EventHandler.PlayerDieEvent += AtPlayerDieEvent;
        EventHandler.AfterSceneLoadedEvent += AtAfterSceneLoadedEvent;
    }

    private void OnDisable()
    {
        EventHandler.PlayerDieEvent -= AtPlayerDieEvent;
        EventHandler.AfterSceneLoadedEvent -= AtAfterSceneLoadedEvent;
    }

    private void AtPlayerDieEvent()
    {
        isPlayerDead = true;
    }
    
    private void AtAfterSceneLoadedEvent()
    {
        player = GameObject.FindWithTag("Player").transform;
        minSpawnPoint = player.GetChild(0);
        maxSpawnPoint = player.GetChild(1);
        
        isPlayerDead = false;
        currentWave = 0;
        currentWaveTime = totalWave[currentWave].waveTime;
        currentWaveSpawnCounter = totalWave[currentWave].timeBetweenSpawn;
        CreatePool(totalWave[currentWave].enemyToSpawn);
    }

    private void Update()
    {
        if (!isPlayerDead)
        {
            currentWaveTime -= Time.deltaTime;
            if (currentWaveTime > 0)
            {
                currentWaveSpawnCounter -= Time.deltaTime;
                if (currentWaveSpawnCounter <= 0)
                {
                    SpawnEnemy();
                    currentWaveSpawnCounter = totalWave[currentWave].timeBetweenSpawn;
                }
            }
            else
            {
                GoToNextWave();
            }
        }
    }
    
    private void SpawnEnemy()
    {
        var objPool = enemyPoolList[Random.Range(0, totalWave[currentWave].enemyToSpawn.Count)];
        var obj = objPool.Get();
        
        obj.GetComponent<Enemy>().enemyPool = objPool; //给生成的敌人的pool赋值
        obj.layer = 6;
        
        obj.transform.position = GetSpawnPos();
    }

    private void GoToNextWave()
    {
        currentWave++;

        currentWaveTime = totalWave[currentWave].waveTime;
        currentWaveSpawnCounter = totalWave[currentWave].timeBetweenSpawn;
        
        CreatePool(totalWave[currentWave].enemyToSpawn);
    }

    private void CreatePool(List<GameObject> enemyToSpawn)
    {
        enemyPoolList.Clear();
        foreach (var enemy in enemyToSpawn)
        {
            var newPool = new ObjectPool<GameObject>
            (
                () => Instantiate(enemy, enemyParent),
                e => { e.SetActive(true); },
                e => { e.SetActive(false); },
                Destroy,
                true,
                20
            );
            
            enemyPoolList.Add(newPool);
        }
    }
    
    private Vector3 GetSpawnPos()
    {
        Vector3 spawnPoint = Vector3.zero;
        
        bool spawnVerticalEdge = Random.Range(0f, 1f) > 0.5f; //敌人是否生成在垂直轴上

        if (spawnVerticalEdge)
        {
            spawnPoint.y = Random.Range(minSpawnPoint.position.y, maxSpawnPoint.position.y);

            if (Random.Range(0, 1) > 0.5f) //生成在右边
            {
                spawnPoint.x = maxSpawnPoint.position.x;
            }
            else //生成在左边
            {
                spawnPoint.x = minSpawnPoint.position.x;
            }
        }
        else
        {
            spawnPoint.x = Random.Range(minSpawnPoint.position.x, maxSpawnPoint.position.x);

            if (Random.Range(0, 1) > 0.5f) //生成在上边
            {
                spawnPoint.y = maxSpawnPoint.position.y;
            }
            else //生成在下边
            {
                spawnPoint.y = minSpawnPoint.position.y;
            }
        }

        return spawnPoint;
    }
    

}
