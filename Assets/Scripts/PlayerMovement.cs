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

    [SerializeField] private float jumpTime = 1.0f;
    [SerializeField] private bool isGrounded = true;
    [SerializeField] private float jumpTimeCounter;

    public float speed;
    public float speedJump;
    public float speedDash;
    public float timerCoyoteTime = 0.1f;
    public float timeCoyote;

    public bool isDashing = false;
    public bool isJumping = false;
    public bool isCoyoteTime= false;

    void Start()
    {
        mytransform = GetComponent<Transform>();
        victoryPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    void Update()
    {

        float moveDelta = Time.deltaTime * speed;
        mytransform.Translate(moveDelta * new Vector2(Input.GetAxis("Horizontal"), 0));


        if (isJumping)
        {
            if (jumpTimeCounter > 0)
            {

                var jumpForce = Time.deltaTime * speedJump;
                jumpTimeCounter -= Time.deltaTime;
                mytransform.Translate(Vector2.up * jumpForce);
                
            }
            else
            {
                isJumping = false;
            }
        }

        if(isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.W) && !isJumping)
            {
                jump();
            }
        }


        if (Input.GetKey(KeyCode.Space))
        {
            dash();
        }

    }

    void jump()
    {
        isJumping = true;
        jumpTimeCounter = jumpTime;

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

        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Win"))
        {
            victory();
        }
        if (collision.gameObject.CompareTag("Die"))
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
