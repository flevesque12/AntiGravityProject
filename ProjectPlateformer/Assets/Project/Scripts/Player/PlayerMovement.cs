using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region Variables

    public float walkSpeed = 3.0f;

    //public float jumpHeight = 6f;
    public float gravityScale = -8.5f;

    private float extraHeight = 0.01f;
    public LayerMask groundLayerMask;
    private bool m_IsMoving = false;
    public bool IsMoving { get { return this.m_IsMoving; } }

    private Rigidbody2D m_rb;
    private Collider2D m_Collider;
    private SpriteRenderer m_Render;
    private Animator m_Anim;

    private Vector2 m_PlayerDirectionY = Vector2.down;
    private bool m_IsAntigravityIsOn = false;
    public bool IsIsAntigravityIsOn { get { return this.m_IsAntigravityIsOn; } }

    private bool m_IsJumpStart = false;
    public bool IsJumpStart { get { return this.m_IsJumpStart; } }

    #endregion Variables

    // Start is called before the first frame update
    private void Start()
    {
        m_Collider = GetComponent<Collider2D>();
        m_rb = GetComponent<Rigidbody2D>();
        m_Render = GetComponent<SpriteRenderer>();
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        FlipSprite();

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
        {
            if (!m_IsAntigravityIsOn)
            {
                m_IsAntigravityIsOn = true;
                m_Render.flipY = true;
                m_PlayerDirectionY = Vector2.up;
            }
            else
            {
                m_IsAntigravityIsOn = false;
                m_Render.flipY = false;
                m_PlayerDirectionY = Vector2.down;
            }
            m_IsJumpStart = true;
        }
        else
        {
            m_IsJumpStart = false;
        }

        ApplyAnimation();

        //IsGrounded();
    }

    private void FixedUpdate()
    {
        Move();
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit2d = Physics2D.Raycast(m_Collider.bounds.center, m_PlayerDirectionY, m_Collider.bounds.extents.y + extraHeight, groundLayerMask);

        Color rayColor;
        rayColor = Color.green;
        if (hit2d.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(m_Collider.bounds.center, m_PlayerDirectionY * (m_Collider.bounds.extents.y + extraHeight), rayColor);

        return hit2d.collider != null;
    }

    public void Move()
    {
        float velocity = Input.GetAxis("Horizontal") * walkSpeed;

        if (m_IsAntigravityIsOn == false)
        {
            m_rb.velocity = new Vector2(velocity, -gravityScale);
        }

        if (m_IsAntigravityIsOn)
        {
            m_rb.velocity = new Vector2(velocity, gravityScale);
        }
    }

    private void ApplyAnimation()
    {
        if (m_rb.velocity.x != 0 && IsGrounded())
        {
            m_Anim.SetBool("IsWalking", true);
            m_IsMoving = true;
        }
        else
        {
            m_Anim.SetBool("IsWalking", false);
            m_IsMoving = false;
        }
    }

    private void FlipSprite()
    {
        if (m_rb.velocity.x < 0)
        {
            m_Render.flipX = true;
        }
        else if (m_rb.velocity.x > 0)
        {
            m_Render.flipX = false;
        }
    }
}