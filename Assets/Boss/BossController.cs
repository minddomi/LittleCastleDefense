using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BossSystem;

public class BossController : EnemyUnit
{
    [Header("Config")]
    [SerializeField] private BossStats stats;
    [SerializeField] private List<BossAbilitySO> abilities = new();

    private readonly List<IBossRuntimeAbility> _runtimes = new();
    private List<Tile> _allTiles;

    public float MaxHealth => stats != null ? stats.maxHealth : 500f;

    // 이벤트
    public event Action OnCycle;
    public event Action<int> OnObstaclesSpawned;

    // Boss API
    public void Heal(float amount) => health = Mathf.Min(health + amount, MaxHealth);

    public IEnumerable<Tile> GetPlaceableTiles() =>
        _allTiles.Where(t => t != null && t.IsPlaceable);

    public void RaiseObstaclesSpawned(int count) => OnObstaclesSpawned?.Invoke(count);

    // ===== Unity Lifecycle =====
    // EnemyUnit에 Awake가 없어도 괜찮음. base.Awake() 호출 X
    private void Awake()
    {
        // EnemyUnit이 health를 기반으로 max/UI를 잡는 구조라면 선세팅이 안전
        health = MaxHealth;
    }

    // EnemyUnit.Start()가 private/프로텍트여서 base.Start() 호출하면 에러 → 아예 Start를 선언하지 않음!
    // 대신 OnEnable -> Init 코루틴으로 초기화 타이밍을 한 프레임 뒤로 미룸.
    private void OnEnable()
    {
        StartCoroutine(InitAfterStart());
    }

    private IEnumerator InitAfterStart()
    {
        // EnemyUnit의 Start가 먼저 돌도록 한 프레임 대기
        yield return null;

        _allTiles = FindObjectsOfType<Tile>().ToList();

        _runtimes.Clear();
        foreach (var so in abilities)
        {
            if (so == null) continue;
            var runtime = so.CreateRuntime();
            runtime.Init(this);
            _runtimes.Add(runtime);
        }

        // 사이클 루프 시작
        StartCoroutine(CycleLoop());
    }

    private IEnumerator CycleLoop()
    {
        var wait = new WaitForSeconds(stats != null ? stats.cycleSeconds : 20f);
        while (true)
        {
            yield return wait;
            OnCycle?.Invoke();
            foreach (var r in _runtimes) r.OnCycle();
        }
    }

    private void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < _runtimes.Count; i++)
            _runtimes[i].Tick(dt);
    }

    private void OnDestroy()
    {
        foreach (var r in _runtimes) r.Dispose();
        _runtimes.Clear();
    }
}
