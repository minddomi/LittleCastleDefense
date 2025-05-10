using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public Transform spawnPoint;

    public float spawnInterval = 2f;
    private float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyUnit unit = enemy.GetComponent<EnemyUnit>();
        unit.waypoints = waypoints;
    }
}

