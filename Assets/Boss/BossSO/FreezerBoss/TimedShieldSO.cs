using UnityEngine;
using BossSystem;

[CreateAssetMenu(menuName = "Boss/Ability/Timed Shield", fileName = "TimedShieldSO")]
public class TimedShieldSO : BossAbilitySO
{
    [Min(1)] public int shieldAmount = 100;
    [Min(0.1f)] public float shieldDuration = 10f;
    [Min(0.1f)] public float intervalSeconds = 90f;

    public override IBossRuntimeAbility CreateRuntime() => new RT(this);

    class RT : BossRuntimeAbilityBase
    {
        readonly TimedShieldSO so; float timer; ShieldController shield;

        public RT(TimedShieldSO s) { so = s; }

        public override void Init(BossController c)
        {
            base.Init(c);
            if (!c.TryGetComponent(out shield))
                shield = c.gameObject.AddComponent<ShieldController>();
        }

        public override void Tick(float dt)
        {
            timer += dt;
            if (timer < so.intervalSeconds) return;
            timer = 0f;
            shield?.AddShield(so.shieldAmount, so.shieldDuration);
        }
    }
}

