using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungrySwordEffect : IItemEffect
{
    public string Id { get; }
    private readonly Dictionary<AllyUnit, float> originalCritChance = new();

    private const float CritChancePerKill = 0.01f;
    private const float MaxCritChanceIncrease = 0.30f;

    public HungrySwordEffect(string id)
    {
        Id = id;
    }

    public void Apply(AllyUnit unit)
    {
        if (!originalCritChance.ContainsKey(unit))
        {
            originalCritChance[unit] = unit.critChance;
        }

        // 적 유닛 처치 이벤트 구독
        Unit.OnEnemyKilled += (killedUnit, killingUnit) =>
        {
            // 이 효과가 적용된 유닛이 막타를 쳤는지 확인
            if (killingUnit == unit)
            {
                if (!originalCritChance.ContainsKey(unit)) return;

                // 현재 증가된 치명타 확률 계산
                float currentIncrease = unit.critChance - originalCritChance[unit];

                // 최대 증가량(30%)에 도달하지 않았을 경우에만 증가
                if (currentIncrease < MaxCritChanceIncrease)
                {
                    unit.critChance += CritChancePerKill;
                    if (unit.critChance > originalCritChance[unit] + MaxCritChanceIncrease)
                    {
                        unit.critChance = originalCritChance[unit] + MaxCritChanceIncrease;
                    }
                    
                    Debug.Log($"[HungrySword] {unit.name} 치명타 확률 증가, 현재: {unit.critChance}");
                }
            }
        };
    }

    public void Remove(AllyUnit unit)
    {
        if (originalCritChance.TryGetValue(unit, out float baseVal))
        {
            unit.critChance = baseVal;
            originalCritChance.Remove(unit);
            Debug.Log($"[HungrySword] {unit.name} 치명타 확률 초기화, 원래 값: {baseVal}");
        }
    }
}
