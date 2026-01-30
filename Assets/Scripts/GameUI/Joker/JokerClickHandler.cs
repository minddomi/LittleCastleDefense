using UnityEngine;

public class JokerClickHandler : MonoBehaviour
{
    private float lastClickTime = 0f;
    public float doubleClickThreshold = 0.3f;

    void OnMouseDown()
    {
        if (Time.time - lastClickTime < doubleClickThreshold)
        {
            UnitStatus status = GetComponent<UnitStatus>();
            if (status == null)
            {
                Debug.LogWarning("[JokerClickHandler] UnitStatus를 찾을 수 없습니다.");
                return;
            }

            string jokerType = GetJokerTypeByID(status.unitID);

            JokerRewardUI ui = FindObjectOfType<JokerRewardUI>(true);
            if (ui != null)
                ui.ShowRandomRewards(jokerType);
            else
                Debug.LogError("[JokerClickHandler] JokerRewardUI를 찾을 수 없습니다.");

            //   UnitStatus의 posX, posY로 타일 점유 해제
            ReleaseTile(status.posX, status.posY);

            Destroy(gameObject);
        }

        lastClickTime = Time.time;
    }

    private string GetJokerTypeByID(string unitID)
    {
        return unitID switch
        {
            "Joker1" => "흑백 조커",
            "Joker2" => "컬러 조커",
            _ => "컬러 조커"
        };
    }

    //  posX, posY로 해당 타일을 찾아 점유 해제
    private void ReleaseTile(int x, int y)
    {
        Tile[] allTiles = FindObjectsOfType<Tile>();
        foreach (Tile tile in allTiles)
        {
            if (tile.gridPosition.x == x && tile.gridPosition.y == y)
            {
                tile.SetCurrentUnit(null); // 내부에서 isOccupied = false 처리됨
                Debug.Log($"[JokerClickHandler] 타일 점유 해제 완료 ({x}, {y})");
                return;
            }
        }

        Debug.LogWarning($"[JokerClickHandler] ({x}, {y}) 위치의 타일을 찾을 수 없습니다.");
    }
}
