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
    public float damage = 10f;
    public float attackSpeed = 1f; // �ʴ� ���� Ƚ��
    public float attackRadius = 5f; // ���� �ݰ�

    private float attackCooldown = 0f; // ���� ��ٿ�

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown -= Time.deltaTime;

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                if (attackCooldown <= 0f)
                {
                    Attack(hitCollider.gameObject);
                    attackCooldown = 1f / attackSpeed;
                }
                break; // ���� ����� �� �ϳ��� ����
            }
        }
    }

    void Attack(GameObject enemy)
    {
        // �� ���ֿ��� �������� �ִ� ����
        EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
        if (enemyUnit != null)
        {
            enemyUnit.TakeDamage(damage);
            Debug.Log($"{unitType} attacked {enemy.name} for {damage} damage.");
        }
    }
}
