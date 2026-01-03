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
    public TMPro.TextMeshProUGUI sellButtonText;

    public UnitSeller unitSeller;
    public EquipSlot equipSlot;

    public ItemRemover itemRemover;

    private UnitStatus currentTarget;

    public GameObject sellButton;

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

        if (infoPanel.activeSelf && currentTarget != null)
        {
            attackText.text = "Attack: " + currentTarget.TotalAttackPower.ToString("F1");
        }

    }

    public void ShowInfo(UnitStatus status)
    {
        currentTarget = status;

        classText.text = "Class: " + status.unitClass;
        gradeText.text = "Grade: " + status.gradeName;
        attackText.text = "Attack: " + status.TotalAttackPower.ToString("F1");

        unitSeller.SetTarget(status);
        equipSlot.SetTarget(status);
        itemRemover.SetTarget(status);

        bool isExchange =
            status.unitGrade == UnitGrade.Supreme ||
            status.unitGrade == UnitGrade.Transcendent;

        // 버튼 텍스트 변경
        if (sellButtonText != null)
        {
            sellButtonText.text = isExchange ? "교환" : "판매";
        }

        bool isBlocked =
            status.unitClass == UnitClass.Joker;
            //status.unitGrade == UnitGrade.Transcendent ||
            //status.unitGrade == UnitGrade.Supreme;
        
        sellButton.SetActive(!isBlocked);


        infoPanel.SetActive(true);
    }

}
