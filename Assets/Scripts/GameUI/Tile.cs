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
}