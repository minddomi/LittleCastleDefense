using System.Linq;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject inputBlocker;
    private TutorialStepPanel[] steps;
    private int i = -1;

    void Start()
    {
        steps = FindObjectsOfType<TutorialStepPanel>(true)
            .OrderBy(s => s.stepIndex)
            .ToArray();

        foreach (var s in steps) s.gameObject.SetActive(false);
        Next();
    }

    public void Next()
    {
        // 이전 스텝 정리
        if (i >= 0 && i < steps.Length)
        {
            var prev = steps[i];
            if (prev.isEventStep && prev.condition != null)
                prev.condition.End();

            prev.gameObject.SetActive(false);
        }

        // 다음 스텝 진입
        i++;
        if (i >= steps.Length)
        {
            if (inputBlocker != null) inputBlocker.SetActive(false);
            TutorialInputLock.Locked = false;
            return;
        }

        var cur = steps[i];
        cur.gameObject.SetActive(true);

        //이 스텝의 옵션대로 잠금 on/off
        bool lockInput = cur.lockOtherUIAndWorld;

        if (inputBlocker != null)
            inputBlocker.SetActive(lockInput);

        // 월드 클릭 잠금도 같이
        TutorialInputLock.Locked = lockInput;

        cur.onEnter?.Invoke();

        if (cur.isEventStep && cur.condition != null)
            cur.condition.Begin();
    }

    // 패널 Button OnClick에서 연결
    public void OnPanelClicked()
    {
        var cur = Current();
        if (cur == null) return;

        if (cur.clickToNext) Next();
    }

    void Update()
    {
        var cur = Current();
        if (cur == null) return;

        if (cur.isEventStep && cur.condition != null && cur.condition.IsMet)
            Next();
    }

    TutorialStepPanel Current()
    {
        if (i < 0 || i >= steps.Length) return null;
        return steps[i];
    }
}
