using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour
{
    public static SelectionManager Instance;

    private ISelectable currentSelection;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // UI 위 클릭이면 무시
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                ISelectable selectable = hit.collider.GetComponent<ISelectable>();
                if (selectable != null)
                {
                    Select(selectable);
                    return;
                }
            }

            Deselect();
        }
    }

    public void Select(ISelectable target)
    {
        if (currentSelection != null && currentSelection != target)
        {
            currentSelection.OnDeselected();
        }

        currentSelection = target;
        currentSelection.OnSelected();
    }

    public void Deselect()
    {
        if (currentSelection != null)
        {
            currentSelection.OnDeselected();
            currentSelection = null;
        }
    }
}