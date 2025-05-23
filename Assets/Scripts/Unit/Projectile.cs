using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;

    private Transform target;
    private AllyUnit.UnitType ownerType;
    private float ownerAttackPower;

    public void SetTarget(Transform enemy, AllyUnit.UnitType unitType, float attackPower)
    {
        target = enemy;
        ownerType = unitType;
        ownerAttackPower = attackPower;
    }

    void Update()
    {
        if (target == null) { Destroy(gameObject); return; }
        Vector3 dir = target.position - transform.position;
        transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
        if (dir.magnitude < 0.2f) HitTarget();
    }

    void HitTarget()
    {
        EnemyUnit enemy = target.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            float multiplier = GetDamageMultiplier(ownerType, enemy.size);
            float damage = ownerAttackPower * multiplier;
            enemy.TakeDamage(damage);
        }
        Destroy(gameObject);
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

