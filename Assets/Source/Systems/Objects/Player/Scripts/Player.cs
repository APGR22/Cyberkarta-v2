using UnityEngine;

public class Player : MonoBehaviour
{
    //informasi luar
    public cbkta_GlobalObjects cbkta_globalobjects;
    public cbkta_GlobalUI cbkta_globalui;

    //informasi player
    private Rigidbody2D rb2D;
    private Animator anim;
    private SpriteRenderer spriteRenderer;

    //informasi pergerakan
    private Vector2 move;
    private bool isMoved = false;
    private float currentMoveSpeed = 0;
    private float moveSpeed = 3; //10 untuk lari
    private float moveAcceleration = 13;
    private float moveDeceleration = 13;

    //informasi lompatan
    private bool isJumped = false;
    private float jumpForce = 5;
    private float jumpDelayInSeconds = 0.2f;
    private float jumpTimer = 0;
    private bool isGrounded = false;

    void Move()
    {
        // Tentukan target kecepatan
        float targetSpeed = this.move.x * this.moveSpeed;

        // Kalau ada input → pakai akselerasi dan di tanah
        if (this.isMoved && this.isGrounded)
        {
            this.currentMoveSpeed = Mathf.MoveTowards(this.currentMoveSpeed, targetSpeed, this.moveAcceleration * Time.deltaTime); //acceleration pakai waktu karena ditentukan oleh waktu juga

            //untuk sprite
            this.spriteRenderer.flipX = this.move.x == Vector2.left.x;
        }
        // Kalau tidak ada input + di tanah → pakai deselerasi
        else if (this.isGrounded)
        {
            this.currentMoveSpeed = Mathf.MoveTowards(this.currentMoveSpeed, 0, this.moveDeceleration * Time.deltaTime); //deceleration pakai waktu karena ditentukan oleh waktu juga
        }

        // Apply kecepatan ke Rigidbody
        rb2D.linearVelocityX = this.currentMoveSpeed;
    }

    void Jump()
    {
        if (!this.isJumped) return; //jika tidak ada mau lompat
        //jika belum menyentuh tanah
        if (!this.isGrounded) return;

        if (this.jumpTimer < this.jumpDelayInSeconds)
        {
            this.jumpTimer += Time.deltaTime;
            return;
        }

        this.rb2D.AddForce(Vector2.up * this.jumpForce, ForceMode2D.Impulse);

        //reset
        this.jumpTimer = 0;
    }

    void Animation()
    {
        this.anim.SetFloat("xVelocity", this.currentMoveSpeed < 0 ? -this.currentMoveSpeed : this.currentMoveSpeed);
        this.anim.SetBool("isJumping", this.isJumped);
    }

    void Awake()
    {
        //inisialisasi
        this.rb2D = GetComponent<Rigidbody2D>();
        this.anim = GetComponent<Animator>();
        this.spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.cbkta_globalui.controls.Player.Move.performed += ctx =>
        {
            this.move = ctx.ReadValue<Vector2>();

            this.isMoved = true;
        };

        this.cbkta_globalui.controls.Player.Move.canceled += ctx =>
        {
            this.move = ctx.ReadValue<Vector2>();

            this.isMoved = false;
        };

        this.cbkta_globalui.controls.Player.Jump.performed += ctx =>
        {
            this.isJumped = true;
        };

        Move();
        Jump();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
        Animation();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //tabrakan dengan tanah
        this.isGrounded = collision.gameObject.CompareTag("Ground");
        this.isJumped = !(this.isGrounded);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        //melepas tabrakan dengan tanah
        this.isGrounded = !collision.gameObject.CompareTag("Ground");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        this.cbkta_globalobjects.playerTriggeredWithObject = collision.gameObject;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        this.cbkta_globalobjects.playerTriggeredWithObject = null;
    }
}
