using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private Transform mytransform;
    public Transform groundCheck;

    public GameObject victoryPanel;

    public LayerMask GroundLayer;

    public float GroundCheckRadius = 0.2f;

    public float speed;
    public float speedJump;
    public float speedDash;
    public float timerCoyoteTime = 0.1f;
    public float timeCoyote;

    public bool isDashing = false;
    public bool isJumping = false;
    public bool isGrounded = false;
    public bool isCoyoteTime= false;

    void Start()
    {
        mytransform = GetComponent<Transform>();
        victoryPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, GroundCheckRadius, GroundLayer);

        float moveDelta = Time.deltaTime * speed;
        mytransform.Translate(moveDelta * new Vector2(Input.GetAxis("Horizontal"), 0));

        //if (isGrounded)
        //{
        //    isJumping = false;
        //    isCoyoteTime = true;
        //    timeCoyote = 0;
        //}

        //if (isJumping || !isCoyoteTime)
        //{
        //    addGravity();
        //}

        //if (!isGrounded && isCoyoteTime)
        //{
        //    timeCoyote += Time.deltaTime;
        //    if (timeCoyote > timerCoyoteTime) isCoyoteTime = false;
        //}

        if (Input.GetKey(KeyCode.W))
        {
            jump();
        }


        if (Input.GetKey(KeyCode.Space))
        {
            dash();
        }

    }

    void jump()
    { 
        isJumping = true;
        float jumpForce = Time.deltaTime * speedJump;
        mytransform.Translate(Vector2.up * jumpForce);
    }

    void addGravity()
    {
        float gravityForce = Physics2D.gravity.y * Time.deltaTime;
        mytransform.Translate(Vector2.up * gravityForce);
    }

    void dash()
    {
        float dashForce = Time.deltaTime * speedDash;

        if (Input.GetKey(KeyCode.D))
        {     
            mytransform.Translate(Vector2.right * (mytransform.localScale.x * dashForce));
        }

        if (Input.GetKey(KeyCode.A))
        {
            mytransform.Translate(Vector2.left * (mytransform.localScale.x * dashForce));
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Hazard"))
        {
            playAgain();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Win"))
        {
            victory();
        }
    }

    public void playAgain()
    {
        SceneManager.LoadScene("Game");
    }


    void victory()
    {
        Time.timeScale = 0.0f;
        victoryPanel.SetActive(true);
    }
}
