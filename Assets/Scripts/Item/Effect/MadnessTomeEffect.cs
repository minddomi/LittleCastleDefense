using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MadnessTomeEffect : IItemEffect
{
    public string Id { get; }

    // 원래 공격주기 복구용
    private readonly Dictionary<AllyUnit, float> baseInterval = new();
    // 유닛별 카운터
    private readonly Dictionary<AllyUnit, int> shotCount = new();
    // 이벤트 해제를 위한 핸들러 저장
    private readonly Dictionary<AllyUnit, Action> handlers = new();

    private const float StallSeconds = 1.0f;

    public MadnessTomeEffect(string id) { Id = id; }

    public void Apply(AllyUnit unit)
    {
        if (unit == null) return;
        if (handlers.ContainsKey(unit)) return; // 중복 적용 방지

        // 공속 ×2 => 공격주기 1/2
        if (!baseInterval.ContainsKey(unit))
            baseInterval[unit] = unit.attackInterval;

        unit.attackInterval = Mathf.Max(0.05f, unit.attackInterval * 0.5f);

        shotCount[unit] = 0;

        // 발사 시 카운트 → 3의 배수면 1초 정지
        Action handler = () =>
        {
            if (!shotCount.ContainsKey(unit)) shotCount[unit] = 0;
            shotCount[unit]++;

            if (shotCount[unit] % 3 == 0)
            {
                unit.FreezeFor(StallSeconds);
            }
        };

        unit.OnAttackFired += handler;
        handlers[unit] = handler;
    }

    public void Remove(AllyUnit unit)
    {
        if (unit == null) return;

        // 이벤트 해제
        if (handlers.TryGetValue(unit, out var h))
        {
            unit.OnAttackFired -= h;
            handlers.Remove(unit);
        }

        // 원래 주기로 복구
        if (baseInterval.TryGetValue(unit, out var baseVal))
        {
            unit.attackInterval = baseVal;
            baseInterval.Remove(unit);
        }

        shotCount.Remove(unit);
    }
}
