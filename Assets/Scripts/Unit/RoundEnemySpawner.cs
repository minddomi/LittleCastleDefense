using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class RoundEnemySpawner : MonoBehaviour
{
    [Header("Path / Movement")]
    public Transform[] waypoints;

    [Header("Behavior")]
    public bool spawnRound1OnStart = true;
    public bool stopPreviousSpawnsOnNewRound = true;

    private class SpawnPlan
    {
        public string enemyID;
        public int count;
        public float interval;
        public Vector2 spawnPos;
    }

    private readonly Dictionary<int, List<SpawnPlan>> schedule = new();

    private void OnEnable()
    {
        RoundTimer.OnRoundChanged += HandleRoundChanged;
    }

    private void OnDisable()
    {
        RoundTimer.OnRoundChanged -= HandleRoundChanged;
    }

    private void Start()
    {
        LoadScheduleCSV();

        // RoundTimer는 시작 시점에 이벤트를 안 쏘기 때문에,
        // 1라운드 스폰이 필요하면 여기서 직접 호출
        if (spawnRound1OnStart)
            StartRound(1);
    }

    void HandleRoundChanged(int round)
    {
        StartRound(round);
    }

    void StartRound(int round)
    {
        if (stopPreviousSpawnsOnNewRound)
            StopAllCoroutines();

        if (!schedule.TryGetValue(round, out var plans))
        {
            // 해당 라운드 스폰 없음
            return;
        }

        foreach (var p in plans)
            StartCoroutine(SpawnRoutine(p, round));
    }

    IEnumerator SpawnRoutine(SpawnPlan plan, int round)
    {
        if (EnemyDataLoader.Instance == null)
        {
            Debug.LogError("EnemyDataLoader.Instance가 없습니다. 씬에 EnemyDataLoader를 배치하세요.");
            yield break;
        }

        if (!EnemyDataLoader.Instance.enemyMap.TryGetValue(plan.enemyID, out var data))
        {
            Debug.LogError($"[Round {round}] enemyID '{plan.enemyID}'를 enemy_units.csv에서 찾지 못했습니다.");
            yield break;
        }

        GameObject prefab = Resources.Load<GameObject>(data.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[Round {round}] prefabPath '{data.prefabPath}'를 Resources에서 찾지 못했습니다.");
            yield break;
        }

        for (int i = 0; i < plan.count; i++)
        {
            Vector3 pos = new Vector3(plan.spawnPos.x, plan.spawnPos.y, -1f);
            GameObject go = Instantiate(prefab, pos, Quaternion.identity);

            var enemy = go.GetComponent<EnemyUnit>();
            if (enemy != null)
            {
                enemy.size = data.size;
                enemy.health = data.hp;
                enemy.speed = data.moveSpeed;
                enemy.waypoints = waypoints;
            }

            yield return new WaitForSeconds(plan.interval);
        }
    }

    void LoadScheduleCSV()
    {
        TextAsset csv = Resources.Load<TextAsset>("CSV/round_shcedule");
        if (csv == null)
        {
            Debug.LogError("round_shcedule.csv를 Resources/CSV에서 찾지 못했습니다.");
            return;
        }

        string[] lines = csv.text.Split('\n');
        for (int i = 1; i < lines.Length; i++)
        {
            string line = lines[i].Trim();
            if (string.IsNullOrEmpty(line)) continue;

            // CSV: round,enemyID,count,interval,spawnPos
            string[] v = SplitCsvLine(line);
            int round = int.Parse(v[0].Trim().Trim('\uFEFF'));
            string enemyID = v[1].Trim();
            int count = int.Parse(v[2].Trim());
            float interval = float.Parse(v[3].Trim());
            Vector2 spawnPos = ParsePos(v[4]);

            var plan = new SpawnPlan
            {
                enemyID = enemyID,
                count = count,
                interval = interval,
                spawnPos = spawnPos
            };

            if (!schedule.ContainsKey(round))
                schedule[round] = new List<SpawnPlan>();
            schedule[round].Add(plan);
        }
    }

    // "(4,9.5)" 형태 파싱
    Vector2 ParsePos(string s)
    {
        s = s.Trim().Trim('"');   // 따옴표 제거
        s = s.Trim('(', ')');     // 괄호 제거

        var parts = s.Split(',');
        if (parts.Length < 2)
            throw new FormatException($"spawnPos 형식이 이상함: '{s}'");

        float x = float.Parse(parts[0].Trim(), CultureInfo.InvariantCulture);
        float y = float.Parse(parts[1].Trim(), CultureInfo.InvariantCulture);
        return new Vector2(x, y);
    }

    // spawnPos에 따옴표가 있어서 단순 Split(',')로 깨질 수 있으니 최소 안전장치
    string[] SplitCsvLine(string line)
    {
        var list = new List<string>();
        bool inQuotes = false;
        var cur = "";

        foreach (char c in line)
        {
            if (c == '"') { inQuotes = !inQuotes; cur += c; continue; }
            if (c == ',' && !inQuotes)
            {
                list.Add(cur);
                cur = "";
            }
            else cur += c;
        }
        list.Add(cur);
        return list.ToArray();
    }
}
