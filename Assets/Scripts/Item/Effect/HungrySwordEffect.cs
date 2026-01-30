using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungrySwordEffect : IItemEffect
{
    public string Id { get; }
    private const float PerKill = 0.01f; // +1%
    private const float Cap = 0.30f; // 최대 +30%

    // 유닛별로 이 아이템이 올려준 치확 누적치 기록
    private readonly Dictionary<AllyUnit, float> addedCrit = new();
    private bool subscribed = false;

    public HungrySwordEffect(string id) { Id = id; }

    public void Apply(AllyUnit unit)
    {
        if (!addedCrit.ContainsKey(unit))
            addedCrit[unit] = 0f;

        // 전역 킬 이벤트 구독(한 번만)
        if (!subscribed)
        {
            EnemyUnit.OnEnemyKilledBy += OnEnemyKilled;
            subscribed = true;
        }
    }

    private void OnEnemyKilled(AllyUnit killer)
    {
        if (killer == null) return;
        if (!addedCrit.TryGetValue(killer, out float cur)) return; // 이 아이템을 장착한 유닛이 아님

        float delta = Mathf.Min(PerKill, Cap - cur);
        if (delta <= 0f) return;

        killer.criticalChance += delta; // 치확 증가
        addedCrit[killer] = cur + delta; // 누적 기록
        // Debug.Log($"[HungrySword] {killer.name} crit +{delta:P0} (total +{addedCrit[killer]:P0})");
    }

    public void Remove(AllyUnit unit)
    {
        if (addedCrit.TryGetValue(unit, out float gained))
        {
            // 이 아이템이 올려준 만큼만 회수
            unit.criticalChance -= gained;
            addedCrit.Remove(unit);
        }

        // 더 이상 이 아이템을 쓰는 유닛이 없으면 구독 해제
        if (subscribed && addedCrit.Count == 0)
        {
            EnemyUnit.OnEnemyKilledBy -= OnEnemyKilled;
            subscribed = false;
        }
    }
}
