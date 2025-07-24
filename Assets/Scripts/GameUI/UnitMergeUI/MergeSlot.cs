using UnityEngine;
using TMPro;

public class MergeSlot : MonoBehaviour
{
    public TextMeshProUGUI unitIdText;
    public UnitStatus assignedUnit;
    public bool isSelected = false;

    [SerializeField] private GameObject highlightImage; // 인스펙터에서 드래그 연결 가능

    private void Awake()
    {
        // 초기화 시 하이라이트 비활성화
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
