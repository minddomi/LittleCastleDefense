using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class TileSpriteFitter : MonoBehaviour
{
    public float targetWidth = 1f;
    public float targetHeight = 1f;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        FitSpriteToSize();
    }

    void FitSpriteToSize()
    {
        if (sr == null || sr.sprite == null)
        {
            Debug.LogWarning("SpriteRenderer 또는 Sprite가 없습니다.");
            return;
        }

        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        transform.localScale = new Vector3(
            targetWidth / spriteWidth,
            targetHeight / spriteHeight,
            1f
        );
    }
}