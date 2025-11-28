using System.Collections;
using UnityEngine;

public class ObstacleBlocker : MonoBehaviour
{
    [Tooltip("방해물이 유지되는 시간(초)")]
    public float lifeTime = 20f;

    private Tile tile;

    public void Init(Tile targetTile)
    {
        tile = targetTile;
        if (tile != null)
        {
            tile.SetBlocked(true);
            // 시각적 연출 원하면 프리팹에서 처리(스프라이트/파티클)
            transform.position = tile.transform.position + new Vector3(0f, 0f, -0.5f);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SelfDestruct());
    }

    private IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(lifeTime);
        if (tile != null) tile.SetBlocked(false);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (tile != null) tile.SetBlocked(false);
    }
}
