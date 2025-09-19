using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : MonoBehaviour
{
    public enum UnitType  // 유닛 종족
    {
        Archer = 0, 
        Wizard = 1,
        Siege = 2,
        Buffer = 3,
        Joker = 4
    }

    public enum UnitGrade  // 유닛 등급
    {
        Basic,
        Intermediate,
        Advanced,
        Epic,
        Supreme,
        Transcendence
    }

    public UnitType unitType;
    public UnitGrade unitGrade = UnitGrade.Basic;

    public float attackRange = 5f; // 공격 범위
    public float attackInterval = 1.5f; // 공격 쿨다운
    public float attackPower = 50f; // 공격력
    public float upgradeLevel = 0.0f; // 업그레이드 레벨
    public float upgradePower = 0.0f; // 업그레이드 공격력

    public float TotalAttackPower;

    public float criticalChance = 0f;         // 0~1 범위 (예: 0.25f = 25%)
    public float criticalMultiplier = 3f;     // 치명타 배율 (예: 3f = 3배)

    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject aoeEffectPrefab; // 초월 유닛용 범위 이펙트    

    private float attackTimer = 0f;

    private Dictionary<BufferUnit, BufferUnit.BuffData> activeBuffs = new();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;
        TotalAttackPower = attackPower + upgradePower;

        if (attackTimer >= attackInterval)
        {
            if (unitType != UnitType.Buffer)    // BufferUnit은 별도 스크립트에서 처리
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

        float totalPower = attackPower + upgradePower;          // 총 데미지 계산
        projectile.SetTarget(target, unitType, totalPower, criticalChance, criticalMultiplier);
    }

    // 버프 처리
    public void AddBuffFrom(BufferUnit buffer, BufferUnit.BuffData data)
    {
        if (!activeBuffs.ContainsKey(buffer))
        {
            upgradePower += attackPower * (data.powerMultiplier - 1f);
            attackInterval *= data.attackSpeedMultiplier;
            criticalChance += data.critChanceBonus;
            activeBuffs[buffer] = data;
        }
    }

    public void RemoveBuffFrom(BufferUnit buffer)
    {
        if (activeBuffs.ContainsKey(buffer))
        {
            BufferUnit.BuffData data = activeBuffs[buffer];
            upgradePower -= attackPower * (data.powerMultiplier - 1f);
            attackInterval /= data.attackSpeedMultiplier;
            criticalChance -= data.critChanceBonus;
            activeBuffs.Remove(buffer);
        }
    }
    void OnMouseDown()
    {
        // 조커 유닛만 클릭 시 삭제
        if (unitType == UnitType.Joker)
        {
            Debug.Log("Joker Unit clicked and removed!");
            Destroy(gameObject);
        }
    }
}
