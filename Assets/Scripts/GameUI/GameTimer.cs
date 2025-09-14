using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool useUnscaledTime = false; // true = 일시정지 중에도 카운트

    private float startTime;

    void Start()
    {
        startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
    }

    void Update()
    {
        float elapsed = (useUnscaledTime ? Time.unscaledTime : Time.time) - startTime;

        int hours = Mathf.FloorToInt(elapsed / 3600f);
        int minutes = Mathf.FloorToInt((elapsed % 3600f) / 60f);
        int seconds = Mathf.FloorToInt(elapsed % 60f);

        timerText.text = $"{hours:00}:{minutes:00}:{seconds:00}";
    }
}
