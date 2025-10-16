using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("이동/점프")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float jumpForce = 7.0f;

    [Header("바닥 체크")]
    public Transform GroundCheck; // 발밑 위치를 나타내는 트랜스폼
    [SerializeField] private float groundCheckRadius = 0.5f; // 감지용 반지름
    [SerializeField] private LayerMask groundLayer; // 레이어 설정

    [Header("프리팹")]
    public Player_Bullet bulletPrefab;
    public Explosion explosionPrefab;

    [Header("총알 설정")]
    private float fireRate = 0.3f;
    float nextFireTime;
    public Transform firePoint1;
    public Transform firePoint2;

    [Header("TEXT")]
    public Score scorescript;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject gameWin;

    private Animator anime;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private float inputX;
    private bool isJumping;
    private bool isGrounded;

    private static readonly int moveHash = Animator.StringToHash("Move");
    private static readonly int jumpHash = Animator.StringToHash("IsJump");

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anime = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Mangers.Pool.CreatePool(bulletPrefab, 10);
        Mangers.Pool.CreatePool(explosionPrefab, 10);
        scorescript.isGameOver = false;
        gameOver.SetActive(false);
        gameWin.SetActive(false);
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        if (inputX != 0)
        {
            if (inputX < 0)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isGrounded)
        {
            isJumping = true;
        }
        anime.SetFloat(moveHash, Mathf.Abs(rb.velocity.x));

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireRate;
            Fire();
        }

        if (transform.position.y <= -5f)
        {
            scorescript.isGameOver = true;
            gameObject.SetActive(false);
            gameOver.SetActive(true);
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(GroundCheck.position, groundCheckRadius, groundLayer);

        rb.velocity = new Vector2(inputX * moveSpeed, rb.velocity.y);

        if (isJumping && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anime.SetBool(jumpHash, true);
        }
        isJumping = false;

        if (isGrounded && rb.velocity.y <= 0.05f)
        {
            anime.SetBool(jumpHash, false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(GroundCheck.position, groundCheckRadius);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Event") || collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            scorescript.isGameOver = true;
            gameObject.SetActive(false);
            gameOver.SetActive(true);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("End"))
        {
            scorescript.isGameOver = true;
            gameObject.SetActive(false);
            gameWin.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Score score = FindAnyObjectByType<Score>();
            score.coinCountUP();
            Destroy(collision.gameObject);
        }
    }

    void Fire()
    {
        Player_Bullet bullet = Mangers.Pool.GetFromPool(bulletPrefab);

        if (spriteRenderer.flipX == true)
        {
            bullet.transform.position = firePoint1.position;
            bullet.SetDir(Vector2.left);
        }
        else
        {
            bullet.transform.position = firePoint2.position;
            bullet.SetDir(Vector2.right);
        }
    }
}
