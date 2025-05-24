using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySelectable : MonoBehaviour, ISelectable
{
    public UnitMetadata meta;

    public void OnSelected()
    {
        //Debug.Log("선택됨");
        InfoUIManager.Instance.Show(meta);
    }

    public void OnDeselected()
    {
       // Debug.Log("선택해제");
        InfoUIManager.Instance.Hide();
    }
}
