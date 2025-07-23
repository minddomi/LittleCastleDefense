using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSeller : MonoBehaviour
{
    private UnitStatus target;

    public void SetTarget(UnitStatus unit)
    {
        target = unit;
    }

    public void SellSelectedUnit()
    {
        if (target == null) return;

        int sellGold = target.sellGold;
        ResourceManager.Instance.AddResource(sellGold);

        Debug.Log($"{target.unitName} ÆÇ¸Å. °ñµå +{sellGold}");

        DragAndDrop drag = target.GetComponent<DragAndDrop>();
        if (drag != null && drag.currentTile != null)
        {
            drag.currentTile.isOccupied = false;
            drag.currentTile.currentUnit = null;
        }

        Destroy(target.gameObject);
        target = null;

        UnitInfoManager.Instance.infoPanel.SetActive(false);
    }
}
