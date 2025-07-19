using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition; // �� Ÿ���� ���� �󿡼� ������ ��ǥ
    public bool isOccupied = false;
    //������ Ÿ���� ���� ������ ���� (DragAndDrop, UnitSpawner���� Ȱ���)

    public GameObject currentUnit;
    // ���� �� Ÿ�� ���� �ö� �ִ� ���� (DragAndDrop ���� Ȱ��)

    public void SetCurrentUnit(GameObject unit)
    {
        currentUnit = unit;
        isOccupied = (unit != null);

        if (unit != null)
        {
            UnitStatus status = unit.GetComponent<UnitStatus>();
            if (status != null)
            {
                status.posX = gridPosition.x;
                status.posY = gridPosition.y;
            }
        }
    }

}