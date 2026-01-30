using UnityEngine;
using BossSystem;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Boss/Ability/Freeze Random Allies", fileName = "FreezeRandomAlliesSO")]
public class FreezeRandomAlliesSO : BossAbilitySO
{
    [Min(1)] public int countPerTrigger = 5;
    [Min(0.1f)] public float freezeDuration = 5f;
    [Min(0.1f)] public float intervalSeconds = 20f;

    public override IBossRuntimeAbility CreateRuntime() => new RT(this);

    class RT : BossRuntimeAbilityBase
    {
        readonly FreezeRandomAlliesSO so; float timer;
        public RT(FreezeRandomAlliesSO s) { so = s; }

        public override void Tick(float dt)
        {
            timer += dt;
            if (timer < so.intervalSeconds) return;
            timer = 0f;

            var all = new List<FreezableAgent>(Object.FindObjectsOfType<FreezableAgent>());
            if (all.Count == 0) return;
            Shuffle(all);

            int n = Mathf.Min(so.countPerTrigger, all.Count);
            for (int i = 0; i < n; i++)
                if (all[i] && all[i].isActiveAndEnabled)
                    all[i].ApplyFreeze(so.freezeDuration);
        }

        static void Shuffle<T>(IList<T> a)
        {
            for (int i = 0; i < a.Count; i++) { int j = Random.Range(i, a.Count); (a[i], a[j]) = (a[j], a[i]); }
        }
    }
}
