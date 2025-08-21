using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenChaliceEffect : IItemEffect
{
    public string Id { get; }

    private readonly Dictionary<AllyUnit, float> originalCritChance = new();

    private const float ForcedCritChance = 0f;   // 치명 0% 고정

    public BrokenChaliceEffect(string id) { Id = id; }


    public void Apply(AllyUnit unit)
    {
        // 치명 0% 고정
        if (!originalCritChance.ContainsKey(unit))
            originalCritChance[unit] = unit.criticalChance;
        unit.criticalChance = Mathf.Clamp01(ForcedCritChance);
    }

    public void Remove(AllyUnit unit)
    {
        // 치명타 확률 원복
        if (originalCritChance.TryGetValue(unit, out float baseCrit))
        {
            unit.criticalChance = Mathf.Clamp01(baseCrit);
            originalCritChance.Remove(unit);
        }
    }
}
