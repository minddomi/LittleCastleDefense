using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public void SpawnUnit(string unitID)
    {
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[랜덤 유닛 소환 실패] unitID가 null 또는 비어 있음");
            return;
        }

        // ID로 UnitData 가져오기
        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[유닛 데이터 없음] unitID: {unitID}");
            return;
        }

        Debug.Log($" [유닛 로딩 성공] ID: {unitID} | 이름: {unitdata.unitName} | 등급: {unitdata.unitGrade} | 공격력: {unitdata.attackPower}[유닛 정보]");

        if (!ResourceManager.Instance.TryUseResource(50))
        {
            return;
        }

        Tile[] allTiles = FindObjectsOfType<Tile>(); //Tile은 Tile.cs안의 Tile클래스(위치,유닛점유를 담고있음)
        List<Tile> emptyTiles = new List<Tile>();

        foreach (Tile tile in allTiles)
        {
            if (!tile.isOccupied)
                emptyTiles.Add(tile);
        }

        if (emptyTiles.Count == 0) return;

        Tile randomTile = emptyTiles[Random.Range(0, emptyTiles.Count)];

        string prefabPath = unitdata.prefabPath;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"[프리팹 로딩 실패] 경로: {prefabPath}");
            return;
        }
        GameObject unit = Instantiate(prefab, new Vector3(randomTile.gridPosition.x, randomTile.gridPosition.y, -1), Quaternion.identity);
        //타일이랑 유닛이랑 겹치는 경우가 발생해서 z값을 -1로 설정

        UnitStatus status = unit.GetComponent<UnitStatus>();
        if (status != null)
        {
            status.Initialize(unitdata, randomTile.gridPosition);
            Debug.Log($"[유닛 정보] {status.unitName} 위치: ({status.posX}, {status.posY})");
        }

        randomTile.isOccupied = true;
        randomTile.currentUnit = unit;
    }
}
