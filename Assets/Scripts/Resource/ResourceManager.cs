using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    public int currentResource = 100;
    public TMP_Text resourceText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        UpdateResourceUI();
    }

    void UpdateResourceUI()
    {
        resourceText.text = $"Resource: {currentResource}";
    }

    public bool TryUseResource(int amount)
    {
        if (currentResource >= amount)
        {
            currentResource -= amount;
            UpdateResourceUI();
            return true;
        }
        return false;
    }

    public int GetSellValue(UnitGrade grade)
    {
        switch (grade)
        {
            case UnitGrade.Basic: return 8;
            case UnitGrade.Intermediate: return 16;
            case UnitGrade.Advanced: return 32;
            case UnitGrade.Epic: return 64;
            case UnitGrade.Transcendent: return 128;
            case UnitGrade.Supreme: return 256;
            default: return 0;
        }
    }

    public void AddResource(int amount)
    {
        currentResource += amount;
        UpdateResourceUI();
    }
}