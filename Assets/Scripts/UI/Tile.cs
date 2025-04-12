using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int gridPosition; // 이 타일이 격자 상에서 가지는 좌표
    public bool isOccupied = false;
    //유닛이 타일을 점유 중인지 여부 (DragAndDrop, UnitSpawner에서 활용됨)

    public GameObject currentUnit;
    // 현재 이 타일 위에 올라가 있는 유닛 (DragAndDrop 에서 활용)
}