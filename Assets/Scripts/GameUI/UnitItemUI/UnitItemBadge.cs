using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnitStatus))]
public class UnitItemBadge : MonoBehaviour
{
    [SerializeField] string badgeResourcePath = "Item/Item_UI/ItemBadge"; // 배지 프리팹
    [SerializeField] string iconResourceRoot = "Item/Item_png";          // PNG 경로
    [SerializeField] Vector3 worldOffset = new Vector3(0.15f, 0.35f, 0f);

    [Header("Icon render")]
    [SerializeField] int iconSortingOrder = 101;   // BG보다 높게
    [SerializeField] string iconSortingLayer = "Default";
    [SerializeField] Vector3 iconLocalPos = Vector3.zero;
    [SerializeField] Vector3 iconLocalScale = Vector3.one * 0.5f; // 배지 안에 맞게 축소

    UnitStatus status;
    GameObject badge;          // 배지 인스턴스(자식)
    GameObject iconGO;         // 우리가 붙이는 PNG 스프라이트 오브젝트
    string currentId = null;   // 마지막으로 표시한 아이템ID

    void Awake()
    {
        status = GetComponent<UnitStatus>();
    }

    void Start()
    {
        EnsureBadge();
        ForceRefresh();
    }

    void LateUpdate()
    {
        if (!badge) return;
        badge.transform.position = transform.position + worldOffset;
        badge.transform.rotation = Quaternion.identity;

        // 상태 폴링(간단 구현)
        UpdateBadge();
    }

    /// 장착/해제 직후 수동으로도 호출 가능
    public void ForceRefresh() => UpdateBadge(true);

    void UpdateBadge(bool force = false)
    {
        if (!badge) return;

        bool equipped = status.isEquipItem && !string.IsNullOrEmpty(status.equippedItemID);

        if (badge.activeSelf != equipped) badge.SetActive(equipped);
        if (!equipped)
        {
            currentId = null;
            if (iconGO) iconGO.SetActive(false);
            return;
        }

        // 같은 아이템이면 스킵
        if (!force && status.equippedItemID == currentId) return;
        currentId = status.equippedItemID;

        // 기존 아이콘 삭제/비활성
        if (iconGO != null) Destroy(iconGO);

        // PNG 로드
        string path = $"{iconResourceRoot}/{currentId}";
        Sprite sprite = Resources.Load<Sprite>(path);

        // 자식 오브젝트 생성해서 스프라이트 붙이기
        iconGO = new GameObject("ItemPNG");
        iconGO.transform.SetParent(badge.transform, false);
        iconGO.transform.localPosition = iconLocalPos;
        iconGO.transform.localScale = iconLocalScale;

        var sr = iconGO.AddComponent<SpriteRenderer>();
        sr.sprite = sprite; // 없으면 null로 빈칸
        sr.sortingLayerName = iconSortingLayer;
        sr.sortingOrder = iconSortingOrder;

        iconGO.SetActive(true);
    }

    void EnsureBadge()
    {
        if (badge != null) return;

        // 이미 자식으로 있을 수도 있으니 먼저 찾아본다
        var found = transform.Find("ItemBadge");
        if (found) badge = found.gameObject;
        else
        {
            var prefab = Resources.Load<GameObject>(badgeResourcePath);
            if (prefab == null)
            {
                Debug.LogError($"[UnitItemBadge] Prefab not found: Resources/{badgeResourcePath}");
                return;
            }
            badge = Instantiate(prefab, transform.position + worldOffset, Quaternion.identity, transform);
            badge.name = "ItemBadge";
        }

        badge.SetActive(false);
    }
}
