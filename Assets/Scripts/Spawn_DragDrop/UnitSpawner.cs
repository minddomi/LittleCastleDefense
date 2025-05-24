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

        Tile[] allTiles = FindObjectsOfType<Tile>(); //Tile�� Tile.cs���� TileŬ����(��ġ,���������� �������)
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
        //Ÿ���̶� �����̶� ��ġ�� ��찡 �߻��ؼ� z���� -1�� ����

        UnitMetadata meta = unit.GetComponent<UnitMetadata>();
        if (meta != null)
        {
            meta.unitClass = unitClass;
            meta.unitGrade = unitGrade;
        }

        randomTile.isOccupied = true;

    }
}
