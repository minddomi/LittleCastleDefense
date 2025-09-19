using UnityEngine;
using TMPro;

public class MergeSlot : MonoBehaviour
{
    public TextMeshProUGUI unitIdText;
    public UnitStatus assignedUnit;
    public bool isSelected = false;

    [SerializeField] private GameObject highlightImage; // 인스펙터에서 드래그 연결 가능
    [SerializeField] private Transform unitPreviewRoot;

    private GameObject currentPreview;

    private void Awake()
    {
        // 초기화 시 하이라이트 비활성화
        if (highlightImage != null)
            highlightImage.SetActive(false);
    }

    public void SetUnit(UnitStatus unit)
    {
        assignedUnit = unit;

        // ID 표시
        if (unitIdText != null)
        {
            unitIdText.text = unit.unitID;
            unitIdText.gameObject.SetActive(true);
        }

        // prefabPath에서 프리팹 로드
        GameObject prefab = Resources.Load<GameObject>(unit.UIPath);
        if (prefab == null)
        {
            Debug.LogWarning($"Prefab not found at path: {unit.UIPath}");
            return;
        }

        // 이전 프리뷰 제거
        if (currentPreview != null)
            Destroy(currentPreview);

        // 새 프리뷰 생성

        currentPreview = Instantiate(prefab, unitPreviewRoot);
        currentPreview.transform.localPosition = Vector3.zero;
        currentPreview.transform.localScale = Vector3.one;
        currentPreview.transform.localRotation = Quaternion.identity;
        currentPreview.transform.localPosition = new Vector3(0, 0, 0);

        // 기능 끄기
        var ally = currentPreview.GetComponent<AllyUnit>();
        var status = currentPreview.GetComponent<UnitStatus>();
        var drag = currentPreview.GetComponent<DragAndDrop>();
        if (ally) ally.enabled = false;
        if (status) status.enabled = false;
        if (drag) drag.enabled = false;
    }

    public void Clear()
    {
        unitIdText.text = "";
        assignedUnit = null;
        isSelected = false;
        SetHighlight(false);

        if (currentPreview != null)
        {
            Destroy(currentPreview);
            currentPreview = null;
        }
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
