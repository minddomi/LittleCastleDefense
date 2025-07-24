using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRandomID : MonoBehaviour
{
    public string GetRandomUnitID()
    {
        UnitClass unitClass = (UnitClass)Random.Range(0, 4);
        float gradeRand = Random.Range(0f, 100f);
        int grade;

        if (gradeRand < 50f) grade = 1;
        else if (gradeRand < 83f) grade = 2;
        else if (gradeRand < 93f) grade = 3;
        else if (gradeRand < 98f) grade = 4;
        else if (gradeRand < 99.5f) grade = 5;
        else grade = 6;

        string unitID = $"{unitClass}{grade}";
        return unitID;
    }
}
