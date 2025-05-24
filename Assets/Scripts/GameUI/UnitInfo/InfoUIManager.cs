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

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(UnitMetadata meta)
    {
        //Debug.Log("����â ǥ��");
        panel.SetActive(true);

        classText.text = $"����: {meta.unitClass}";
        gradeText.text = $"���: {meta.unitGrade}";
        attackText.text = $"���ݷ�: {meta.baseAttack} (+{meta.bonusAttack})";
    }

    public void Hide()
    {
        //Debug.Log("����â ����");
        panel.SetActive(false);
    }
}