using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject death;
    public GameObject buttons;
    public float speed;
    public float swimmingSpeed;
    public float jumpForceValue;
    public float airValue;

    private float moveInput;
    private float verticalMove;
    private float jumpForce;

    private Rigidbody2D rb;

    private bool facingRight = true;

    public float checkRadius;

    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    public LayerMask whatIsGround;

    [SerializeField] private bool isWatered;
    public Transform waterCheck;
    public LayerMask whatIsWater;

    private int extraJumps;
    public int extraJumpsValue;

    // Start is called before the first frame update
    void Start()
    {
        jumpForce = jumpForceValue;
        extraJumps = extraJumpsValue;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded)
            extraJumps = extraJumpsValue;

        if (!isWatered)
        {
            jumpForce = jumpForceValue;
            Jump();
            Run();
        }
        else
        {
            jumpForce = jumpForceValue / 2;
            Jump();
            Swim();
        }
    }

    private void Swim()
    {
        verticalMove = Input.GetAxis("Vertical");
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(swimmingSpeed * moveInput, swimmingSpeed * verticalMove);


        if (!facingRight && moveInput > 0 || facingRight && moveInput < 0)
            Flip();
    }

    private void Run()
    {
        moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(speed * moveInput, rb.velocity.y);

        if (!facingRight && moveInput > 0 || facingRight && moveInput < 0)
            Flip();
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && extraJumps > 0)
        {
            rb.velocity = Vector2.up * jumpForce;
            extraJumps--;
        }
        else if (Input.GetButtonDown("Jump") && extraJumps == 0 && isGrounded)
        {
            rb.velocity = Vector2.up * jumpForce;
        }
    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
        isWatered = Physics2D.OverlapCircle(waterCheck.position, checkRadius, whatIsWater);
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Air"))
        {
            if (AirCollect.slider.value + airValue < AirCollect.slider.maxValue)
                AirCollect.slider.value += airValue;
            else
            {
                AirCollect.slider.maxValue = AirCollect.slider.value + airValue;
                AirCollect.slider.value = AirCollect.slider.maxValue;
            }
            Destroy(collision.gameObject);
        }

        if (collision.tag.Equals("Ghost"))
        {
            Time.timeScale = 0f;
            buttons.SetActive(false);
            death.SetActive(true);
        }
    }
}
