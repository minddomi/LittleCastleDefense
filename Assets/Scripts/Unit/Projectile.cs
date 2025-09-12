using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;

    private Transform target;
    private AllyUnit.UnitType ownerType;
    private AllyUnit.UnitGrade ownerGrade;
    private float ownerAttackPower;

    private float critChance;
    private float critMultiplier;

    private int maxBounces = 5; // 튕김 제한
    private int currentBounce = 0;
    private HashSet<EnemyUnit> hitEnemies = new HashSet<EnemyUnit>();

    public void SetTarget(Transform enemy, AllyUnit.UnitType unitType, float attackPower, float critChance, float critMultiplier)
    {
        target = enemy;
        ownerType = unitType;
        ownerAttackPower = attackPower;
        this.critChance = critChance;
        this.critMultiplier = critMultiplier;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

        if (dir.magnitude < 0.2f)
            HitTarget();
    }

    void HitTarget()
    {
        EnemyUnit enemy = target.GetComponent<EnemyUnit>();

        if (enemy != null)
        {
            // 최고 유닛일 경우: 중복 제거
            if (ownerGrade == AllyUnit.UnitGrade.Supreme)
            {
                if (hitEnemies.Contains(enemy))
                {
                    Destroy(gameObject); // 이미 맞은 적이면 종료
                    return;
                }

                hitEnemies.Add(enemy);
            }

            float multiplier = GetDamageMultiplier(ownerType, enemy.size);
            float damage = ownerAttackPower * multiplier;

            bool isCritical = Random.value < critChance;
            if (isCritical)
            {
                damage *= critMultiplier;
                Debug.Log($"CRITICAL HIT! {enemy.name} took {damage}");
            }

            enemy.TakeDamage(damage);

            if (ownerGrade == AllyUnit.UnitGrade.Supreme && currentBounce < maxBounces)
            {
                currentBounce++;
                Transform nextTarget = FindNextEnemy(enemy);
                if (nextTarget != null)
                {
                    target = nextTarget;
                    return;
                }
            }
        }

        Destroy(gameObject);
    }


    Transform FindNextEnemy(EnemyUnit current)
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float minDist = Mathf.Infinity;
        Transform next = null;

        foreach (GameObject go in enemies)
        {
            EnemyUnit e = go.GetComponent<EnemyUnit>();
            if (e != null && go.transform != current.transform)
            {
                if (hitEnemies.Contains(e)) continue; // 최고 유닛: 중복 제외

                float dist = Vector3.Distance(current.transform.position, go.transform.position);
                if (dist < 5f && dist < minDist)
                {
                    minDist = dist;
                    next = go.transform;
                }
            }
        }

        return next;
    }


    float GetDamageMultiplier(AllyUnit.UnitType type, EnemySize size)
    {
        switch (type)
        {
            case AllyUnit.UnitType.Archer:
                if (size == EnemySize.Small) return 1.0f;
                if (size == EnemySize.Medium) return 0.5f;
                return 0.25f;
            case AllyUnit.UnitType.Siege:
                if (size == EnemySize.Large) return 1.0f;
                if (size == EnemySize.Medium) return 0.5f;
                return 0.25f;
            case AllyUnit.UnitType.Wizard:
                return 0.75f;
        }
        return 1.0f;
    }
}

