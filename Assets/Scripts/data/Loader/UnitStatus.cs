using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public event System.Action<UnitStatus> OnUpgraded;

    public UnitData unitData;

    public string unitID;
    public string unitName;
    public UnitClass unitClass;
    public UnitGrade unitGrade;
    public string gradeName;

    public float attackPower;
    public float attackRange;
    public float attackInterval;
    public float criticalChance;

    public int sellGold;
    public bool isBuffer;
    public BuffType buffType;

    public bool isEquipItem;
    public string equippedItemID;
    public string prefabPath;

    public int posX;
    public int posY;

    public bool canMerge;

    public string UIPath;

    public float criticalMultiplier;
    public float upgradeLevel;
    public float upgradePower;
    public float TotalAttackPower;

    public void Initialize(UnitData data, Vector2Int pos)
    {
        unitID = data.unitID;
        unitName = data.unitName;
        unitClass = data.unitClass;
        unitGrade = data.unitGrade;
        gradeName = data.gradeName;

        attackPower = data.attackPower;
        attackRange = data.attackRange;
        attackInterval = data.attackInterval;
        criticalChance = data.criticalChance;

        sellGold = data.sellGold;
        isBuffer = data.isBuffer;
        buffType = data.buffType;

        isEquipItem = data.isEquipItem;
        equippedItemID = data.equippedItemID;
        prefabPath = data.prefabPath;

        posX = pos.x;
        posY = pos.y;

        canMerge = data.canMerge;

        UIPath = data.UIPath;

        criticalMultiplier = data.criticalMultiplier;
        upgradeLevel = data.upgradeLevel;
        upgradePower = data.upgradePower;
        TotalAttackPower = data.TotalAttackPower;

        if (GlobalUpgradeManager.Instance != null)
            GlobalUpgradeManager.Instance.RegisterUnit(this);
    }
    public void ApplyUpgrade(float multiplier)
    {
        attackPower *= multiplier;

        OnUpgraded?.Invoke(this); // 
    }
}
