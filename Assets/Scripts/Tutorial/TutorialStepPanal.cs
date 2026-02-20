using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class TutorialStepPanel : MonoBehaviour, IPointerClickHandler
{
    public int stepIndex;

    [Header("Default: Click -> Next")]
    public bool clickToNext = true;

    [Header("Event Step")]
    public bool isEventStep = false;
    public TutorialCondition condition;

    [Header("On Enter Actions")]
    public UnityEvent onEnter;

    [Header("Input Lock")]
    public bool lockOtherUIAndWorld = true; // ±âº» true

    private TutorialManager manager;

    void Awake()
    {
        manager = FindObjectOfType<TutorialManager>(true);
    }

    void OnValidate()
    {
        if (isEventStep) clickToNext = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!clickToNext) return;
        if (manager == null) return;

        manager.OnPanelClicked();
    }
}
