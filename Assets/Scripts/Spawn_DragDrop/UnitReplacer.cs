using System.Collections.Generic;
using UnityEngine;

public class UnitReplacer : MonoBehaviour
{
    /// <summary>
    /// 프리팹(UnitStatus)에 저장된 좌표로 소환.
    /// </summary>
    public void SpawnUnit(string unitID)
    {
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[지정 위치 소환 실패] unitID가 null 또는 비어 있음");
            return;
        }

        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[유닛 데이터 없음] unitID: {unitID}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(unitdata.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[프리팹 로딩 실패] 경로: {unitdata.prefabPath}");
            return;
        }

        UnitStatus prefabStatus = prefab.GetComponent<UnitStatus>();
        if (prefabStatus == null)
        {
            Debug.LogError("[소환 실패] 프리팹에 UnitStatus가 없습니다.");
            return;
        }

        Vector2Int gridPos = new Vector2Int(prefabStatus.posX, prefabStatus.posY);
        SpawnCore(prefab, unitdata, gridPos);
    }

    /// <summary>
    /// 지정 좌표(gridPos)로 같은 유닛을 소환(자원 소모 없음).
    /// BrokenChaliceEffect.Remove()에서 사용.
    /// </summary>
    public void SpawnSameSpot(string unitID, Vector2Int gridPos)
    {
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[지정 위치 소환 실패] unitID가 null 또는 비어 있음");
            return;
        }

        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[유닛 데이터 없음] unitID: {unitID}");
            return;
        }

        GameObject prefab = Resources.Load<GameObject>(unitdata.prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"[프리팹 로딩 실패] 경로: {unitdata.prefabPath}");
            return;
        }

        SpawnCore(prefab, unitdata, gridPos);
    }

    /// <summary>
    /// 실제 소환 처리(공통).
    /// - 타일 점유 검사 및 설정
    /// - UnitStatus.Initialize(data, gridPos)
    /// - z=-1 배치
    /// </summary>
    private void SpawnCore(GameObject prefab, UnitData unitdata, Vector2Int gridPos)
    {
        Tile targetTile = FindTileByGridPos(gridPos);
        if (targetTile == null)
        {
            Debug.LogError($"[소환 실패] 해당 좌표의 타일을 찾을 수 없음: {gridPos}");
            return;
        }
        if (targetTile.isOccupied)
        {
            Debug.LogWarning($"[소환 취소] 타일이 이미 점유됨: {gridPos}");
            return;
        }

        Vector3 worldPos = new Vector3(gridPos.x, gridPos.y, -1f);
        GameObject unit = Instantiate(prefab, worldPos, Quaternion.identity);

        UnitStatus status = unit.GetComponent<UnitStatus>();
        if (status != null)
        {
            status.Initialize(unitdata, gridPos);
            // 필요 시 장비/효과 재동기화가 여기서 이뤄지도록 매니저 호출 가능
            // ItemEffectsManager.Instance?.Sync(unit.GetComponent<AllyUnit>(), status);
        }

        targetTile.isOccupied = true;
        targetTile.currentUnit = unit;
    }

    private Tile FindTileByGridPos(Vector2Int gridPos)
    {
        foreach (var t in FindObjectsOfType<Tile>())
            if (t.gridPosition == gridPos) return t;
        return null;
    }
}
