using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenChaliceSpawnButton : MonoBehaviour
{
    [SerializeField] private ItemSlotManager itemSlotManager;

    public void OnClickSpawn()
    {
        GameObject prefab = Resources.Load<GameObject>("Item/BrokenChalice");

        if (prefab == null)
        {
            Debug.LogError("Resources/Item/BrokenChalice 프리팹을 찾을 수 없습니다.");
            return;
        }

        itemSlotManager.TrySpawnItem(prefab);
    }
}
