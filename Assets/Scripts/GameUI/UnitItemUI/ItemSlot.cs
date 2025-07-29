using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour
{
    private bool isOccupied = false;
    public bool IsOccupied => isOccupied;

    public void SetOccupied(bool value) => isOccupied = value;
}
