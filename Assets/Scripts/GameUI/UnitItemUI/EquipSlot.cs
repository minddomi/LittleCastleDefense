using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : MonoBehaviour, IDropHandler
{
    public UnitStatus unitStatus;
    private GameObject equippedItemObject;

    public void SetTarget(UnitStatus status)
    {
        unitStatus = status;

        if (equippedItemObject != null)
        {
            Destroy(equippedItemObject);
            equippedItemObject = null;
        }

        if (unitStatus == null || !unitStatus.isEquipItem || string.IsNullOrEmpty(unitStatus.equippedItemID))
            return;

        string path = $"Item/{unitStatus.equippedItemID}"; // ��: "Item/Item1"
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            equippedItemObject = Instantiate(prefab, transform);
            equippedItemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //Debug.Log($"[SetTarget] {unitStatus.unitName}�� ���� ������ {unitStatus.equippedItemID} ǥ�õ�");
        }
        else
        {
            Debug.LogWarning($"[SetTarget] ��� {path} �� �ش��ϴ� �������� �����ϴ�.");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedItem = eventData.pointerDrag?.GetComponent<ItemDragAndDrop>();
        if (draggedItem == null || unitStatus == null) return;

        if (unitStatus.isEquipItem)
        {
            draggedItem.ReturnToOrigin();
            Debug.Log($"{unitStatus.unitName}�� �̹� �������� ���� ���Դϴ�.");
            return;
        }

        // ���� ������ ����
        if (equippedItemObject != null)
            Destroy(equippedItemObject);

        // ���� ������ ����
        unitStatus.isEquipItem = true;
        unitStatus.equippedItemID = draggedItem.gameObject.name.Replace("(Clone)", "");

        // ������ �ε� �� �ð������� ���
        string path = $"Item/{unitStatus.equippedItemID}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            equippedItemObject = Instantiate(prefab, transform);
            equippedItemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // �巡�׵� ���� ����
        Destroy(draggedItem.gameObject);

        Debug.Log($"[����] {unitStatus.unitName} �� {unitStatus.equippedItemID}");
    }

    private void ClearSlot()
    {
        if (equippedItemObject != null)
        {
            Destroy(equippedItemObject);
            equippedItemObject = null;
        }
    }

    private void ResetRectTransform(GameObject obj)
    {
        RectTransform rt = obj.GetComponent<RectTransform>();
        if (rt)
        {
            rt.anchoredPosition = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
        }
    }
}