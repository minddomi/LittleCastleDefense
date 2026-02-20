using UnityEngine;

public class Cond_PanelActive : TutorialCondition
{
    [Header("Target Panel (Canvas UI)")]
    [SerializeField] private GameObject targetPanel;

    public override bool IsMet
    {
        get
        {
            if (targetPanel == null) return false;
            return targetPanel.activeInHierarchy; // 실제로 켜져있는지
        }
    }

    public override void Begin()
    {
        if (targetPanel == null)
            Debug.LogError("[Tutorial] Cond_PanelActive: targetPanel이 인스펙터에 연결되지 않았습니다.");
    }

    public override void End()
    {
        // 
    }
}