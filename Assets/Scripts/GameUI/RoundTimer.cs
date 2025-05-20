using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RoundTimer : MonoBehaviour
{
    public TMP_Text timeText;
    public TMP_Text roundText;

    public float roundDuration = 105f;
    public int maxRounds = 60;

    private float elapsedTime = 0f;
    private int currentRound = 1;

    void Update()
    {
        if (currentRound > maxRounds) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= roundDuration)
        {
            currentRound++;
            elapsedTime = 0f;
        }

        int timeLeft = Mathf.FloorToInt(roundDuration - elapsedTime);
        timeText.text = $"Time: {timeLeft}";
        roundText.text = $"Round: {currentRound}/{maxRounds}";
    }
}
