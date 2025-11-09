using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class BrokenChaliceEffect : IItemEffect
{
    public string Id { get; }

    private readonly Dictionary<AllyUnit, float> prevUpgrade = new();

    // 이 아이템이 올려준 양/치확 저장
    private readonly Dictionary<AllyUnit, float> atkBonus = new();
    private readonly Dictionary<AllyUnit, float> savedCrit = new();
    private readonly HashSet<AllyUnit> applied = new();

    public BrokenChaliceEffect(string id) { Id = id; }

    public void Apply(AllyUnit unit)
    {
        if (unit == null || applied.Contains(unit)) return;
        applied.Add(unit);

        var status = unit.GetComponent<UnitStatus>();
        if (status == null) return;

        // 현재 upgradePower 백업
        prevUpgrade[unit] = unit.upgradePower;

        // 리셋 후 성배 보정만 적용
        unit.upgradePower = 0f;
        float baseAtk = status.attackPower;
        float bonus = baseAtk * 0.5f;
        atkBonus[unit] = bonus;
        unit.upgradePower += bonus;

        // 치확 0으로 고정(원값 저장)
        savedCrit[unit] = unit.criticalChance;
        unit.criticalChance = 0f;

        // 버퍼 버프 차단
        unit.SetBlockBufferBuffs(true);

        //  강화 감지용 코루틴 시작 (ChaosCrystalEffect 방식)
        unit.StartCoroutine(TrackAttackChange(unit, status));

        Debug.Log($"[BrokenChalice] {unit.name}: +50% of base({baseAtk}), crit=0, block buffs ON");
    }

    //  강화 시 공격력 자동 보정 코루틴
    private IEnumerator TrackAttackChange(AllyUnit unit, UnitStatus status)
    {
        float lastAtk = status.attackPower;
        while (applied.Contains(unit))
        {
            yield return new WaitForSeconds(0.5f);

            if (Mathf.Abs(status.attackPower - lastAtk) > 0.01f)
            {
                if (atkBonus.TryGetValue(unit, out float prevBonus))
                    unit.upgradePower -= prevBonus;

                float newBonus = status.attackPower * 0.5f;
                atkBonus[unit] = newBonus;
                unit.upgradePower += newBonus;

                lastAtk = status.attackPower;
                Debug.Log($"[BrokenChalice] {unit.name}: 강화 감지 → 보너스 갱신 ({newBonus:F1})");
            }
        }
    }

    public void Remove(AllyUnit unit)
    {
        if (unit == null || !applied.Contains(unit)) return;
        applied.Remove(unit);

        // 성배 전 상태로 upgradePower 복구
        if (prevUpgrade.TryGetValue(unit, out float prev))
        {
            unit.upgradePower = prev;
            prevUpgrade.Remove(unit);
        }
        else
        {
          // 백업이 없으면 안전하게 0
            unit.upgradePower = 0f;
        }

        // 치확 원복
        if (savedCrit.TryGetValue(unit, out float crit))
        {
            unit.criticalChance = crit;
            savedCrit.Remove(unit);
        }

        // 더했던 보정 기록 정리
        atkBonus.Remove(unit);

        // 버프 차단 해제
        unit.SetBlockBufferBuffs(false);

        Debug.Log($"[BrokenChalice] {unit.name}: restored (upgradePower={unit.upgradePower}, crit={unit.criticalChance})");
    }
}
