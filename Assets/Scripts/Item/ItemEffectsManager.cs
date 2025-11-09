using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemEffectsManager : MonoBehaviour
{
    public static ItemEffectsManager Instance { get; private set; }

    // 아이템 ID
    private readonly Dictionary<string, IItemEffect> registry = new();

    // 유닛별 현재 적용 중인 아이템 ID (변경 감지용)
    private readonly Dictionary<AllyUnit, string> appliedByUnit = new();

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        Register(new HungrySwordEffect("HungrySword"));
        Register(new ChaosCrystalEffect("ChaosCrystal"));
        Register(new InfiniteBranchEffect("InfiniteBranch"));
        Register(new BalanceScaleEffect("BalanceScale"));
        Register(new BrokenChaliceEffect("BrokenChalice"));
        Register(new MadnessTomeEffect("MadnessTome"));
    }

    private void Start()
    {
        // 씬 내 모든 유닛을 순회하면서 강화 이벤트 구독
        foreach (var status in FindObjectsOfType<UnitStatus>())
        {
            status.OnUpgraded -= HandleUpgrade;
            status.OnUpgraded += HandleUpgrade;
        }
    }

    private void HandleUpgrade(UnitStatus status)
    {
        if (status == null) return;
        var unit = status.GetComponent<AllyUnit>();
        if (unit == null) return;

        Debug.Log($"[ItemEffectsManager] {status.name} 강화 감지 → 아이템 효과 재적용");
        Sync(unit, status); // 자동 Remove + Apply
    }

    private void Register(IItemEffect effect)
    {
        if (!registry.ContainsKey(effect.Id))
            registry.Add(effect.Id, effect);
    }

    public void Sync(AllyUnit unit, UnitStatus status)
    {
        if (unit == null || status == null) return;

        // 현재 장착 아이템 ID (없으면 null)
        string nextId = (status.isEquipItem && !string.IsNullOrEmpty(status.equippedItemID))
            ? status.equippedItemID
            : null;

        appliedByUnit.TryGetValue(unit, out string prevId);
        if (prevId == nextId) return;

        // 이전 효과 제거
        if (!string.IsNullOrEmpty(prevId) && registry.TryGetValue(prevId, out var prevEff))
            prevEff.Remove(unit);

        // 새 효과 적용
        if (!string.IsNullOrEmpty(nextId) && registry.TryGetValue(nextId, out var nextEff))
        {
            nextEff.Apply(unit);
            appliedByUnit[unit] = nextId;
        }
        else
        {
            appliedByUnit.Remove(unit);
        }
    }

    public void OnUnitDestroyed(AllyUnit unit)
    {
        if (unit == null) return;
        if (appliedByUnit.TryGetValue(unit, out var id) && registry.TryGetValue(id, out var eff))
            eff.Remove(unit);
        appliedByUnit.Remove(unit);
    }
}
