using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSeller : MonoBehaviour
{
    private UnitStatus target;
    public UnitSpawner spawner;

    public void SetTarget(UnitStatus unit)
    {
        target = unit;
    }

    public void SellSelectedUnit()
    {
        if (target == null) return;

        int sellGold = target.sellGold;
        ResourceManager.Instance.AddResource(sellGold);

        //RoundTimer timer = FindObjectOfType<RoundTimer>();
        //string currentTime = timer != null ? timer.GetFormattedTime() : "--:--";
        string gradeName = target.gradeName;

        bool blockLog = (gradeName == "최고" || gradeName == "초월");

        if (!blockLog)
        {
            //string unitName = GetKoreanName(target.unitClass);

            //string timeColor = "#BBBBBB";
            //string gainColor = "#66FF66";
            //string gradeColor = GetGradeColor(gradeName);
            //string whiteColor = "#FFFFFF";

            //string logMessage =
            //    $"<color={timeColor}>[{currentTime}]</color> " +
            //    $"유닛 판매(<color={gainColor}>자원 획득 +{sellGold}</color>): " +
            //    $"<color={gradeColor}>{gradeName}</color> 등급 " +
            //    $"<color={whiteColor}>{unitName}</color> 유닛 판매";

            //GameLogManager.Instance.AddLog(logMessage);
        }

        Debug.Log($"{target.unitName} 판매. 골드 +{sellGold}");

        DragAndDrop drag2 = target.GetComponent<DragAndDrop>();
        if (drag2 != null && drag2.currentTile != null)
        {
            drag2.currentTile.isOccupied = false;
            drag2.currentTile.currentUnit = null;
        }


        UnitGrade grade = target.unitGrade;
        bool isExchange = grade == UnitGrade.Supreme || grade == UnitGrade.Transcendent;

        // 판매 직전 장착 아이템이 있으면 먼저 해제해서 슬롯으로 반환
        if (target.isEquipItem && !string.IsNullOrEmpty(target.equippedItemID))
        {
            UnitInfoManager.Instance.itemRemover.RemoveEquippedItem();
        }

        if (isExchange)
        {
            if (spawner == null)
            {
                Debug.LogError("[Exchange] spawner가 인스펙터에 연결되지 않았습니다.");
                return;
            }
            // 교환 전 정보 백업
            string beforeUnitID = target.unitID;
            UnitGrade beforeGrade = target.unitGrade;

            //  교환 추가 비용 계산
            int extraCost = (grade == UnitGrade.Supreme) ? 50 : 150;
            // 총 교환 비용
            //int totalCost = (beforeGrade == UnitGrade.Supreme) ? 100 : 200;

            int totalCost = (grade == UnitGrade.Supreme) ? 100 : 200;
            if (ResourceManager.Instance.currentResource < totalCost)
            {
                Debug.LogWarning("[교환 실패] 자원 부족");
                return;
            }

            if (grade == UnitGrade.Supreme)
                extraCost = 50;        // 100 - 50
            else if (grade == UnitGrade.Transcendent)
                extraCost = 150;       // 200 - 50

            if (!ResourceManager.Instance.TryUseResource(extraCost))
            {
                Debug.LogWarning("[교환 실패] 자원 부족");
                return;
            }

            // 아이템 장착 중이면 먼저 해제해서 슬롯으로 반환
            if (target.isEquipItem && !string.IsNullOrEmpty(target.equippedItemID))
            {
                if (UnitInfoManager.Instance == null || UnitInfoManager.Instance.itemRemover == null)
                {
                    Debug.LogError("[Exchange] UnitInfoManager/itemRemover 연결 안 됨");
                    return;
                }
                UnitInfoManager.Instance.itemRemover.RemoveEquippedItem();
            }

            // 교환용 unitID 결정
            string unitID = GetExchangeUnitID(grade);
            if (string.IsNullOrEmpty(unitID))
            {
                Debug.LogError("[Exchange] unitID 생성 실패");
                return;
            }

            // 기존 타일 비우기
            DragAndDrop drag = target.GetComponent<DragAndDrop>();
            if (drag != null && drag.currentTile != null)
            {
                drag.currentTile.isOccupied = false;
                drag.currentTile.currentUnit = null;
            }

            // 기존 유닛 제거
            Destroy(target.gameObject);
            target = null;

            // 랜덤 스폰 (자리 상관 없음)
            var spawned = spawner.SpawnUnit(unitID);
            if (spawned == null)
                Debug.LogWarning($"[Exchange] SpawnUnit returned null (unitID={unitID})");


            if (spawned != null)
            {
                //string afterUnitID = spawned.unitID; // 예: "Mage6"

                //string log =
                //    $"[{currentTime}] " +
                //    $"{beforeUnitID} → {afterUnitID} " +
                //    $"(자원 -{totalCost})";

                //GameLogManager.Instance.AddLog(log);

                UnitInfoManager.Instance?.ShowInfo(spawned);
            }



            return;
        }

        Destroy(target.gameObject);
        target = null;
    }

    private string GetExchangeUnitID(UnitGrade grade)
    {
        string[] pool = null;

        if (grade == UnitGrade.Supreme)
        {
            pool = new string[]
            {
            "Archer5",
            "Mage5",
            "Siege5",
            "Buffer5"
            };
        }
        else if (grade == UnitGrade.Transcendent)
        {
            pool = new string[]
            {
            "Archer6",
            "Mage6",
            "Siege6",
            "Buffer6"
            };
        }

        if (pool == null || pool.Length == 0)
            return null;

        return pool[Random.Range(0, pool.Length)];
    }

    private string GetKoreanName(UnitClass unitClass)
    {
        switch (unitClass)
        {
            case UnitClass.Archer: return "궁수";
            case UnitClass.Mage: return "마법사";
            case UnitClass.Siege: return "공성병";
            case UnitClass.Buffer: return "버퍼";
            default: return "알 수 없음";
        }
    }

    private string GetGradeColor(string gradeName)
    {
        switch (gradeName)
        {
            case "초급": return "#DDDDDD";
            case "중급": return "#00FFFF";
            case "고급": return "#4AFF3C";
            case "서사": return "#FFD700";
            case "최고": return "#FF66FF";
            case "초월": return "#FF4444";
            default: return "#FFFFFF";
        }
    }
}
