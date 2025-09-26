using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class JokerRewardUI : MonoBehaviour
{
    public static JokerRewardUI Instance;

    [Header("UI References")]
    public GameObject rewardPanel;
    public Button[] rewardButtons;
    public TMP_Text[] buttonLabels;

    [Header("Item Prefab Paths (Resources/Item/...)")]
    public string[] allItemPaths = {
        "Item/BalanceScale",
        "Item/BrokenChalice",
        "Item/ChaosCrystal",
        "Item/HungrySword",
        "Item/InfiniteBranch",
        "Item/MadnessTome"
    };

    private string[] selectedPaths = new string[4];

    private void Awake()
    {
        Instance = this;
        rewardPanel.SetActive(false);
    }

    public void ShowRandomRewards()
    {
        rewardPanel.SetActive(true);

        // 4개 랜덤 추출
        List<string> pool = new List<string>(allItemPaths);
        for (int i = 0; i < rewardButtons.Length; i++)
        {
            int idx = Random.Range(0, pool.Count);
            string path = pool[idx];
            pool.RemoveAt(idx);

            selectedPaths[i] = path;

            if (buttonLabels != null && i < buttonLabels.Length)
                buttonLabels[i].text = path.Replace("Item/", "");
            else
                rewardButtons[i].GetComponentInChildren<Text>().text = path.Replace("Item/", "");

            // 초기화
            int capturedIndex = i;
            rewardButtons[i].onClick.RemoveAllListeners();
            rewardButtons[i].onClick.AddListener(() => OnRewardSelected(capturedIndex));
        }
    }

    private void OnRewardSelected(int index)
    {
        string prefabPath = selectedPaths[index];
        SpawnItem(prefabPath);
        rewardPanel.SetActive(false);
    }

    private void SpawnItem(string prefabPath)
    {
        GameObject prefab = Resources.Load<GameObject>(prefabPath);
        if (prefab == null)
        {
            Debug.LogError($"{prefabPath} 프리팹을 찾을 수 없습니다.");
            return;
        }

        ItemSlotManager slotManager = FindObjectOfType<ItemSlotManager>();
        if (slotManager != null)
        {
            slotManager.TrySpawnItem(prefab);
            Debug.Log($"[보상 획득] {prefab.name} 생성 완료");
        }
    }
}