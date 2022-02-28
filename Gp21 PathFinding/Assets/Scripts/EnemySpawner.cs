using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTimer = 5f;

    private void Start()
    {
        //StartSpawn();
    }

    private void OnEnable()
    {
        EventManager.onMyEvent += StartSpawn;
    }

    private void OnDisable()
    {
        EventManager.onMyEvent -= StartSpawn;
        StopSpawn();
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    void StartSpawn()
    {
        InvokeRepeating("SpawnEnemy", spawnTimer, spawnTimer);
    }
    void StopSpawn()
    {
        CancelInvoke();
    }
}
