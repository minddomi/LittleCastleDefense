using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSeller : MonoBehaviour
{
    private UnitStatus target;

    public void SetTarget(UnitStatus unit)
    {
        target = unit;
    }

    public void SellSelectedUnit()
    {
        if (target == null) return;

        int sellGold = target.sellGold;
        ResourceManager.Instance.AddResource(sellGold);

        RoundTimer timer = FindObjectOfType<RoundTimer>();
        string currentTime = timer != null ? timer.GetFormattedTime() : "--:--";

        //  등급 / 이름 / 종족 정보
        string gradeName = target.gradeName;               // 예: "서사"
        string unitName = GetKoreanName(target.unitClass); // 예: "버퍼"

        //  색상 구성
        string timeColor = "#BBBBBB";
        string gainColor = "#66FF66"; // 자원 획득은 초록색 강조
        string gradeColor = GetGradeColor(gradeName);
        string whiteColor = "#FFFFFF";

        //  로그 조합
        string logMessage =
            $"<color={timeColor}>[{currentTime}]</color> " +
            $"유닛 판매(<color={gainColor}>자원 획득 +{sellGold}</color>): " +
            $"<color={gradeColor}>{gradeName}</color> 등급 " +
            $"<color={whiteColor}>{unitName}</color> 유닛 판매";

        GameLogManager.Instance.AddLog(logMessage);

        Debug.Log($"{target.unitName} 판매. 골드 +{sellGold}");

        DragAndDrop drag = target.GetComponent<DragAndDrop>();
        if (drag != null && drag.currentTile != null)
        {
            drag.currentTile.isOccupied = false;
            drag.currentTile.currentUnit = null;
        }

        Destroy(target.gameObject);
        target = null;

        UnitInfoManager.Instance.infoPanel.SetActive(false);
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
            case "일반": return "#DDDDDD";
            case "고급": return "#00FFFF";
            case "희귀": return "#4AFF3C";
            case "영웅": return "#FFD700";
            case "전설": return "#FF66FF";
            case "초월": return "#FF4444";
            default: return "#FFFFFF";
        }
    }
}
