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
            Debug.LogError("SpawnButton: idGenerator �Ǵ� unitSpawner�� ������� �ʾҽ��ϴ�.");
            return;
        }

        string unitID = idGenerator.GetRandomUnitID();
        unitSpawner.SpawnUnit(unitID);

        EventSystem.current.SetSelectedGameObject(null);
    }
}
