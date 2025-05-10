using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 9;
    public int height = 9;

    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileObj.transform.parent = transform; // GridManager�� �ڽ����� �־� ����

                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y); // Ÿ�� ��ǥ�� ������(���� ��ġ�� Ȱ����)
            }
        }
    }
} 
