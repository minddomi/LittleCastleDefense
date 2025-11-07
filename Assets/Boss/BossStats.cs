using UnityEngine;

[CreateAssetMenu(menuName = "Boss/Boss Stats", fileName = "BossStats")]
public class BossStats : ScriptableObject
{
    [Header("Health")]
    public float maxHealth = 500f;

    [Header("Cycle")]
    public float cycleSeconds = 20f;   // 20초마다 사이클 트리거
}