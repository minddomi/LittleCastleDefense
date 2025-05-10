using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    public Tile currentTile;

    private static readonly Vector3 TileDepth = new Vector3(0, 0, -1); //모든 y값 -1로 설정

    private void Start()
    {
        Vector2 myPos = transform.position; //클릭한 유닛의 월드 좌표 저장

        foreach (Tile tile in FindObjectsOfType<Tile>()) 
        {
            if ((Vector2)tile.transform.position == myPos) // 유닛위치와 타일위치가 같은곳을 찾고
            {
                SetTile(tile); // currentTile값을 tile로 설정하고, 타일과 유닛을 서로 연결
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        // 클릭 시 유닛을 마우스 중앙으로 바로 이동
        Vector3 mousePos = GetMouseWorldPos();
        mousePos.z = -2f;
        transform.position = mousePos;

        offset = Vector3.zero;
    }

    private void OnMouseDrag()
    {
        Vector3 dragPos = GetMouseWorldPos() + offset; 
        dragPos.z = -2f; //z값 -2로 설정해서 유닛끼리 겹치는걸 방지
        transform.position = Vector3.Lerp(transform.position, dragPos, Time.deltaTime * 40f);
        //유닛의 현재 위치에서 마우스 위치까지 부드럽게 따라가게함

    }

    private void OnMouseUp()
    {
        Tile targetTile = FindClosestTile();

        if (targetTile == null) // 드롭했는데 타일이 없을시
        {
            GoHome();
            return;
        }

        if (targetTile == currentTile) // 같은 자리에 드롭시
        {
            GoHome();
            return;
        }

        if (!targetTile.isOccupied) // 점유중이지 않은 타일에 드롭시
        {
            MoveToTile(targetTile);
        }
        else if (targetTile.currentUnit != gameObject) //다른 오브젝트가 있는 타일 위에 드롭시
        {
            SwapWith(targetTile.currentUnit.GetComponent<DragAndDrop>());
        }
        else
        {
            GoHome();
        }
    }

    private void GoHome() // 본래 위치로 되돌아 가는 함수
    {
        transform.position = currentTile.transform.position + TileDepth;
    }

    private void MoveToTile(Tile newTile) // 기존 타일과 연결을 끊고 새로운 타일로 위치 이동 후 연결
    {
        currentTile.isOccupied = false;
        currentTile.currentUnit = null;

        transform.position = newTile.transform.position + TileDepth;
        SetTile(newTile);
    }

    private void SwapWith(DragAndDrop other) // 다른 유닛과 위치와 타일정보 교환
    {
        Tile originalTile = currentTile;
        Tile targetTile = other.currentTile;

        transform.position = targetTile.transform.position + TileDepth;
        other.transform.position = originalTile.transform.position + TileDepth;

        SetTile(targetTile);
        other.SetTile(originalTile);
    }

    private void SetTile(Tile tile) //유닛과 타일간 점유 관계를 연결해주는 함수
    {
        currentTile = tile;
        tile.isOccupied = true;
        tile.currentUnit = gameObject;
    }

    private Tile FindClosestTile() // 가장 가까운 Tile을 찾아주는 함수
    {
        Tile closestTile = null; // 가장 가까운 타일을 저장할 변수
        float minDist = float.MaxValue;

        foreach (var tile in FindObjectsOfType<Tile>())
        {
            float dist = Vector2.Distance(tile.transform.position, transform.position);
            if (dist < 0.5f && dist < minDist) 
            {
                minDist = dist; 
                closestTile = tile;
            }
        }

        return closestTile;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
        return mousePos;
    }
}
