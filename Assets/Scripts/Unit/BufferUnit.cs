using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BufferGrade
{
    Basic,        // 초급
    Intermediate, // 중급
    Advanced,     // 고급
    Epic          // 서사
}

[RequireComponent(typeof(AllyUnit))]

public class BufferUnit : MonoBehaviour
{
    public BufferGrade grade = BufferGrade.Basic;

    private AllyUnit self;
    private float interval = 1.5f;
    private float timer = 0f;

    private readonly Dictionary<BufferGrade, BuffData> buffTable = new()
    {
        { BufferGrade.Basic,     new BuffData { range = 1.5f, powerMultiplier = 1f } }
    };

    // Start is called before the first frame update
    void Start()
    {
        self = GetComponent<AllyUnit>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            Debug.Log("[Buffer] ApplyBuff() 실행 시도됨"); // ← 추가
            ApplyBuff();
            timer = 0f;
        }
    }

    void ApplyBuff()
    {
        BuffData data = buffTable[grade];
        AllyUnit[] allies = FindObjectsOfType<AllyUnit>();

        foreach (AllyUnit ally in allies)
        {
            if (ally == self) continue;
            if (ally.unitType == AllyUnit.UnitType.Buffer) continue; // 다른 버퍼에게는 버프 적용 X

            float dist = Vector3.Distance(transform.position, ally.transform.position);
            if (dist <= data.range)
            {
                float buffValue = self.attackPower * data.powerMultiplier;
                ally.RemoveBuffFrom(this); // 중복 방지
                ally.AddBuffFrom(this, buffValue);
                Debug.Log($"[Buffer] {self.name} → {ally.name}: +{buffValue} 공격력 버프 적용");
            }
            else
            {
                ally.RemoveBuffFrom(this); // 범위에서 벗어나면 버프 제거
            }
        }
    }

    struct BuffData
    {
        public float range;
        public float powerMultiplier;
    }

}
