using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaosCrystalEffect : IItemEffect
{
    public string Id { get; }

    private readonly Dictionary<AllyUnit, float> savedRange = new();
    private readonly Dictionary<AllyUnit, float> atkBonus = new();
    private readonly HashSet<AllyUnit> applied = new();

    public ChaosCrystalEffect(string id) { Id = id; }

    public void Apply(AllyUnit unit)
    {
        if (unit == null || applied.Contains(unit)) return;
        applied.Add(unit);

        var status = unit.GetComponent<UnitStatus>();
        if (status == null) return;

        //공격력 +20% (프리팹 기본 공격력 기준)
        float baseAtk = status.attackPower;
        float bonus = baseAtk * 0.20f;
        atkBonus[unit] = bonus;
        unit.upgradePower += bonus;

        //랜덤 타겟팅 + 사거리 무제한
        savedRange[unit] = unit.attackRange;
        unit.attackRange = 999999f;

        unit.targetRandom = true;

        Debug.Log($"[ChaosCrystal] {unit.name}: +20% ATK, random targeting ON, infinite range");
    }

    public void Remove(AllyUnit unit)
    {
        if (unit == null || !applied.Contains(unit)) return;
        applied.Remove(unit);

        //공격력 보정 원복
        if (atkBonus.TryGetValue(unit, out float bonus))
        {
            unit.upgradePower -= bonus;
            atkBonus.Remove(unit);
        }

        //사거리/타겟팅 원복
        if (savedRange.TryGetValue(unit, out float prevRange))
        {
            unit.attackRange = prevRange;
            savedRange.Remove(unit);
        }
        unit.targetRandom = false;

        Debug.Log($"[ChaosCrystal] {unit.name}: removed → stats/targeting restored");
    }
}
