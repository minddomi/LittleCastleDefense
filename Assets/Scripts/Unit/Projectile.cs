using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 8f;
    public int damage = 10;

    private Transform target;
    private AllyUnit.UnitType ownerType;

    public void SetTarget(Transform enemy)
    {
        target = enemy;
    }

    public void Setdamage(int Damage)
    {
        damage = Damage;
    }

    public void SetOwnerType(AllyUnit.UnitType type)
    {
        ownerType = type;
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
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        EnemyUnit enemy = target.GetComponent<EnemyUnit>();
        if (enemy != null)
        {
            float multiplier = GetDamageMultiplier(ownerType, enemy.size);
            int finalDamage = Mathf.RoundToInt(damage * multiplier);
            enemy.TakeDamage(finalDamage);
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
