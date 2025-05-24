using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllySelectable : MonoBehaviour, ISelectable
{
    public UnitMetadata meta;

    public void OnSelected()
    {
        //Debug.Log("���õ�");
        InfoUIManager.Instance.Show(meta);
    }

    public void OnDeselected()
    {
       // Debug.Log("��������");
        InfoUIManager.Instance.Hide();
    }
}
