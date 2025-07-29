using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSlotManager : MonoBehaviour
{
    public RectTransform dragCanvas;
    public List<ItemSlot> slots;

    public void TrySpawnItem(GameObject prefab)
    {
        foreach (var slot in slots)
        {
            if (!slot.IsOccupied)
            {
                GameObject item = Instantiate(prefab, slot.transform); // 슬롯에 귀속
                item.transform.localPosition = Vector3.zero; // 정중앙 배치

                // 슬롯 상태 업데이트
                slot.SetOccupied(true);

                // 아이템 드래그 스크립트에 슬롯 정보 전달
                var dragScript = item.GetComponent<ItemDragAndDrop>();
                if (dragScript != null)
                {
                    dragScript.currentSlot = slot;
                }

                return;
            }
        }
    }
}
