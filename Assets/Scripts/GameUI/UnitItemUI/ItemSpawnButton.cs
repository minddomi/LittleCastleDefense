using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnButton : MonoBehaviour
{
    [SerializeField] private ItemSlotManager itemSlotManager;

    public void OnClickSpawn()
    {
        GameObject prefab = Resources.Load<GameObject>("Item/Item1");

        if (prefab == null)
        {
            Debug.LogError("Resources/Item/Item1 프리팹을 찾을 수 없습니다.");
            return;
        }

        itemSlotManager.TrySpawnItem(prefab);
    }
}
