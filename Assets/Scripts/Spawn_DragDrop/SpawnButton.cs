using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GetRandomID idGenerator;
    public UnitSpawner unitSpawner;

    public void SpawnRandomUnitByButton()
    {
        if (idGenerator == null || unitSpawner == null)
        {
            Debug.LogError("SpawnButton: idGenerator 또는 unitSpawner가 연결되지 않았습니다.");
            return;
        }

        string unitID = idGenerator.GetRandomUnitID();
        UnitStatus spawnedUnit = unitSpawner.SpawnUnit(unitID);

        if (spawnedUnit != null)
        {
            if (UnitInfoManager.Instance != null)
                UnitInfoManager.Instance.ShowInfo(spawnedUnit);
        }
        EventSystem.current.SetSelectedGameObject(null);
    }
}
