using UnityEngine;
using TMPro;

public class MergeSlot : MonoBehaviour
{
    public TextMeshProUGUI unitIdText;
    public UnitStatus assignedUnit;
    public bool isSelected = false;

    [SerializeField] private GameObject highlightImage; // �ν����Ϳ��� �巡�� ���� ����

    private void Awake()
    {
        // �ʱ�ȭ �� ���̶���Ʈ ��Ȱ��ȭ
        if (highlightImage != null)
            highlightImage.SetActive(false);
    }

    public void SetUnitID(string id)
    {
        unitIdText.text = id;
    }

    public void Clear()
    {
        unitIdText.text = "";
        assignedUnit = null;
        isSelected = false;
        SetHighlight(false);
    }

    public void OnClickSlot()
    {
        MergeManager.Instance.SelectSlot(this);
    }

    public void SetHighlight(bool on)
    {
        if (highlightImage != null)
            highlightImage.SetActive(on);
    }
}
