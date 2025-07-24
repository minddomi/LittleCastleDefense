using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    public enum UnitType
    {
        Archer = 0,
        Wizard = 1,
        Siege = 2,
        Supreme = 3,       // 최고 등급 유닛
        Transcendence = 4  // 초월 등급 유닛
    }

    public UnitType unitType;
    public float attackRange = 5f; // 공격 범위
    public float attackInterval = 1.5f; // 공격 쿨다운
    public float attackPower = 50f; // 공격력
    public float upgradeLevel = 0.0f; // 업그레이드 레벨
    public float upgradePower = 0.0f; // 업그레이드 공격력

    public GameObject projectilePrefab;
    public Transform firePoint;

    public GameObject aoeEffectPrefab; // 초월 유닛용 범위 이펙트

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
            if (unitType == UnitType.Transcendence)
            {
                PerformAoEAttack();
            }
            else
            {
                GameObject target = FindClosestEnemyInRange();
                if (target != null)
                    Shoot(target.transform);
            }

            attackTimer = 0f;
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

        float totalPower = attackPower + upgradePower; // 총 데미지 계산
        projectile.SetTarget(target, unitType, totalPower); //
    }

    void PerformAoEAttack()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float totalPower = attackPower + upgradePower;

        foreach (GameObject enemyObj in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemyObj.transform.position);
            if (dist <= attackRange)
            {
                EnemyUnit enemy = enemyObj.GetComponent<EnemyUnit>();
                if (enemy != null)
                {
                    enemy.TakeDamage(totalPower);
                }
            }
        }

        if (aoeEffectPrefab != null)
        {
            GameObject fx = Instantiate(aoeEffectPrefab, transform.position, Quaternion.identity);
            Destroy(fx, 1.5f);
        }
    }

}
