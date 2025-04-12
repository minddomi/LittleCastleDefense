using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 offset;
    public Tile currentTile;

    private static readonly Vector3 TileDepth = new Vector3(0, 0, -1); //��� y�� -1�� ����

    private void Start()
    {
        Vector2 myPos = transform.position; //Ŭ���� ������ ���� ��ǥ ����

        foreach (Tile tile in FindObjectsOfType<Tile>()) 
        {
            if ((Vector2)tile.transform.position == myPos) // ������ġ�� Ÿ����ġ�� �������� ã��
            {
                SetTile(tile); // currentTile���� tile�� �����ϰ�, Ÿ�ϰ� ������ ���� ����
                break;
            }
        }
    }

    private void OnMouseDown()
    {
        // Ŭ�� �� ������ ���콺 �߾����� �ٷ� �̵�
        Vector3 mousePos = GetMouseWorldPos();
        mousePos.z = -2f;
        transform.position = mousePos;

        offset = Vector3.zero;
    }

    private void OnMouseDrag()
    {
        Vector3 dragPos = GetMouseWorldPos() + offset; 
        dragPos.z = -2f; //z�� -2�� �����ؼ� ���ֳ��� ��ġ�°� ����
        transform.position = Vector3.Lerp(transform.position, dragPos, Time.deltaTime * 40f);
        //������ ���� ��ġ���� ���콺 ��ġ���� �ε巴�� ���󰡰���

    }

    private void OnMouseUp()
    {
        Tile targetTile = FindClosestTile();

        if (targetTile == null) // ����ߴµ� Ÿ���� ������
        {
            GoHome();
            return;
        }

        if (targetTile == currentTile) // ���� �ڸ��� ��ӽ�
        {
            GoHome();
            return;
        }

        if (!targetTile.isOccupied) // ���������� ���� Ÿ�Ͽ� ��ӽ�
        {
            MoveToTile(targetTile);
        }
        else if (targetTile.currentUnit != gameObject) //�ٸ� ������Ʈ�� �ִ� Ÿ�� ���� ��ӽ�
        {
            SwapWith(targetTile.currentUnit.GetComponent<DragAndDrop>());
        }
        else
        {
            GoHome();
        }
    }

    private void GoHome() // ���� ��ġ�� �ǵ��� ���� �Լ�
    {
        transform.position = currentTile.transform.position + TileDepth;
    }

    private void MoveToTile(Tile newTile) // ���� Ÿ�ϰ� ������ ���� ���ο� Ÿ�Ϸ� ��ġ �̵� �� ����
    {
        currentTile.isOccupied = false;
        currentTile.currentUnit = null;

        transform.position = newTile.transform.position + TileDepth;
        SetTile(newTile);
    }

    private void SwapWith(DragAndDrop other) // �ٸ� ���ְ� ��ġ�� Ÿ������ ��ȯ
    {
        Tile originalTile = currentTile;
        Tile targetTile = other.currentTile;

        transform.position = targetTile.transform.position + TileDepth;
        other.transform.position = originalTile.transform.position + TileDepth;

        SetTile(targetTile);
        other.SetTile(originalTile);
    }

    private void SetTile(Tile tile) //���ְ� Ÿ�ϰ� ���� ���踦 �������ִ� �Լ�
    {
        currentTile = tile;
        tile.isOccupied = true;
        tile.currentUnit = gameObject;
    }

    private Tile FindClosestTile() // ���� ����� Tile�� ã���ִ� �Լ�
    {
        Tile closestTile = null; // ���� ����� Ÿ���� ������ ����
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
