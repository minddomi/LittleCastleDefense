using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnitProbability
{
    public static (UnitClass, UnitGrade) GetRandomUnitCombination()
    {
        UnitClass unitClass = GetRandomClass();
        UnitGrade unitGrade = GetRandomGrade();
        return (unitClass, unitGrade);
    }

    public static UnitClass GetRandomClass()
    {
        int rand = Random.Range(0, 3);
        return (UnitClass)rand;
    }

    public static UnitGrade GetRandomGrade()
    {
        float rand = Random.Range(0f, 100f);

        if (rand < 50f) return UnitGrade.Basic;
        else if (rand < 83f) return UnitGrade.Intermediate;
        else if (rand < 93f) return UnitGrade.Advanced;
        else if (rand < 98f) return UnitGrade.Epic;
        else if (rand < 99.5f) return UnitGrade.Transcendent;
        else return UnitGrade.Supreme;
    }
}