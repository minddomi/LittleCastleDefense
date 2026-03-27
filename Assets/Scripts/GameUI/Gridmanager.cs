using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 9;
    public int height = 9;

    //public GameObject overlayPrefab; // 이것만 추가

    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x, y, 0);
                GameObject tileObj = Instantiate(tilePrefab, pos, Quaternion.identity);
                tileObj.transform.parent = transform; // GridManager에 자식으로 넣어 정리

                Tile tile = tileObj.GetComponent<Tile>();
                tile.gridPosition = new Vector2Int(x, y); // 타일 좌표룰 설정함(유닛 배치에 활용함)

                //Instantiate(overlayPrefab, tileObj.transform); //  
            }
        }
    }
} 
