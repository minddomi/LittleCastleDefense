using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;

    private Transform target;
    private AllyUnit.UnitType ownerType;
    private float ownerAttackPower;

    private int maxBounces = 5; // ƨ�� ����
    private int currentBounce = 0;
    private HashSet<EnemyUnit> hitEnemies = new HashSet<EnemyUnit>();

    public void SetTarget(Transform enemy, AllyUnit.UnitType unitType, float attackPower)
    {
        target = enemy;
        ownerType = unitType;
        ownerAttackPower = attackPower;
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
            // �ְ� ������ ���: �ߺ� ����
            if (ownerType == AllyUnit.UnitType.Supreme)
            {
                if (hitEnemies.Contains(enemy))
                {
                    Destroy(gameObject); // �̹� ���� ���̸� ����
                    return;
                }

                hitEnemies.Add(enemy);
            }

            float multiplier = GetDamageMultiplier(ownerType, enemy.size);
            float damage = ownerAttackPower * multiplier;
            enemy.TakeDamage(damage);

            if (ownerType == AllyUnit.UnitType.Supreme && currentBounce < maxBounces)
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
                if (hitEnemies.Contains(e)) continue; // �ְ� ����: �ߺ� ����

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

