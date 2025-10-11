using System;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBranchEffect : IItemEffect
{
    public string Id { get; }

    // 이 아이템이 줄여준 누적치만 기록 (복구용)
    private readonly Dictionary<AllyUnit, float> totalDelta = new();
    // 이벤트 핸들러 저장(중복 구독/해제용)
    private readonly Dictionary<AllyUnit, Action<int>> handlers = new();

    private const float StepPerRound = 0.01f;  // 라운드마다 공격주기 0.01s 감소
    private const float MinInterval = 0.05f;  // 하한

    public InfiniteBranchEffect(string id) { Id = id; }

    public void Apply(AllyUnit unit)
    {
        if (unit == null) return;
        if (handlers.ContainsKey(unit)) return;     // 중복 방지

        if (!totalDelta.ContainsKey(unit))
            totalDelta[unit] = 0f;

        // 장착 라운드에는 아무 것도 하지 않음. 다음 라운드 이벤트부터 적용.
        Action<int> h = (round) =>
        {
            if (unit == null) return;

            float before = unit.attackInterval;
            float after = Mathf.Max(MinInterval, before - StepPerRound);
            float applied = before - after; // 실제로 줄인 양

            if (applied > 0f)
            {
                unit.attackInterval = after;
                totalDelta[unit] += applied; // 누적 기록(복구용)
                Debug.Log($"[InfiniteBranch] {unit.name} R{round}: {before:F3} → {after:F3} (Δ{applied:F3})  totalΔ={totalDelta[unit]:F3}");
            }
        };

        RoundTimer.OnRoundChanged += h;
        handlers[unit] = h;
    }

    public void Remove(AllyUnit unit)
    {
        if (unit == null) return;

        // 이벤트 해제
        if (handlers.TryGetValue(unit, out var h))
        {
            RoundTimer.OnRoundChanged -= h;
            handlers.Remove(unit);
        }

        // 아이템이 적용했던 양만큼만 되돌림
        if (totalDelta.TryGetValue(unit, out float delta))
        {
            unit.attackInterval += delta;
            totalDelta.Remove(unit);
        }
    }
}
