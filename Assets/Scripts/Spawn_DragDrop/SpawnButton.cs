using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class SpawnButton : MonoBehaviour
{
    public GetRandomID idGenerator;
    public UnitSpawner unitSpawner;

    public void SpawnRandomUnitByButton()
    {
        if (idGenerator == null || unitSpawner == null)
        {
            Debug.LogError("SpawnButton: idGenerator 또는 unitSpawner가 연결되지 않았습니다.");
            return;
        }

        string unitID = idGenerator.GetRandomUnitID();
        UnitStatus spawnedUnit = unitSpawner.SpawnUnit(unitID);

        if (spawnedUnit != null)
        {
            // 시간
            RoundTimer timer = FindObjectOfType<RoundTimer>();
            string currentTime = timer != null ? timer.GetFormattedTime() : "--:--";

            // 유닛 정보
            string gradeName = spawnedUnit.gradeName;
            string unitName = spawnedUnit.unitClass.ToString();
            int cost = 50;

            // 통일 색상
            string mainColor = "#77DDFF";   // 등급 + 종족 공통 강조색
            string costColor = "#FF7777";   // 자원 소비 강조색
            string timeColor = "#BBBBBB";   // 시간 회색

            // 로그 조립
            string logMessage =
                $"<color={timeColor}>[{currentTime}]</color> " +
                $"랜덤 유닛 뽑기 (<color={costColor}>자원 소비 -{cost}</color>): " +
                $"<color={mainColor}>{gradeName}" +
                $" <color=#FFFFFF>등급</color>" + 
                $"{unitName}</color> 유닛 획득!";

            GameLogManager.Instance.AddLog(logMessage);

            if (UnitInfoManager.Instance != null)
                UnitInfoManager.Instance.ShowInfo(spawnedUnit);
        }

        EventSystem.current.SetSelectedGameObject(null);
    }
}
