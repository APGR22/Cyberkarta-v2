using UnityEngine;
using UnityEngine.UI;

public class FightArrowController : MonoBehaviour
{
    [HideInInspector] public FightDataType arrowType;
    [HideInInspector] public bool isAnimationEnded = false;

    private Image image;
    private SpriteRenderer spriteRenderer;

    private KeyCode keyCode;

    private bool run = true;

    public void Explosion()
    {
        Animator animator = GetComponent<Animator>();

        animator.SetBool("Explosion", true);
    }

    /// <summary>
    /// Akan cek apakah tombol yang benar ditekan.
    /// </summary>
    /// <returns>
    /// True jika ditekan, false jika tidak.
    /// </returns>
    public bool IsKeyPressedCorrectly()
    {
        return Input.GetKeyDown(this.keyCode);
    }

    public bool IsKeyPressedWrongly()
    {
        //jika suatu tombol ditekan dan tipe arrow yang dimiliki tidak sesuai

        foreach (FightDataType type in System.Enum.GetValues(typeof(FightDataType)))
        {
            if (Input.GetKeyDown(this.ConvertToKeyCode(type)) && this.arrowType != type)
            {
                //true jika suatu tombol yang salah ditekan
                return true;
            }
        }

        return false;
    }

    [ContextMenu("ulang")]
    public void Init(int randomValue = -1)
    {
        /*setup dan init*/

        this.image = GetComponent<Image>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();

        //arrow
        if (randomValue == -1)
        {
            int fightDataTypesCount = System.Enum.GetValues(typeof(FightDataType)).Length;

            System.Random randomSystem = new System.Random(System.Guid.NewGuid().GetHashCode());
            randomValue = randomSystem.Next(0, fightDataTypesCount); //0 - max-1
        }

        this.arrowType = (FightDataType) randomValue;

        //rotate arrow sprite
        switch (this.arrowType)
        {
            case FightDataType.arrowUp:
                transform.Rotate(0, 0, 90);

                this.spriteRenderer.color = Color.blue;
                break;
            case FightDataType.arrowDown:
                transform.Rotate(0, 0, -90);

                this.spriteRenderer.color = Color.red;
                break;
            case FightDataType.arrowLeft:
                this.spriteRenderer.flipX = true;
                this.image.rectTransform.localScale = new Vector3(-1, 1, 1);

                this.spriteRenderer.color = Color.gray;
                break;
            case FightDataType.arrowRight:

                this.spriteRenderer.color = Color.yellow;
                break;
        }

        this.keyCode = this.ConvertToKeyCode(this.arrowType);
    }

    public void SetError()
    {
        this.image.sprite = null;
        this.run = false;
    }

    private KeyCode ConvertToKeyCode(FightDataType arrowType)
    {
        switch (arrowType)
        {
            case FightDataType.arrowUp: return KeyCode.UpArrow;
            case FightDataType.arrowDown: return KeyCode.DownArrow;
            case FightDataType.arrowLeft: return KeyCode.LeftArrow;
            case FightDataType.arrowRight: return KeyCode.RightArrow;
            //case FightDataType.A: return KeyCode.A;
            //case FightDataType.B: return KeyCode.B;
            //case FightDataType.C: return KeyCode.C;
            //case FightDataType.D: return KeyCode.D;
            default: return KeyCode.None;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!this.run) return;

        this.image.sprite = this.spriteRenderer.sprite;
        this.image.color = this.spriteRenderer.color;
    }

    public void AnimationEnded()
    {
        this.isAnimationEnded = true;
    }
}
