using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyWave
{
    public float waveTime;
    public float timeBetweenSpawn;
    public List<GameObject> enemyToSpawn;
}
