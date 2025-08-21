using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBranchEffect : IItemEffect
{
    public string Id { get; }
    private readonly Dictionary<AllyUnit, float> originalInterval = new();

    // 성장 배율
    private const float GrowthPerRound = 0.99f; // 1% 빨라짐 = interval × 0.99

    public InfiniteBranchEffect(string id)
    {
        Id = id;
    }

    public void Apply(AllyUnit unit)
    {
        if (!originalInterval.ContainsKey(unit))
            originalInterval[unit] = unit.attackInterval;

        // 라운드 이벤트 구독
        RoundTimer.OnRoundChanged += (round) =>
        {
            if (unit == null) return;
            if (!originalInterval.ContainsKey(unit)) return;

            unit.attackInterval *= GrowthPerRound;
            if (unit.attackInterval < 0.05f) // 안전 최소치
                unit.attackInterval = 0.05f;

            Debug.Log($"[InfiniteBranch] {unit.name} round {round}, new interval {unit.attackInterval}");
        };
    }

    public void Remove(AllyUnit unit)
    {
        if (originalInterval.TryGetValue(unit, out float baseVal))
        {
            unit.attackInterval = baseVal;
            originalInterval.Remove(unit);
        }
    }
}
