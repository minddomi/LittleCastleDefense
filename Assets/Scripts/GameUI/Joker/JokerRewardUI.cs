using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class JokerRewardUI : MonoBehaviour
{
    [Header("패널 연결")]
    public GameObject colorRewardPanel;
    public GameObject blackRewardPanel;

    [Header("컬러 조커 버튼 세트")]
    public Button[] colorRewardButtons;
    public TMP_Text[] colorButtonLabels;

    [Header("흑백 조커 버튼 세트")]
    public Button[] blackRewardButtons;
    public TMP_Text[] blackButtonLabels;

    [Header("아이템 (컬러 조커용)")]
    public string[] allItemPaths = {
        "Item/BalanceScale",
        "Item/BrokenChalice",
        "Item/ChaosCrystal",
        "Item/HungrySword",
        "Item/InfiniteBranch",
        "Item/MadnessTome"
    };

    //  경로 대신 unitID로 관리 (CSV와 일치)
    [Header("서사 유닛 ID (흑백 조커용)")]
    public string[] epicUnitIDs = {
        "Mage4",   // 마법사(서사)
        "Archer4", // 궁수(서사)
        "Siege4",  // 공성병(서사)
        "Buffer4"  // 버퍼(서사)
    };

    private string[] selectedPaths = new string[4];
    private string currentJokerType = "컬러 조커"; // 로그 색상 구분용만 사용

    private void Awake()
    {
        if (colorRewardPanel) colorRewardPanel.SetActive(false);
        if (blackRewardPanel) blackRewardPanel.SetActive(false);
    }

    // 외부에서 "컬러 조커" / "흑백 조커" 전달
    public void ShowRandomRewards(string jokerType)
    {
        currentJokerType = jokerType;

        if (colorRewardPanel) colorRewardPanel.SetActive(false);
        if (blackRewardPanel) blackRewardPanel.SetActive(false);

        if (jokerType == "흑백 조커")
        {
            if (blackRewardPanel)
            {
                blackRewardPanel.SetActive(true);
                ShowBlackRewards_ByID();
            }
        }
        else
        {
            if (colorRewardPanel)
            {
                colorRewardPanel.SetActive(true);
                ShowColorRewards();
            }
        }
    }

    // 컬러 조커용 (아이템은 기존대로 경로 사용)
    private void ShowColorRewards()
    {
        List<string> pool = new List<string>(allItemPaths);
        for (int i = 0; i < colorRewardButtons.Length; i++)
        {
            int idx = Random.Range(0, pool.Count);
            string path = pool[idx];
            pool.RemoveAt(idx);

            string displayName = path.Replace("Item/", "");
            if (i < colorButtonLabels.Length)
                colorButtonLabels[i].text = displayName;

            colorRewardButtons[i].onClick.RemoveAllListeners();
            colorRewardButtons[i].onClick.AddListener(() =>
            {
                // 아이템은 경로 로드
                SpawnItemByPath(path);
                ClosePanelsAndLog(displayName);
            });
        }
    }

    //  흑백 조커용 (unitID로 스폰)
    private void ShowBlackRewards_ByID()
    {
        // 버튼 4개에 각각 고정 ID 매핑
        for (int i = 0; i < blackRewardButtons.Length; i++)
        {
            if (i >= epicUnitIDs.Length)
            {
                blackRewardButtons[i].gameObject.SetActive(false);
                continue;
            }

            string unitID = epicUnitIDs[i];
            string displayName = ToEpicKoreanName(unitID); // 보기용 텍스트

            if (i < blackButtonLabels.Length)
                blackButtonLabels[i].text = displayName;

            blackRewardButtons[i].onClick.RemoveAllListeners();
            blackRewardButtons[i].onClick.AddListener(() =>
            {
                SpawnEpicUnitByID(unitID);     // ← 경로 X, ID로 스폰
                ClosePanelsAndLog(displayName);
            });
        }
    }

    private void ClosePanelsAndLog(string rewardName)
    {
        if (colorRewardPanel) colorRewardPanel.SetActive(false);
        if (blackRewardPanel) blackRewardPanel.SetActive(false);
        AddJokerUseLog(rewardName); // 컬러/흑백 모두 로그
    }

    // ----------------- 스폰 로직 -----------------

    // 아이템은 기존처럼 Resources 경로로
    private void SpawnItemByPath(string prefabPath)
    {
        var prefab = Resources.Load<GameObject>(prefabPath); // ← 여기서 경로 틀리면 못찾음:contentReference[oaicite:1]{index=1}
        if (prefab == null)
        {
            Debug.LogError($"{prefabPath} 프리팹을 찾을 수 없습니다.");
            return;
        }

        var slotManager = FindObjectOfType<ItemSlotManager>();
        if (slotManager != null)
            slotManager.TrySpawnItem(prefab);
    }

    //  유닛은 unitID로 스폰 (CSV/DB 기반 시스템과 자연스럽게 연동)
    private void SpawnEpicUnitByID(string unitID)
    {
        var spawner = FindObjectOfType<UnitSpawner>();
        if (spawner != null)
        {
            // 프로젝트에 있는 시그니처에 맞춰 호출하세요.
            // 보통 SpawnUnit(string unitID) 형태일 가능성이 큼.
            spawner.SpawnUnit(unitID);
        }
        else
        {
            Debug.LogWarning("[JokerRewardUI] UnitSpawner가 씬에 없습니다.");
        }
    }

    // 보기용 텍스트: unitID -> "서사 마법사" 등
    private string ToEpicKoreanName(string unitID)
    {
        // 간단 매핑 (필요시 CSV를 참고해도 됨)
        if (unitID.Contains("Mage")) return "서사 마법사";
        if (unitID.Contains("Archer")) return "서사 궁수";
        if (unitID.Contains("Siege")) return "서사 공성병";
        if (unitID.Contains("Buffer")) return "서사 버퍼";
        return unitID;
    }

    // ----------------- 로그 -----------------
    private void AddJokerUseLog(string rewardName)
    {
        RoundTimer timer = FindObjectOfType<RoundTimer>();
        string currentTime = timer != null ? timer.GetFormattedTime() : "--:--";

        string timeColor = "#BBBBBB";
        string jokerColor = (currentJokerType == "흑백 조커") ? "#AAAAAA" : "#FF66FF";
        string rewardColor = "#77DDFF";

        string logMsg =
            $"<color={timeColor}>[{currentTime}]</color> " +
            $"<color={jokerColor}>{currentJokerType}</color> 사용 " +
            $"(<color={rewardColor}>{rewardName} 획득</color>)";

        GameLogManager.Instance?.AddLog(logMsg);
    }
}
