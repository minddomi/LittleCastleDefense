using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public int CurrentShield { get; private set; }
    Coroutine expireCo;

    public void AddShield(int amount, float duration)
    {
        CurrentShield += amount;
        if (expireCo != null) StopCoroutine(expireCo);
        expireCo = StartCoroutine(Expire(duration));
    }

    IEnumerator Expire(float t)
    {
        yield return new WaitForSeconds(t);
        CurrentShield = 0;
        expireCo = null;
    }

    // 데미지 흡수: 남은 데미지는 ref로 반환
    public void TryAbsorb(ref float damage)
    {
        if (CurrentShield <= 0 || damage <= 0f) return;
        int use = Mathf.Min(CurrentShield, Mathf.CeilToInt(damage));
        CurrentShield -= use;
        damage -= use;
        if (CurrentShield < 0) CurrentShield = 0;
    }
}

