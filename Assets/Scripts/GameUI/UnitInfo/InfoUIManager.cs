using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoUIManager : MonoBehaviour
{
    public static InfoUIManager Instance;

    public GameObject panel;
    public TMP_Text classText;
    public TMP_Text gradeText;
    public TMP_Text attackText;

    public UnitSell sellButton;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }


    public void Show(UnitMetadata meta)
    {
        sellButton.SetTarget(meta);

        //Debug.Log("정보창 표시");
        panel.SetActive(true);

        classText.text = $"종족: {meta.unitClass}";
        gradeText.text = $"등급: {meta.unitGrade}";
        attackText.text = $"공격력: {meta.baseAttack} (+{meta.bonusAttack})";
    }

    public void Hide()
    {
        //Debug.Log("정보창 숨김");
        panel.SetActive(false);
    }
}