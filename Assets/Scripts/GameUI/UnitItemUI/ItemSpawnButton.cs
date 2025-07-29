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
            Debug.LogError("Resources/Item/Item1 �������� ã�� �� �����ϴ�.");
            return;
        }

        itemSlotManager.TrySpawnItem(prefab);
    }
}
