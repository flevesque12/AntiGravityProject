﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    #region Variables

    [SerializeField]
    private float walkSpeed = 3.0f;

    //public float jumpHeight = 6f;
    [SerializeField]
    private float gravityScale = 8.5f;

    private float extraHeight = 0.01f;
    public LayerMask groundLayerMask;

    private bool m_IsMoving = false;
    public bool IsMoving { get { return this.m_IsMoving; } }

    private Rigidbody2D m_rb;
    private Collider2D m_Collider;
    private SpriteRenderer m_Render;
    private Animator m_Anim;

    private Vector2 m_PlayerDirectionY = Vector2.down;
    private float m_velocity;

    private bool m_IsAntigravityIsOn = false;
    public bool IsIsAntigravityIsOn { get { return this.m_IsAntigravityIsOn; } }

    private bool m_IsJumpStart = false;
    public bool IsJumpStart { get { return this.m_IsJumpStart; } }

    private bool m_IsOnFloor = false;
    public bool IsOnFloor { get { return this.m_IsOnFloor; } set { this.IsOnFloor = value; } }
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
        m_velocity = Input.GetAxisRaw("Horizontal") * walkSpeed;

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

        FlipSprite();
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

        if (m_IsAntigravityIsOn == false)
        {
            m_rb.velocity = new Vector2(m_velocity, -gravityScale);
        }

        if (m_IsAntigravityIsOn)
        {
            m_rb.velocity = new Vector2(m_velocity, gravityScale);
        }
    }

    private void ApplyAnimation()
    {
       
        if (m_velocity != 0 && IsGrounded())
        {
            m_Anim.SetBool("IsWalking", true);
            
            m_IsMoving = true;
            m_IsOnFloor = true;
        }
        else
        {
            m_Anim.SetBool("IsWalking", false);
           
            m_IsMoving = false;
            m_IsOnFloor = false;
        }

        
    }

    private void FlipSprite()
    {
        float dotprod = Vector2.Dot(m_rb.velocity, transform.right);

        if (dotprod > 0)
        {
           m_Render.flipX = false;
        }
        else if(dotprod < 0)
        {
             m_Render.flipX = true;
        }
    }

}