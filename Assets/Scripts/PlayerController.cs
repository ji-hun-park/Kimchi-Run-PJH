using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Settings")]
    public float jumpForce;
    
    [Header("References")]
    public Rigidbody2D playerRigidBody;
    
    public Animator playerAnimator;
    
    public BoxCollider2D playerCollider;
    
    private bool isGrounded = true;
    
    private int lives = 3;
    private bool isInvincible = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            playerRigidBody.AddForceY(jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
            playerAnimator.SetInteger("State", 1);
        }
    }

    void KillPlayer()
    {
        playerCollider.enabled = false;
        playerAnimator.enabled = false;
        playerRigidBody.AddForceY(jumpForce, ForceMode2D.Impulse);
    }

    void Hit()
    {
        lives = Mathf.Max(0, lives - 1);
        if (lives == 0)
        {
            KillPlayer();
        }
    }
    
    void Heal()
    {
        lives = Mathf.Min(3, lives + 1);
    }

    void StartInvincible()
    {
        isInvincible = true;
        Invoke("StopInvincible", 5f);
    }
    
    void StopInvincible()
    {
        isInvincible = false;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            if (!isGrounded)
            {
                playerAnimator.SetInteger("State", 2);
            }
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (!isInvincible)
            {
                Destroy(other.gameObject);
                Hit();
            }
        }
        else if (other.CompareTag("Food"))
        {
            Destroy(other.gameObject);
            Heal();
        }
        else if (other.CompareTag("Golden"))
        {
            Destroy(other.gameObject);
            StartInvincible();
        }
    }
}
