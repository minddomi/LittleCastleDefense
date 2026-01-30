using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public UnitStatus SpawnUnit(string unitID, int cost = 50)
    {
        // 0) 유효성 체크
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[랜덤 유닛 소환 실패] unitID가 null 또는 비어 있음");
            return null;
        }

        // 1) ID로 UnitData 가져오기
        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[유닛 데이터 없음] unitID: {unitID}");
            return null;
        }

        // 2) 소환 비용 차감 (실패 시 중단)
        if (!ResourceManager.Instance.TryUseResource(50))
        {
            return null;
        }

        // 3) 배치 가능한 타일 수집 (비어 있고 && 차단 아님)
        Tile[] tiles = FindObjectsOfType<Tile>();
        List<Tile> candidates = new List<Tile>();
        foreach (var t in tiles)
        {
            if (t.IsPlaceable) candidates.Add(t);
        }

        if (candidates.Count == 0)
        {
            Debug.Log("[스폰] 배치 가능한 타일이 없습니다.");
            return null;
        }

        // 4) 프리팹 로드
        string prefabPath = unitdata.prefabPath;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[프리팹 로딩 실패] 경로: {prefabPath}");
            return null;
        }

        // 5) 랜덤 타일 선택
        int idx = Random.Range(0, candidates.Count);
        Tile randomTile = candidates[idx];

        // 6) 스폰 (Z를 -1로 내려서 겹침 방지)
        Vector3 spawnPos = new Vector3(randomTile.gridPosition.x, randomTile.gridPosition.y, -1f);
        GameObject unit = Instantiate(prefab, spawnPos, Quaternion.identity);

        // 7) 유닛 초기화
        UnitStatus unitStatus = unit.GetComponent<UnitStatus>();
        if (unitStatus != null)
        {
            unitStatus.Initialize(unitdata, randomTile.gridPosition);
            // Debug.Log($"[유닛 정보] {status.unitName} 위치: ({status.posX}, {status.posY})");
        }

        // 8) 타일 점유 처리(내부에서 isOccupied=true / currentUnit=unit 처리)
        randomTile.SetCurrentUnit(unit);

        return unitStatus;
    }
}
