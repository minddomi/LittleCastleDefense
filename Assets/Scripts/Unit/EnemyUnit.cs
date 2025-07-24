using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemySize { Small, Medium, Large }

public class EnemyUnit : MonoBehaviour
{
    public EnemySize size = EnemySize.Medium; // 초기 적 유닛 유형 설정
    public float health = 100f; // 체력
    public float speed = 3.5f; // 이동속도
    public Transform[] waypoints;
    public GameObject healthBarPrefab;        
    private EnemyHealthBar healthBarInstance;

    private int currentWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
            hb.transform.SetParent(GameObject.Find("Canvas").transform, false); // UI 캔버스에 붙이기

            healthBarInstance = hb.GetComponent<EnemyHealthBar>();
            healthBarInstance.enemyTransform = this.transform;
            healthBarInstance.UpdateHealth(health, health); // 초기 체력 표시
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 dir = (target.position - transform.position).normalized;

        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealth(health, 100f);

        if (health <= 0f) Die();
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");

        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject); // 체력바도 제거

        Destroy(gameObject);
    }
}
