using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(SceneDataTemplate))]
public class FightArrowController : MonoBehaviour
{
    public int minArrowCounts = 3;
    public int maxArrowCounts = 5;

    [HideInInspector] public FightDataType arrowType;
    [HideInInspector] public bool isAnimationEnded = false;

    private KeyCode keyCode;

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

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

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
                break;
            case FightDataType.arrowDown:
                transform.Rotate(0, 0, -90);
                break;
            case FightDataType.arrowLeft:
                spriteRenderer.flipX = true;
                break;
            case FightDataType.arrowRight:
                break;
        }

        this.keyCode = this.ConvertToKeyCode(this.arrowType);
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
        
    }

    public void AnimationEnded()
    {
        this.isAnimationEnded = true;
    }
}
