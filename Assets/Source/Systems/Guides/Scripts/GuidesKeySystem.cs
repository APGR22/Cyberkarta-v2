using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class GuidesKeySystem : MonoBehaviour
{
    public Sprite KeyNotPressed;
    public Sprite KeyPressed;
    public KeyCode KeyCode;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(this.KeyCode))
        {
            this.spriteRenderer.sprite = this.KeyPressed;
        }
        
        if (Input.GetKeyUp(this.KeyCode))
        {
            this.spriteRenderer.sprite = this.KeyNotPressed;
        }
    }
}
