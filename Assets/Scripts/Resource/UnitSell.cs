using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSell : MonoBehaviour
{
    private UnitMetadata currentMeta;
    private bool isSelected = false; // 선택된 유닛만 D 키 감지

    public void SetTarget(UnitMetadata meta)
    {
        currentMeta = meta;
        isSelected = true;
    }

    public void Deselect()
    {
        isSelected = false;
    }

    void Update()
    {
        if (isSelected && Input.GetKeyDown(KeyCode.D))
        {
            OnClickSell();
        }
    }

    public void OnClickSell()
    {
        if (currentMeta != null)
        {
            var drag = currentMeta.GetComponent<DragAndDrop>();
            if (drag != null)
            {
                Tile tile = drag.currentTile;
                if (tile != null)
                {
                    tile.isOccupied = false;
                    tile.currentUnit = null;
                }

                drag.MarkAsDead(); // 드래그 방지
            }

            int value = ResourceManager.Instance.GetSellValue(currentMeta.unitGrade);
            ResourceManager.Instance.AddResource(value);
            Destroy(currentMeta.gameObject);
            InfoUIManager.Instance.Hide();
        }
        else
        {
            Debug.LogWarning("판매 실패");
        }
    }

}
