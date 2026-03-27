using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BackgroundFitter2D : MonoBehaviour
{
    [Header("배경을 배치할 카메라")]
    public Camera targetCamera;

    [Header("배경의 Z 위치 (2D 기본 카메라는 -10에 있음)")]
    public float backgroundZ = 10f;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void Start()
    {
        FitToCamera();
    }

    void FitToCamera()
    {
        if (targetCamera == null)
        {
            Debug.LogWarning("카메라가 연결되지 않았습니다.");
            return;
        }

        if (spriteRenderer.sprite == null)
        {
            Debug.LogWarning("SpriteRenderer에 스프라이트가 없습니다.");
            return;
        }

        float worldHeight = targetCamera.orthographicSize * 2f;
        float worldWidth = worldHeight * targetCamera.aspect;

        float spriteWidth = spriteRenderer.sprite.bounds.size.x;
        float spriteHeight = spriteRenderer.sprite.bounds.size.y;

        transform.localScale = new Vector3(
            worldWidth / spriteWidth,
            worldHeight / spriteHeight,
            1f
        );

        transform.position = new Vector3(
            targetCamera.transform.position.x,
            targetCamera.transform.position.y,
            backgroundZ
        );
    }
}