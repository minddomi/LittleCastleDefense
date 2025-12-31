using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyData
{
    public string enemyID;
    public string name;
    public EnemySize size;
    public float hp;
    public float moveSpeed;
    public string prefabPath; // Resources.Load에 넣을 경로(확장자 X)
}

public class EnemyDataLoader : MonoBehaviour
{
    public static EnemyDataLoader Instance { get; private set; }
    public Dictionary<string, EnemyData> enemyMap = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCSV();
        }
        else Destroy(gameObject);
    }

    void LoadCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("CSV/enemy_units");
        if (csv == null)
        {
            Debug.LogError("enemy_units.csv를 Resources/CSV에서 찾지 못했습니다.");
            return;
        }

        string[] lines = csv.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            string[] v = line.Split(',');
            // CSV: enemyID,name,type,HP,moveSpeed,prefabName
            string id = v[0].Trim().Trim('\uFEFF'); // BOM 방지
            string nm = v[1].Trim();
            string type = v[2].Trim();
            float hp = float.Parse(v[3].Trim());
            float sp = float.Parse(v[4].Trim());
            string prefabName = v[5].Trim();

            EnemySize size = EnemySize.Medium;
            if (type.Contains("소형")) size = EnemySize.Small;
            else if (type.Contains("중형")) size = EnemySize.Medium;
            else if (type.Contains("대형")) size = EnemySize.Large;

            // 경로
            var data = new EnemyData
            {
                enemyID = id,
                name = nm,
                size = size,
                hp = hp,
                moveSpeed = sp,
                prefabPath = $"Enemies/{prefabName}"
            };

            enemyMap[id] = data;
        }
    }
}
