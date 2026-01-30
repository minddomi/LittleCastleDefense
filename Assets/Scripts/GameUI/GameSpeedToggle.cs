using UnityEngine;
using UnityEngine.UI;

public class GameSpeedToggle : MonoBehaviour
{
    public TMPro.TextMeshProUGUI speedText; // "1x", "2x", "3x" 표시용 (선택)

    private readonly float[] speeds = { 1f, 2f, 3f };
    private int currentIndex = 0;

    private void Start()
    {
        ApplySpeed();
    }

    public void ToggleSpeed()
    {
        currentIndex++;
        if (currentIndex >= speeds.Length)
            currentIndex = 0;

        ApplySpeed();
    }

    private void ApplySpeed()
    {
        float speed = speeds[currentIndex];
        Time.timeScale = speed;
        Time.fixedDeltaTime = 0.02f * speed;

        if (speedText != null)
            speedText.text = $"{speed:0}x";

        Debug.Log($"[GameSpeed] {speed}x");
    }
}
