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
                GameObject item = Instantiate(prefab, slot.transform); // ���Կ� �ͼ�
                item.transform.localPosition = Vector3.zero; // ���߾� ��ġ

                // ���� ���� ������Ʈ
                slot.SetOccupied(true);

                // ������ �巡�� ��ũ��Ʈ�� ���� ���� ����
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
