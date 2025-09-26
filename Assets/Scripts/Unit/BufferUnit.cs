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
        { AllyUnit.UnitGrade.Basic,        new BuffData { range = 1.5f, powerMultiplier = 1.1f, attackSpeedMultiplier = 1.0f, critChanceBonus = 0.00f } },
        { AllyUnit.UnitGrade.Transcendence,new BuffData { range = 12.0f, powerMultiplier = 2.2f, attackSpeedMultiplier = 0.65f, critChanceBonus = 0.30f } }
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
        AllyUnit[] allies = FindObjectsOfType<AllyUnit>();

        foreach (AllyUnit ally in allies)
        {
            if (ally == self) continue;                               // 자신 제외
            if (ally.unitType == AllyUnit.UnitType.Buffer) continue; // 다른 버퍼 제외

            float dist = Vector3.Distance(transform.position, ally.transform.position);
            if (dist <= data.range)
            {
                ally.RemoveBuffFrom(this);
                ally.AddBuffFrom(this, data);

                Debug.Log($"[Buffer] {self.name} → {ally.name}: " +
                          $"공격력 ×{data.powerMultiplier}, " +
                          $"공속 ×{data.attackSpeedMultiplier}, " +
                          $"치명타 +{data.critChanceBonus * 100f}% (거리: {dist:F2})");
            }
            else
            {
                ally.RemoveBuffFrom(this);
                Debug.Log($"[Buffer] {ally.name}은(는) {self.name} 범위({data.range}) 밖 (거리: {dist:F2}) → 버프 제거");
            }
        }
    }

    public struct BuffData
    {
        public float range;
        public float powerMultiplier;
        public float attackSpeedMultiplier;
        public float critChanceBonus;
    }
}

