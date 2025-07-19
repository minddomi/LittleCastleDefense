using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideLeftPanel : MonoBehaviour
{
    public RectTransform panel;// 사각형 패널 (오른쪽 고정)
    public float targetWidth = 500f;// 스르륵 펼쳐질 최종 너비
    public float speed = 10f;

    private bool isOpen = false;

    void Start()
    {
        SetWidth(0f);// 처음엔 숨김
    }

    void Update()
    {
        float goal = isOpen ? targetWidth : 0f;
        float current = panel.sizeDelta.x;
        float next = Mathf.Lerp(current, goal, Time.deltaTime * speed);

        SetWidth(next);
    }

    void SetWidth(float width)
    {
        panel.sizeDelta = new Vector2(width, panel.sizeDelta.y);
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
    }
}