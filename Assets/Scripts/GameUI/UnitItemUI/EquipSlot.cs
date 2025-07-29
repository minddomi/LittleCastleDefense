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

        string path = $"Item/{unitStatus.equippedItemID}"; // 예: "Item/Item1"
        GameObject prefab = Resources.Load<GameObject>(path);

        if (prefab != null)
        {
            equippedItemObject = Instantiate(prefab, transform);
            equippedItemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            //Debug.Log($"[SetTarget] {unitStatus.unitName}의 장착 아이템 {unitStatus.equippedItemID} 표시됨");
        }
        else
        {
            Debug.LogWarning($"[SetTarget] 경로 {path} 에 해당하는 프리팹이 없습니다.");
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        var draggedItem = eventData.pointerDrag?.GetComponent<ItemDragAndDrop>();
        if (draggedItem == null || unitStatus == null) return;

        if (unitStatus.isEquipItem)
        {
            draggedItem.ReturnToOrigin();
            Debug.Log($"{unitStatus.unitName}은 이미 아이템을 장착 중입니다.");
            return;
        }

        // 기존 아이템 제거
        if (equippedItemObject != null)
            Destroy(equippedItemObject);

        // 장착 데이터 저장
        unitStatus.isEquipItem = true;
        unitStatus.equippedItemID = draggedItem.gameObject.name.Replace("(Clone)", "");

        // 프리팹 로드 후 시각적으로 띄움
        string path = $"Item/{unitStatus.equippedItemID}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            equippedItemObject = Instantiate(prefab, transform);
            equippedItemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }

        // 드래그된 원본 제거
        Destroy(draggedItem.gameObject);

        Debug.Log($"[장착] {unitStatus.unitName} → {unitStatus.equippedItemID}");
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