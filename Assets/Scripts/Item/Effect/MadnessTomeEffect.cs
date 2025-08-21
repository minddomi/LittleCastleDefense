using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadnessTomeEffect : IItemEffect
{
    public string Id { get; }
    private readonly float targetInterval;
    private readonly Dictionary<AllyUnit, float> originalInterval = new();

    public MadnessTomeEffect(string id, float targetInterval = 0.5f)
    {
        Id = id;
        this.targetInterval = Mathf.Max(0.01f, targetInterval);
    }

    public void Apply(AllyUnit unit)
    {
        // 원래 공격 주기를 저장
        if (!originalInterval.ContainsKey(unit))
            originalInterval[unit] = unit.attackInterval;
        // 공격 주기 변경
        unit.attackInterval = targetInterval;
    }

    public void Remove(AllyUnit unit)
    {
        // 원래 공격 주기를 복원
        if (originalInterval.TryGetValue(unit, out float baseVal))
        {
            unit.attackInterval = baseVal;
            originalInterval.Remove(unit);
        }
    }
}
