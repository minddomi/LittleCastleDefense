using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] waypoints;
    public Transform spawnPoint;

    [Header("소환된 적들을 정리할 컨테이너")]
    public Transform enemiesContainer;     // ← 여기에 빈 GameObject를 연결하세요

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
        // 부모 Transform 결정
        Transform parent = enemiesContainer != null ? enemiesContainer : transform;
        // transform으로 하면 Spawner 자신의 자식이 됨 (선택사항)

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity, parent);
        EnemyUnit unit = enemy.GetComponent<EnemyUnit>();
        unit.waypoints = waypoints;

        Debug.Log(parent.name);
    }
}

