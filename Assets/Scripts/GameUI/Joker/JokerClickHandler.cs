using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JokerClickHandler : MonoBehaviour
{
    private float lastClickTime = 0f;
    public float doubleClickThreshold = 0.3f;

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            // 흑백조커 클릭 시
            JokerRewardUI.Instance.ShowRandomRewards();
            Destroy(gameObject); // 조커 제거
        }
        lastClickTime = Time.time;
    }
}
