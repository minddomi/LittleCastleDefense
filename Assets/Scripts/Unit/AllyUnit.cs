using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AllyUnit : MonoBehaviour
{
    private UnitStatus unitStatus;

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
        Transcendence,
        Supreme
    }

    public UnitType unitType;
    public UnitGrade unitGrade;

    public float attackRange; // 공격 범위 int
    public float attackInterval; // 공격 쿨다운 float
    public float attackPower; // 공격력 float
    public float upgradeLevel; // 업그레이드 레벨 float
    public float upgradePower; // 업그레이드 공격력 float

    public float TotalAttackPower;

    public float criticalChance;         // 0~1 범위 (예: 0.25f = 25%) float
    public float criticalMultiplier;     // 치명타 배율 (예: 3f = 3배) float

    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject aoeEffectPrefab; // 초월 유닛용 범위 이펙트    

    private float attackTimer = 0f;

    private Dictionary<BufferUnit, BufferUnit.BuffData> activeBuffs = new();

    public event Action OnAttackFired;// 발사 후 알림

    private bool isFrozen = false; // 유닛 정지상태
    private float freezeTimer = 0f;

    public bool ignoreTypeForDamage = false; // 평등의 저울
    public bool blockBufferBuffs = false; // 부서진 성배
    public bool targetRandom = false; // 혼돈의 수정

    // Start is called before the first frame update
    void Start()
    {
        unitStatus = GetComponent<UnitStatus>(); // 아이템 관련 추가

        unitType = unitStatus.unitClass switch
        {
            UnitClass.Archer => UnitType.Archer,
            UnitClass.Mage => UnitType.Wizard,
            UnitClass.Siege => UnitType.Siege,
            UnitClass.Buffer => UnitType.Buffer,
            UnitClass.Joker => UnitType.Joker,
            _ => UnitType.Archer
        };
        unitGrade = (UnitGrade)unitStatus.unitGrade;
        unitGrade = (UnitGrade)unitStatus.unitGrade;
        attackRange = unitStatus.attackRange;
        attackInterval = unitStatus.attackInterval;
        attackPower = unitStatus.attackPower;
        upgradeLevel = unitStatus.upgradeLevel;
        upgradePower = unitStatus.upgradePower;
        TotalAttackPower = unitStatus.TotalAttackPower;
        criticalChance = unitStatus.criticalChance;
        criticalMultiplier = unitStatus.criticalMultiplier;
    }

    // Update is called once per frame
    void Update()
    {
        attackPower = unitStatus.attackPower;

        if (ItemEffectsManager.Instance != null) // 아이템 관련 추가
        {
            ItemEffectsManager.Instance.Sync(this, unitStatus);
        }

        if (isFrozen) // 유닛 정지상태
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f) { isFrozen = false; }
            return;
        }

        attackTimer += Time.deltaTime;
        TotalAttackPower = attackPower + upgradePower;

        unitStatus.TotalAttackPower = TotalAttackPower; // UnitStatus와 동기화

        if (attackTimer >= attackInterval)
        {
            if (unitType != UnitType.Buffer)    // BufferUnit은 별도 스크립트에서 처리
            {
                GameObject target = FindClosestEnemyInRange();
                if (target != null)
                    Shoot(target.transform);
                    OnAttackFired?.Invoke();   //발사 알림 (광기의 마법서)
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

        projectile.ownerUnit = this; // 탄의 주인 설정 (굶줄인 검)

        projectile.ignoreType = ignoreTypeForDamage; // 아이템 평등의 저울 관련 추가

        projectile.targetRandomAll = this.targetRandom; // 랜덤 타겟/ 무한 사거리 적용 (혼돈의 수정)
    }

    private readonly Dictionary<object, BufferUnit.BuffData> _buffs = new();

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
            if (blockBufferBuffs) return; // 부서진 성배 관련 추가

            BufferUnit.BuffData data = activeBuffs[buffer];
            upgradePower -= attackPower * (data.powerMultiplier - 1f);
            attackInterval /= data.attackSpeedMultiplier;
            criticalChance -= data.critChanceBonus;
            activeBuffs.Remove(buffer);
        }
    }

    public IEnumerable<BufferUnit.BuffData> GetActiveBuffs() => _buffs.Values;

    void OnMouseDown()
    {
        //// 조커 유닛만 클릭 시 삭제
        //if (unitType == UnitType.Joker)
        //{ 
        //    Debug.Log("Joker Unit clicked and removed!");
        //    Destroy(gameObject);
        //}
    }

    public void FreezeFor(float seconds) // 유닛 정지 (광기의 마법서)
    {
        isFrozen = true;
        // 중첩 시 더 긴 시간 우선
        freezeTimer = Mathf.Max(freezeTimer, seconds);
    }

    public void SetBlockBufferBuffs(bool block) // 버프 차단 (부서진 성배)
    {
        blockBufferBuffs = block;
        if (block) ClearActiveBufferBuffs();
    }

    public void ClearActiveBufferBuffs() // 현재 적용중인 버프 제거 (부서진 성배)
    {
        var list = new List<BufferUnit>(activeBuffs.Keys);
        foreach (var b in list) RemoveBuffFrom(b);
    }
}
