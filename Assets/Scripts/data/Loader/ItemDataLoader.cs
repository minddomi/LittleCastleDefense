using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ItemDataLoader : MonoBehaviour
{
    public static ItemDataLoader Instance;

    public Dictionary<string, ItemData> itemDataMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadItemData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadItemData()
    {
        TextAsset csvFile = Resources.Load<TextAsset>("CSV/Item"); // Resources/Item.csv
        string[] lines = csvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++) // 0번은 헤더
        {
            string line = lines[i].Trim();
            if (string.IsNullOrWhiteSpace(line)) continue;

            string[] cells = line.Split(',');

            ItemData data = new ItemData
            {
                itemID = cells[0],
                prefabPath = cells[1]
            };

            itemDataMap[data.itemID] = data;
        }
    }
}
