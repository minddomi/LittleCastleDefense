using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public void SpawnUnit(UnitClass unitClass, UnitGrade unitGrade)
    {
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

        string prefabPath = $"Units/{unitClass}/{unitGrade}/{unitClass}Unit";
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null) return;

        GameObject unit = Instantiate(prefab, new Vector3(randomTile.gridPosition.x, randomTile.gridPosition.y, -1), Quaternion.identity);
        //타일이랑 유닛이랑 곂치는 경우가 발생해서 z값을 -1로 설정

        UnitMetadata meta = unit.GetComponent<UnitMetadata>();
        if (meta != null)
        {
            meta.unitClass = unitClass;
            meta.unitGrade = unitGrade;
        }

        randomTile.isOccupied = true;

    }
}
