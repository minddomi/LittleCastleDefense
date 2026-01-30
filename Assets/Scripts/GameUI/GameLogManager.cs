using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections; // 코루틴용

public class GameLogManager : MonoBehaviour
{
    public static GameLogManager Instance;

    [Header("UI References")]
    public RectTransform contentParent;    // ScrollRect → Content
    public TMP_Text logTextPrefab;         // log_Text 프리팹
    public ScrollRect scrollRect;          // ScrollRect
    public int maxLogs = 30;               // 최대 로그 개수

    private readonly List<GameObject> logEntries = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddLog(string message)
    {
        //  1. 새 로그 생성
        TMP_Text newText = Instantiate(logTextPrefab, contentParent);
        newText.text = message;
        newText.gameObject.SetActive(true);

        logEntries.Add(newText.gameObject);

        //  2. 오래된 로그 삭제
        if (logEntries.Count > maxLogs)
        {
            Destroy(logEntries[0]);
            logEntries.RemoveAt(0);
        }

        //  3. 레이아웃 즉시 갱신
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(contentParent);

        //  4. 한 프레임 뒤에 스크롤 이동 (갱신 타이밍 보정)
        StartCoroutine(ScrollToBottomNextFrame());
    }

    private IEnumerator ScrollToBottomNextFrame()
    {
        yield return null; // 다음 프레임까지 대기 (LayoutRebuild 후)
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 0f;
    }
}
