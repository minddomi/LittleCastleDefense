 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AllyUnit))]
public class BufferUnit : MonoBehaviour
{
    private AllyUnit self;
    private float interval = 1.5f;
    private float timer = 0f;

    private readonly Dictionary<AllyUnit.UnitGrade, BuffData> buffTable = new()
    {
        { AllyUnit.UnitGrade.Basic,        new BuffData { range = 1.5f, powerMultiplier = 1.1f, attackSpeedMultiplier = 1.0f, critChanceBonus = 0.00f, resourceBonus = 0 } },
        { AllyUnit.UnitGrade.Intermediate, new BuffData { range = 1.5f, powerMultiplier = 1.0f, attackSpeedMultiplier = 1.0f, critChanceBonus = 0.00f, resourceBonus = 0 } },
        { AllyUnit.UnitGrade.Advanced,     new BuffData { range = 3.0f, powerMultiplier = 1.0f, attackSpeedMultiplier = 1.0f, critChanceBonus = 0.00f, resourceBonus = 0 } },
        { AllyUnit.UnitGrade.Epic,         new BuffData { range = 5.0f, powerMultiplier = 1.0f, attackSpeedMultiplier = 1.0f, critChanceBonus = 0.00f, resourceBonus = 0 } },
        { AllyUnit.UnitGrade.Transcendence,new BuffData { range = 12.0f, powerMultiplier = 2.2f, attackSpeedMultiplier = 0.65f, critChanceBonus = 0.30f, resourceBonus = 0 } }
    };

   // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<AllyUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            Debug.Log($"[Buffer] {self.name} (등급: {self.unitGrade}) → ApplyBuff 실행");
            ApplyBuff();
            timer = 0f;
        }
    }

    void ApplyBuff()
    {
        BuffData data = buffTable[self.unitGrade];

        // 등급별로 “자신의 공격력” 기반 추가 효과 계산
        switch (self.unitGrade)
        {
            case AllyUnit.UnitGrade.Intermediate:
                // 자신의 공격력만큼 주변 아군 공격속도 증가
                // 공격력이 1 → +0.1초 빠르게 → 약 +10% 가속 정도로 환산
                data.attackSpeedMultiplier = 1.5f * (self.attackPower * 0.01f);
                break;

            case AllyUnit.UnitGrade.Advanced:
                // 자신의 공격력만큼 주변 아군 치명타 확률 증가
                // 공격력이 1 → +1%
                data.critChanceBonus = 0.01f * self.attackPower;
                break;

            case AllyUnit.UnitGrade.Epic:
                // 자신의 공격력만큼 막타시 획득 자원 증가
                // 공격력이 1 → +1 자원
                data.resourceBonus = Mathf.FloorToInt(self.attackPower);
                break;
        }

        // 범위 내 아군에게 버프 적용
        AllyUnit[] allies = FindObjectsOfType<AllyUnit>();
        foreach (AllyUnit ally in allies)
        {
            if (ally == self) continue;  // 자신 제외
            if (ally.unitType == AllyUnit.UnitType.Buffer) continue; // 다른 버퍼 제외

            if (ally.blockBufferBuffs)// 버프 차단 (부서진 성배)
            {
                ally.RemoveBuffFrom(this);
                Debug.Log($"[Buffer] {ally.name} 버프 차단중 → 스킵");
                continue;
            }

            float dist = Vector3.Distance(transform.position, ally.transform.position);
            if (dist <= data.range)
            {
                ally.RemoveBuffFrom(this);
                ally.AddBuffFrom(this, data);

                Debug.Log($"[Buffer] {self.name} ({self.unitGrade}) → {ally.name} | " +
                          $"공격력×{data.powerMultiplier:F2}, " +
                          $"공속×{data.attackSpeedMultiplier:F2}, " +
                          $"치확+{data.critChanceBonus * 100f:F1}%, " +
                          $"자원보너스+{data.resourceBonus} (거리 {dist:F1})");
            }
            else
            {
                ally.RemoveBuffFrom(this);
            }
        }
    }

    public struct BuffData
    {
        public float range;
        public float powerMultiplier;
        public float attackSpeedMultiplier;
        public float critChanceBonus;
        public int resourceBonus;
    }
}

