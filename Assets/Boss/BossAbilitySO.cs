using UnityEngine;
using BossSystem;

public abstract class BossAbilitySO : ScriptableObject
{
    // 각 SO가 자신만의 런타임 객체를 만들어 반환
    public abstract IBossRuntimeAbility CreateRuntime();
}