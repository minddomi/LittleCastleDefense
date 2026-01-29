using System.Collections.Generic;
using UnityEngine;

public class GlobalUpgradeManager : MonoBehaviour
{
    public static GlobalUpgradeManager Instance;

    [Header("기본 강화 설정")]
    public float archerBaseCost = 7f;
    public float archerAttackIncrease = 5f;
    public float archerCostIncrease = 4f;

    public float mageBaseCost = 5f;
    public float mageAttackIncrease = 3f;
    public float mageCostIncrease = 2f;

    public float siegeBaseCost = 7f;
    public float siegeAttackIncrease = 5f;
    public float siegeCostIncrease = 4f;

    public float bufferBaseCost = 7f;
    public float bufferAttackIncrease = 1f;
    public float bufferCostIncrease = 4f;

    private List<UnitStatus> activeUnits = new List<UnitStatus>();

    // 현재 누적 강화량
    private float archerBonus = 0f;
    private float mageBonus = 0f;
    private float siegeBonus = 0f;
    private float bufferBonus = 0f;

    // 다음 강화 비용
    private float archerCost;
    private float mageCost;
    private float siegeCost;
    private float bufferCost;

    // 강화 레벨
    private int archerLevel = 0;
    private int mageLevel = 0;
    private int siegeLevel = 0;
    private int bufferLevel = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        archerCost = archerBaseCost;
        mageCost = mageBaseCost;
        siegeCost = siegeBaseCost;
        bufferCost = bufferBaseCost;
    }

    public void RegisterUnit(UnitStatus unit)
    {
        activeUnits.Add(unit);

        switch (unit.unitClass)
        {
            case UnitClass.Archer: unit.attackPower += archerBonus; break;
            case UnitClass.Mage: unit.attackPower += mageBonus; break;
            case UnitClass.Siege: unit.attackPower += siegeBonus; break;
            case UnitClass.Buffer: unit.attackPower += bufferBonus; break;
        }
    }

    public void UnregisterUnit(UnitStatus unit)
    {
        activeUnits.Remove(unit);
    }

    // 클래스별 업그레이드
    public void UpgradeArcher() => TryUpgrade(UnitClass.Archer, ref archerBonus, archerAttackIncrease, ref archerCost, archerCostIncrease, ref archerLevel);
    public void UpgradeMage() => TryUpgrade(UnitClass.Mage, ref mageBonus, mageAttackIncrease, ref mageCost, mageCostIncrease, ref mageLevel);
    public void UpgradeSiege() => TryUpgrade(UnitClass.Siege, ref siegeBonus, siegeAttackIncrease, ref siegeCost, siegeCostIncrease, ref siegeLevel);
    public void UpgradeBuffer() => TryUpgrade(UnitClass.Buffer, ref bufferBonus, bufferAttackIncrease, ref bufferCost, bufferCostIncrease, ref bufferLevel);

    private bool TryUpgrade(UnitClass type, ref float bonus, float atkIncrease, ref float cost, float costIncrease, ref int level)
    {
        int intCost = Mathf.RoundToInt(cost);
        if (!ResourceManager.Instance.TryUseResource(intCost))
        {
            //GameLogManager.Instance.AddLog("<color=#FF7777>자원이 부족합니다!</color>");
            return false;
        }

        level++;
        bonus += atkIncrease;

        // 기존 유닛에 즉시 적용
        foreach (var unit in activeUnits)
        {
            if (unit.unitClass == type)
                unit.attackPower += atkIncrease;
        }

        // 로그 출력
        //RoundTimer timer = FindObjectOfType<RoundTimer>();
        //string currentTime = timer != null ? timer.GetFormattedTime() : "--:--";
        //string unitName = GetKoreanName(type);

        //string logMsg =
        //    $"<color=#BBBBBB>[{currentTime}]</color> " +
        //    $"유닛 종족 강화(<color=#FF7777>자원 소비 -{intCost}</color>): " +
        //    $"<color=#77DDFF>{unitName}</color> 유닛 강화 " +
        //    $"(<color=#77DDFF>{level - 1}</color> → <color=#77DDFF>{level}</color>, " +
        //    $"누적 공격력 <color=#77DDFF>+{bonus}</color>)";

        //GameLogManager.Instance.AddLog(logMsg);

        // 다음 단계 비용 증가
        cost += costIncrease;

        // 모든 버튼 UI 갱신 (Archer만 아닌 전체)
        foreach (var btn in FindObjectsOfType<UnitUpgradeButton>())
        {
            btn.RefreshUI(type, level, Mathf.RoundToInt(cost), bonus);
        }

        return true;
    }

    private string GetKoreanName(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Archer: return "궁수";
            case UnitClass.Mage: return "마법사";
            case UnitClass.Siege: return "공성병";
            case UnitClass.Buffer: return "버퍼";
            default: return "알 수 없음";
        }
    }

    // UI에서 호출할 수 있도록 현재 값 반환
    public int GetLevel(UnitClass type)
    {
        return type switch
        {
            UnitClass.Archer => archerLevel,
            UnitClass.Mage => mageLevel,
            UnitClass.Siege => siegeLevel,
            UnitClass.Buffer => bufferLevel,
            _ => 0
        };
    }

    public int GetCost(UnitClass type)
    {
        return Mathf.RoundToInt(type switch
        {
            UnitClass.Archer => archerCost,
            UnitClass.Mage => mageCost,
            UnitClass.Siege => siegeCost,
            UnitClass.Buffer => bufferCost,
            _ => 0
        });
    }

    public float GetBonus(UnitClass type)
    {
        return type switch
        {
            UnitClass.Archer => archerBonus,
            UnitClass.Mage => mageBonus,
            UnitClass.Siege => siegeBonus,
            UnitClass.Buffer => bufferBonus,
            _ => 0f
        };
    }
}
