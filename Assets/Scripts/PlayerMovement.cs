using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask diamondLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;
    public DiamondManager diamondManager;

    private bool moveLeft;
    private bool moveRight;

    [Header("SFX")]
    [SerializeField] public AudioClip jumpSound;
    [SerializeField] public AudioClip diamondSound;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        diamondManager = FindObjectOfType<DiamondManager>();

        moveLeft = false;
        moveRight = false;
    }

    public void PointerDownLeft()
    {
        moveLeft = true;
    }

    public void PointerUpLeft()
    {
        moveLeft = false;
    }

    public void PointerDownRight()
    {
        moveRight = true;
    }

    public void PointerUpRight()
    {
        moveRight = false;
    }

    public void Update()
    {
        Move();
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if (wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.velocity = Vector2.zero;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, 1f, LayerMask.GetMask("YourWallLayer"));
                if (!hit)
                    body.gravityScale = 7;
            }
            else
                body.gravityScale = 7;

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
                if(Input.GetKeyDown(KeyCode.Space) && isGrounded())
                    SoundManager.instance.PlaySound(jumpSound);
            }
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    public void Move()
    {
        if (moveLeft)
        {
            horizontalInput = -1;
        }
        else if (moveRight)
        {
            horizontalInput = 1;
        }
        else
        {
            horizontalInput = 0;
        }
    }

    private void FixedUpdate()
    {
        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
    }

    public void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if (onWall() && !isGrounded())
        {
            if (horizontalInput == 0)
            {
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 10, 0);
                transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            }
            else
                body.velocity = new Vector2(-Mathf.Sign(transform.localScale.x) * 3, 6);
            wallJumpCooldown = 0;
        }
    }

    private bool isGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }

    void OnTriggerEnter2D(Collider2D diamondO)
    {
        if (diamondO.gameObject.CompareTag("Diamond"))
        {
            diamondManager.UpdateDiamondCount(1);
            SoundManager.instance.PlaySound(diamondSound);
            Destroy(diamondO.gameObject);
        }
    }
}
