using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FreezeEnemy : MonoBehaviour
{
    public float freezeDuration = 3f; // 적 유닛 정지 시간 (초)
    private bool isAbilityActive = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isAbilityActive && Input.GetMouseButtonDown(0))
        {
            // UI 클릭 방지
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                EnemyUnit enemy = hit.collider.GetComponent<EnemyUnit>();
                if (enemy != null)
                {
                    enemy.Freeze(freezeDuration);
                    Debug.Log($"[FreezeEnemy] {enemy.name} 정지됨 ({freezeDuration}초)");
                    isAbilityActive = false;
                }
            }
        }
    }

    // 버튼 클릭 시 연결할 함수
    public void ActivateFreezeAbility()
    {
        isAbilityActive = true;
        Debug.Log("[FreezeEnemy] 능력 사용 대기 중: 적을 클릭하세요.");
    }
}
