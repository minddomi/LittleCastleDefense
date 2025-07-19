using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public void SpawnUnit(string unitID)
    {
        if (string.IsNullOrEmpty(unitID))
        {
            Debug.LogWarning("[���� ���� ��ȯ ����] unitID�� null �Ǵ� ��� ����");
            return;
        }

        // ID�� UnitData ��������
        if (!UnitDataLoader.Instance.unitDataMap.TryGetValue(unitID, out UnitData unitdata))
        {
            Debug.LogWarning($"[���� ������ ����] unitID: {unitID}");
            return;
        }

        Debug.Log($" [���� �ε� ����] ID: {unitID} | �̸�: {unitdata.unitName} | ���: {unitdata.unitGrade} | ���ݷ�: {unitdata.attackPower}[���� ����]");

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

        string prefabPath = unitdata.prefabPath;
        GameObject prefab = Resources.Load<GameObject>(prefabPath);

        if (prefab == null)
        {
            Debug.LogError($"[������ �ε� ����] ���: {prefabPath}");
            return;
        }
        GameObject unit = Instantiate(prefab, new Vector3(randomTile.gridPosition.x, randomTile.gridPosition.y, -1), Quaternion.identity);
        //Ÿ���̶� �����̶� ��ġ�� ��찡 �߻��ؼ� z���� -1�� ����

        UnitStatus status = unit.GetComponent<UnitStatus>();
        if (status != null)
        {
            status.Initialize(unitdata, randomTile.gridPosition);
            Debug.Log($"[���� ����] {status.unitName} ��ġ: ({status.posX}, {status.posY})");
        }

        randomTile.isOccupied = true;
        randomTile.currentUnit = unit;
    }
}
