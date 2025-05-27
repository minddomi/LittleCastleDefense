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
    public float attackRange = 5f;
    public float attackInterval = 1.5f;
    public int baseDamage = 1;

    // 업그레이드 관련
    public int upgradeLevel = 0;
    public int upgradeDamage = 0;

    public GameObject projectilePrefab;
    public Transform firePoint;

    private float attackTimer = 0f;

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
        int finalDamage = baseDamage + upgradeLevel * upgradeDamage;

        GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        Projectile projectile = proj.GetComponent<Projectile>();

        projectile.SetTarget(target);
        projectile.SetDamage(finalDamage);
    }

    // 레벨 업 함수
    public void IncreaseLevel(int amount = 1)
    {
        upgradeLevel += amount;
    }
}
