using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatus : MonoBehaviour
{
    public UnitData unitData;

    public string unitID;
    public string unitName;
    public UnitClass unitClass;
    public UnitGrade unitGrade;
    public string gradeName;

    public float attackPower;
    public float attackRange;
    public float attackCooldown;
    public float critRate;

    public int sellGold;
    public bool isBuffer;
    public BuffType buffType;

    public bool isEquipItem;
    public string equippedItemID;
    public string prefabPath;

    public int posX;
    public int posY;

    public void Initialize(UnitData data, Vector2Int pos)
    {
        unitID = data.unitID;
        unitName = data.unitName;
        unitClass = data.unitClass;
        unitGrade = data.unitGrade;
        gradeName = data.gradeName;

        attackPower = data.attackPower;
        attackRange = data.attackRange;
        attackCooldown = data.attackCooldown;
        critRate = data.critRate;

        sellGold = data.sellGold;
        isBuffer = data.isBuffer;
        buffType = data.buffType;

        isEquipItem = data.isEquipItem;
        equippedItemID = data.equippedItemID;
        prefabPath = data.prefabPath;

        posX = pos.x;
        posY = pos.y;
    }
}
