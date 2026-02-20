using UnityEngine;
using UnityEngine.UI;

public class Cond_ButtonClickCount : TutorialCondition
{
    [Header("Target Button")]
    [SerializeField] private Button targetButton;

    [Header("Required Clicks")]
    [SerializeField] private int requiredCount = 2;

    private int count;

    public override bool IsMet => count >= requiredCount;

    public override void Begin()
    {
        count = 0;

        if (targetButton == null)
        {
            Debug.LogError("[Tutorial] Cond_ButtonClickCount: targetButton이 비어있습니다.");
            return;
        }

        targetButton.onClick.AddListener(OnClicked);
    }

    public override void End()
    {
        if (targetButton != null)
            targetButton.onClick.RemoveListener(OnClicked);
    }

    private void OnClicked()
    {
        count++;
        // 디버그
        // Debug.Log($"[Tutorial] SpawnButton Click Count: {count}/{requiredCount}");
    }
}
