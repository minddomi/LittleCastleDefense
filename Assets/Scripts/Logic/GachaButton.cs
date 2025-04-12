using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GachaButton : MonoBehaviour
{
    public UnitSpawner unitSpawner;

    public void OnClickGacha()
    {
        var (unitClass, unitGrade) = UnitProbability.GetRandomUnitCombination();

        unitSpawner.SpawnUnit(unitClass, unitGrade);

    }
}
