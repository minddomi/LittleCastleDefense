using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnitData
{
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

    public float posX;
    public float posY;

    public bool canMerge;

    public string UIPath;
}