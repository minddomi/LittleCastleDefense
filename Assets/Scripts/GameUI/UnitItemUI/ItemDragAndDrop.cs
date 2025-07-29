// ItemDragAndDrop.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    //public string itemID;
    public bool isEquipped = false;
    public ItemSlot currentSlot;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private Canvas canvas;
    private Transform originalParent;
    private Vector3 originalPosition;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvas = GetComponentInParent<Canvas>();

        //itemID = gameObject.name;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isEquipped)
        {
            Debug.Log("[드래그 차단] 이 아이템은 장착 중이라 드래그할 수 없습니다.");
            return;
        }

        originalParent = transform.parent; // 기억해둔다
        transform.SetParent(canvas.transform); // Canvas로 이동
        transform.SetAsLastSibling();
        canvasGroup.blocksRaycasts = false;

        if (currentSlot != null)
        {
            currentSlot.SetOccupied(false);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 드롭된 위치의 슬롯 감지
        ItemSlot droppedSlot = eventData.pointerEnter?.GetComponentInParent<ItemSlot>();

        if (droppedSlot != null && !droppedSlot.IsOccupied)
        {
            transform.SetParent(droppedSlot.transform);
            transform.localPosition = Vector3.zero;
            droppedSlot.SetOccupied(true);
            currentSlot = droppedSlot;
        }
        else
        {
            // 원래 슬롯으로 복귀
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
            if (originalParent != null)
            {
                ItemSlot fallbackSlot = originalParent.GetComponent<ItemSlot>();
                if (fallbackSlot != null)
                {
                    fallbackSlot.SetOccupied(true);
                    currentSlot = fallbackSlot;
                }
            }
        }
    }

    public void ReturnToOrigin()
    {
        transform.SetParent(originalParent);
        transform.position = originalPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}