using UnityEngine;
using System.Collections;


public class TutorialUnitSpawner : MonoBehaviour
{
    [Header("Spawn on Scene Start")]
    [SerializeField] private bool spawnOnStart = true;

    private bool joker1Spawned = false;

    IEnumerator Start()
    {
        Debug.Log("[TutorialSpawner] Start called");
        if (!spawnOnStart) yield break;

        yield return null; // 1프레임 대기 (타일 생성 기다림)

        Debug.Log("[TutorialSpawner] Spawning after 1 frame");
        // 씬 시작 시 바로 소환(바로 보임)
        SpawnAt("Archer1", new Vector2Int(1, 7));
        SpawnAt("Siege1", new Vector2Int(3, 7));
        SpawnAt("Mage1", new Vector2Int(5, 7));
        SpawnAt("Buffer1", new Vector2Int(7, 7));

        // 조커도 같이 시작하려면 주석 해제
        //SpawnAt("Joker1", new Vector2Int(3, 3));
        //SpawnAt("Joker2", new Vector2Int(5, 3));
    }

    public void SpawnJoker2()
    {
        if (joker1Spawned) return;
        joker1Spawned = true;

        SpawnAt("Joker2", new Vector2Int(5, 3));
    }

    public void SpawnJoker1()
    {
        if (joker1Spawned) return;
        joker1Spawned = true;

        SpawnAt("Joker1", new Vector2Int(3, 3));
    }

    public UnitStatus SpawnAt(string unitID, Vector2Int gridPos)
    {
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[Tutorial Spawn Fail] unitID 비어있음");
            return null;
        }

        if (UnitDataLoader.Instance == null)
        {
            Debug.LogError("[Tutorial Spawn Fail] UnitDataLoader.Instance null");
            return null;
        }

        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[Tutorial Spawn Fail] 유닛 데이터 없음: {unitID}");
            return null;
        }

        Tile targetTile = FindTile(gridPos);
        if (targetTile == null)
        {
            Debug.LogWarning($"[Tutorial Spawn Fail] 타일 없음: ({gridPos.x},{gridPos.y})");
            return null;
        }

        if (!targetTile.IsPlaceable)
        {
            Debug.LogWarning($"[Tutorial Spawn Fail] 배치 불가 타일: ({gridPos.x},{gridPos.y})");
            return null;
        }

        GameObject prefab = Resources.Load<GameObject>(unitdata.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[Tutorial Spawn Fail] 프리팹 로딩 실패: {unitdata.prefabPath}");
            return null;
        }

        Vector3 spawnPos = new Vector3(gridPos.x, gridPos.y, -1f);
        GameObject unit = Instantiate(prefab, spawnPos, Quaternion.identity);

        UnitStatus status = unit.GetComponent<UnitStatus>();
        if (status != null)
            status.Initialize(unitdata, gridPos);
        else
            Debug.LogWarning($"[Tutorial Spawn Warn] UnitStatus 없음: {unitID}");

        targetTile.SetCurrentUnit(unit);

        return status;
    }

    private Tile FindTile(Vector2Int gridPos)
    {
        Tile[] tiles = FindObjectsOfType<Tile>();
        foreach (var t in tiles)
        {
            if (t.gridPosition == gridPos)
                return t;
        }
        return null;
    }
}
