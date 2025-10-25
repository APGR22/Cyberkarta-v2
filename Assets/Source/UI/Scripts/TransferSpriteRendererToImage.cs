using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Image))]
public class TransferSpriteRendererToImage : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Image image;

    void Awake()
    {
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.image = this.GetComponent<Image>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Sprite sprite = this.spriteRenderer.sprite;
        Rect spriteRect = sprite.rect;

        RectTransform rectTransform = this.GetComponent<RectTransform>();
        Rect RTRect = rectTransform.rect;

        RTRect.width = spriteRect.width;
        RTRect.height = spriteRect.height;

        if (this.spriteRenderer.flipX)
        {
            RTRect.width *= -1;
        }
        if (this.spriteRenderer.flipY)
        {
            RTRect.height *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        this.image.sprite = spriteRenderer.sprite;
    }
}
