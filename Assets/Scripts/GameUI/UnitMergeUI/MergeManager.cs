using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeManager : MonoBehaviour
{
    public static MergeManager Instance;

    public MergeSlot[] mergeSlots;
    private MergeSlot currentSelectedSlot;

    private void Awake()
    {
        Instance = this;
    }

 
    public void SelectSlot(MergeSlot slot)
    {
        foreach (var s in mergeSlots)
        {
            s.isSelected = false;
            s.SetHighlight(false);
        }

        currentSelectedSlot = slot;
        slot.isSelected = true;
        slot.SetHighlight(true);
    }

   
    public void TryAssignUnit(UnitStatus unit)
    {
        if (!unit.canMerge)
        {
            return;
        }

        if (currentSelectedSlot != null && currentSelectedSlot.isSelected)
        {
            currentSelectedSlot.SetUnitID(unit.unitID);
            currentSelectedSlot.assignedUnit = unit;
            unit.canMerge = false;

            currentSelectedSlot.SetHighlight(false);
            currentSelectedSlot.isSelected = false;
            currentSelectedSlot = null;
        }
    }

    
    public void OnClickMergeButton()
    {
        ResetAllSlots();
    }

    public void ResetAllSlots()
    {
        foreach (var slot in mergeSlots)
        {
            if (slot.assignedUnit != null)
            {
                slot.assignedUnit.canMerge = true;
            }

            slot.Clear();
            slot.SetHighlight(false);
        }
        currentSelectedSlot = null;
    }
}

