using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class EnemyCounter : MonoBehaviour
{
    [Header("적 컨테이너")]
    public Transform enemiesContainer;

    [Header("UI")]
    public TMP_Text enemyCountText;

    [Header("트리거 설정")]
    public int triggerCount = 10;
    private bool triggered = false;

    public UnityEvent onEnemyCountReached;

    void Update()
    {
        if (enemiesContainer == null) return;

        int count = enemiesContainer.childCount;

        // UI 표시
        if (enemyCountText != null)
        {
            enemyCountText.text = $"Enemies: {count}";
        }

        // 특정 개수 도달 시 이벤트
        if (!triggered && count >= triggerCount)
        {
            triggered = true;
            onEnemyCountReached?.Invoke();
        }
    }
}