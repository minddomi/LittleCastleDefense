using UnityEngine;
using BossSystem;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Boss/Ability/Tree Boss", fileName = "TreeBossSO")]
public class TreeBossSO : BossAbilitySO
{
    [Header("Obstacle")]
    public GameObject obstaclePrefab;     // ObstacleBlocker 프리팹
    [Min(1)] public int obstaclesPerCycle = 3;
    [Min(0.1f)] public float obstacleLifeTime = 20f;

    [Header("Heal")]
    [Range(0f, 1f)]
    public float healPercentOfMaxOnSuccess = 0.03f; // 사이클에 1개 이상 생성되면 최대체력의 3% 회복

    public override IBossRuntimeAbility CreateRuntime() => new Runtime(this);

    private class Runtime : BossRuntimeAbilityBase
    {
        private readonly TreeBossSO _so;
        public Runtime(TreeBossSO so) { _so = so; }

        public override void OnCycle()
        {
            if (Controller == null || _so.obstaclePrefab == null) return;

            var candidates = new List<Tile>(Controller.GetPlaceableTiles());
            if (candidates.Count == 0) return;

            Shuffle(candidates);

            int spawnCount = Mathf.Min(_so.obstaclesPerCycle, candidates.Count);
            int spawned = 0;

            for (int i = 0; i < spawnCount; i++)
            {
                var tile = candidates[i];
                if (TrySpawnOn(tile)) spawned++;
            }

            if (spawned > 0 && _so.healPercentOfMaxOnSuccess > 0f)
            {
                float heal = Controller.MaxHealth * _so.healPercentOfMaxOnSuccess;
                Controller.Heal(heal);
            }
        }

        private bool TrySpawnOn(Tile tile)
        {
            if (tile == null || !tile.IsPlaceable) return false;

            var go = Object.Instantiate(_so.obstaclePrefab);
            var blocker = go.GetComponent<ObstacleBlocker>();
            if (blocker != null)
            {
                blocker.lifeTime = _so.obstacleLifeTime;
                blocker.Init(tile); // 내부에서 tile.SetBlocked(true) → 수명 끝 해제
                return true;
            }

            // 안전망: 컴포넌트가 없으면 수동 차단·해제
            tile.SetBlocked(true);
            Object.Destroy(go, _so.obstacleLifeTime);
            Controller.StartCoroutine(UnblockLater(tile, _so.obstacleLifeTime));
            return true;
        }

        private System.Collections.IEnumerator UnblockLater(Tile tile, float t)
        {
            yield return new WaitForSeconds(t);
            if (tile) tile.SetBlocked(false);
        }

        private static void Shuffle<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}
