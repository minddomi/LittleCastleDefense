using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitUpgradeButton : MonoBehaviour
{
    [Header("버튼 정보")]
    public UnitClass unitClass; //  각 버튼마다 다르게 설정 (Archer, Mage, Siege, Buffer)

    [Header("UI 참조")]
    public TMP_Text titleText;  // (선택) 상단 이름
    public TMP_Text levelText;  // Lv. 표기
    public TMP_Text costText;   // Cost: 표기
    public Button button;       // 버튼 컴포넌트

    private void Awake()
    {
        // 중복 방지: 기존 리스너 제거 후 새로 등록
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClickUpgrade);
        }
    }

    private void Start()
    {
        RefreshUI();
        //button.onClick.AddListener(OnClickUpgrade);
    }

    private void OnClickUpgrade()
    {
        switch (unitClass)
        {
            case UnitClass.Archer:
                GlobalUpgradeManager.Instance.UpgradeArcher();
                break;
            case UnitClass.Mage:
                GlobalUpgradeManager.Instance.UpgradeMage();
                break;
            case UnitClass.Siege:
                GlobalUpgradeManager.Instance.UpgradeSiege();
                break;
            case UnitClass.Buffer:
                GlobalUpgradeManager.Instance.UpgradeBuffer();
                break;
        }
    }

    public void RefreshUI()
    {
        if (GlobalUpgradeManager.Instance == null) return;

        int level = GlobalUpgradeManager.Instance.GetLevel(unitClass);
        int cost = GlobalUpgradeManager.Instance.GetCost(unitClass);
        float bonus = GlobalUpgradeManager.Instance.GetBonus(unitClass);

        levelText.text = $"Lv. {level}  (+{bonus})";
        costText.text = $"Cost: {cost}";
    }

    // 강화 시 호출되는 외부용 UI 갱신
    public void RefreshUI(UnitClass type, int level, int nextCost, float bonus)
    {
        if (unitClass != type) return;

        levelText.text = $"Lv. {level}  (+{bonus})";
        costText.text = $"Cost: {nextCost}";
    }
}
