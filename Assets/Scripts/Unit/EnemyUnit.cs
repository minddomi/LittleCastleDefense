using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : MonoBehaviour
{
    public float health = 100f; // 체력
    public float speed = 3.5f; // 이동속도
    public Transform[] waypoints;

    private int currentWaypointIndex = 0;


    // Start is called before the first frame update
    void Start()
    {
      
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
            {
                currentWaypointIndex = 0; // 반복 시작
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died.");
        Destroy(gameObject);
    }
}
