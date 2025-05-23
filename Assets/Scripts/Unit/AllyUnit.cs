using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    public enum UnitType
    {
        Archer = 0,
        Wizard = 1,
        Siege = 2
    }

    public UnitType unitType;
    public float attackRange = 5f; // 공격 범위
    public float attackInterval = 1.5f; // 공격 쿨다운
    public float attackPower = 50f; // 공격력
    public GameObject projectilePrefab;
    public Transform firePoint;

    private float attackTimer = 0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer >= attackInterval)
        {
            GameObject target = FindClosestEnemyInRange();
            if (target != null)
            {
                Shoot(target.transform);
                attackTimer = 0f;
            }
        }
    }

    GameObject FindClosestEnemyInRange()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < attackRange && dist < minDist)
            {
                minDist = dist;
                closest = enemy;
            }
        }

        return closest;
    }

    void Shoot(Transform target)
    {
        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();
        projectile.SetTarget(target, unitType, attackPower);
    }
}
