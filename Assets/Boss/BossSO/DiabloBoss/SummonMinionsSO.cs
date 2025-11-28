using UnityEngine;
using BossSystem;

[CreateAssetMenu(menuName = "Boss/Ability/Summon Minions", fileName = "SummonMinionsSO")]
public class SummonMinionsSO : BossAbilitySO
{
    [Header("Summon Settings")]
    public EnemyUnit minionPrefab;          // 소환할 적 유닛 프리팹(EnemyUnit 붙어있는 것)
    [Min(1)] public int countPerTrigger = 3;
    [Min(0.1f)] public float intervalSeconds = 30f;

    [Header("Overrides")]
    public EnemySize sizeOverride = EnemySize.Medium;
    public int baseResourceRewardOverride = 0;  // 소환몹 처치 보상(원하면 0~소량)
    public float healthOverride = 1f;           // 체력 1로 덮어쓰기
    public float scatterRadius = 0.6f;          // 보스 주변 산개 반경

    public override IBossRuntimeAbility CreateRuntime() => new RT(this);

    private class RT : BossRuntimeAbilityBase
    {
        private readonly SummonMinionsSO so;
        private float timer;

        public RT(SummonMinionsSO s) { so = s; }

        public override void Tick(float dt)
        {
            timer += dt;
            if (timer < so.intervalSeconds) return;
            timer = 0f;

            if (so.minionPrefab == null || Controller == null) return;

            for (int i = 0; i < so.countPerTrigger; i++)
                SpawnOne();
        }

        private void SpawnOne()
        {
            // 보스 주변 랜덤 위치
            Vector2 rnd = Random.insideUnitCircle * so.scatterRadius;
            Vector3 pos = Controller.transform.position + new Vector3(rnd.x, rnd.y, 0f);

            EnemyUnit m = Object.Instantiate(so.minionPrefab, pos, Quaternion.identity);

            // 소환몹 파라미터 덮어쓰기
            m.size = so.sizeOverride;
            m.health = so.healthOverride;               // Start() 이전에 health 세팅 → maxHealth도 1로 초기화됨
            m.baseResourceReward = so.baseResourceRewardOverride;

            // 보스와 같은 웨이포인트를 쓰게 하면 자연스럽게 전진
            if (Controller.waypoints != null && Controller.waypoints.Length > 0)
                m.waypoints = Controller.waypoints;
        }
    }
}

