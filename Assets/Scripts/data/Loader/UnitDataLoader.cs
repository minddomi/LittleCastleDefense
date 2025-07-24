using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitDataLoader : MonoBehaviour
{
    public static UnitDataLoader Instance { get; private set; }

    public Dictionary<string, UnitData> unitDataMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCSV();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCSV()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/Unit");
        if (csvFile == null)
        {
            Debug.LogError("CSV 파일을 찾을 수 없습니다.");
            return;
        }

        string[] lines = csvFile.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] values = line.Split(',');

            UnitData unit = new UnitData();

            unit.unitID = values[0];
            unit.unitName = values[1];
            unit.unitClass = Enum.Parse<UnitClass>(values[2]);
            unit.unitGrade = Enum.Parse<UnitGrade>(values[3]);
            unit.gradeName = values[4];

            unit.attackPower = float.Parse(values[5]);
            unit.attackRange = float.Parse(values[6]);
            unit.attackCooldown = float.Parse(values[7]);
            unit.critRate = float.Parse(values[8]);

            unit.sellGold = int.Parse(values[9]);

            bool isBuffer = false;
            bool.TryParse(values[10].Trim(), out isBuffer);
            unit.isBuffer = isBuffer;

            unit.buffType = Enum.Parse<BuffType>(values[11]);

            bool isEquipItem = false;
            bool.TryParse(values[12].Trim(), out isEquipItem);
            unit.isEquipItem = isEquipItem;

            unit.equippedItemID = values[13];
            unit.prefabPath = values[14];

            unit.posX = float.Parse(values[15]);
            unit.posY = float.Parse(values[16]);

            unit.canMerge = true;

            unitDataMap[unit.unitID] = unit;
        }
    }
}

