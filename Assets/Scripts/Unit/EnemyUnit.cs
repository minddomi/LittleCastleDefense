using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EnemySize { Small, Medium, Large }

public class EnemyUnit : MonoBehaviour
{
    [Header("Stats")]
    public EnemySize size = EnemySize.Medium;
    public float health = 100f;
    public float speed = 3.5f;
    public Transform[] waypoints;

    [Header("Resource Reward")]
    [Tooltip("적 기본 처치 보상(자원)")]
    public int baseResourceReward = 10;

    [Header("UI")]
    public GameObject healthBarPrefab;
    private EnemyHealthBar healthBarInstance;

    private int currentWaypointIndex = 0;
    private bool isFrozen = false;
    private float freezeTimer = 0f;

    private float maxHealth; // UI 갱신용

    public static event System.Action<AllyUnit> OnEnemyKilledBy; // 굶줄인 검

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;

        if (healthBarPrefab != null)
        {
            GameObject hb = Instantiate(healthBarPrefab, Vector3.zero, Quaternion.identity);
            hb.transform.SetParent(GameObject.Find("Canvas").transform, false);

            healthBarInstance = hb.GetComponent<EnemyHealthBar>();
            healthBarInstance.enemyTransform = this.transform;
            healthBarInstance.UpdateHealth(health, maxHealth);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 정지 상태 처리
        if (isFrozen)
        {
            freezeTimer -= Time.deltaTime;
            if (freezeTimer <= 0f)
            {
                isFrozen = false;
                Debug.Log($"{name} 정지 해제됨");
            }
            return;
        }

        if (waypoints == null || waypoints.Length == 0) return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 dir = (target.position - transform.position).normalized;
        transform.position += (Vector3)(dir * speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
                currentWaypointIndex = 0;
        }
    }

    public void Freeze(float duration)
    {
        isFrozen = true;
        freezeTimer = duration;
        Debug.Log($"{name} 정지됨 ({duration}초)");
    }

    // 기존 호환: 공격자 정보 없이도 동작(보너스 없음)
    public void TakeDamage(float damage)
    {
        InternalTakeDamage(damage, null);
    }

    // 새 오버로드: 공격자(AllyUnit)를 받아 막타 보너스 처리 가능
    public void TakeDamage(float damage, AllyUnit attacker)
    {
        InternalTakeDamage(damage, attacker);
    }

    private void InternalTakeDamage(float damage, AllyUnit attacker)
    {
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        if (healthBarInstance != null)
            healthBarInstance.UpdateHealth(health, maxHealth);

        if (health <= 0f)
            Die(attacker);
    }

    private void Die(AllyUnit killer)
    {
        Debug.Log($"{gameObject.name} died.");

        // 막타 보너스(자원) 계산: killer가 가진 활성 버프 중 resourceBonus 합산
        int extraResource = 0;
        if (killer != null)
        {
            // AllyUnit 쪽에 활성 버프를 열람할 수 있는 접근자 필요 (아래 패치 참고)
            foreach (var buff in killer.GetActiveBuffs()) // IEnumerable<BufferUnit.BuffData>
                extraResource += buff.resourceBonus;
        }

        // 자원 지급
        int totalReward = baseResourceReward + Mathf.Max(0, extraResource);
        if (ResourceManager.Instance != null)
        {
            ResourceManager.Instance.AddResource(totalReward);
            Debug.Log($"[Reward] Base {baseResourceReward} + Bonus {extraResource} = +{totalReward} resource");
        }
        else
        {
            Debug.LogWarning("[Reward] ResourceManager.Instance가 없습니다.");
        }

        if (healthBarInstance != null)
            Destroy(healthBarInstance.gameObject);

        OnEnemyKilledBy?.Invoke(killer); // 막타 유닛 알림

        Destroy(gameObject);
    }
}
