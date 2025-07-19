using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideLeftPanel : MonoBehaviour
{
    public RectTransform panel;// �簢�� �г� (������ ����)
    public float targetWidth = 500f;// ������ ������ ���� �ʺ�
    public float speed = 10f;

    private bool isOpen = false;

    void Start()
    {
        SetWidth(0f);// ó���� ����
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