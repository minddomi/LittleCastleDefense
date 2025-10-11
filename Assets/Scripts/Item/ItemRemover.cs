using System.Collections;
using System.Collections.Generic;
// ItemRemover.cs
using UnityEngine;

public class ItemRemover : MonoBehaviour
{
    [SerializeField] private ItemSlotManager slotManager;  // ★ 인스펙터에 할당

    private UnitStatus target;
    public void SetTarget(UnitStatus unit) => target = unit;

    public void RemoveEquippedItem()
    {
        if (target == null) return;
        var ally = target.GetComponent<AllyUnit>();
        if (ally == null) return;

        // 0) 드랍할 아이템 ID 확보
        string removedId = target.equippedItemID;

        // 1) 장착 상태 끄기
        target.isEquipItem = false;
        target.equippedItemID = string.Empty;

        // 2) 효과 해제
        ItemEffectsManager.Instance?.Sync(ally, target);

        UnitInfoManager.Instance?.ShowInfo(target);

        // 3) UI 슬롯에 아이템 프리팹 생성
        SpawnItemToUISlot(removedId);
    }

    private void SpawnItemToUISlot(string itemId)
    {
        if (string.IsNullOrEmpty(itemId) || slotManager == null) return;

        // UI용 프리팹을 로드해야 함 (RectTransform 포함)
        // 예: Resources/ItemUI/BalanceScaleUI
        string path = $"Item/{itemId}";
        GameObject uiPrefab = Resources.Load<GameObject>(path);
        if (uiPrefab == null)
        {
            Debug.LogWarning($"[ItemRemover] UI 프리팹 로드 실패: {path}");
            return;
        }

        slotManager.TrySpawnItem(uiPrefab);  // ★ 여기로 연결!
    }
}

