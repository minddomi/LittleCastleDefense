using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitInfoManager : MonoBehaviour
{
    public static UnitInfoManager Instance;

    public GameObject infoPanel;
    public TMPro.TextMeshProUGUI classText;
    public TMPro.TextMeshProUGUI gradeText;
    public TMPro.TextMeshProUGUI attackText;

    public UnitSeller unitSeller;
    public EquipSlot equipSlot;

    private void Awake()
    {
        Instance = this;
        infoPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider == null || hit.collider.GetComponent<UnitStatus>() == null)
            {
                infoPanel.SetActive(false);
            }
        }
    }

    public void ShowInfo(UnitStatus status)
    {
        classText.text = "Class: " + status.unitClass;
        gradeText.text = "Grade: " + status.gradeName;
        attackText.text = "Attack: " + status.attackPower.ToString();

        unitSeller.SetTarget(status);
        equipSlot.SetTarget(status);
       
        infoPanel.SetActive(true);
    }
}
