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

    public void KillPlayer()
    {
        playerCollider.enabled = false;
        playerAnimator.enabled = false;
        if (isGrounded) playerRigidBody.AddForceY(jumpForce * 0.5f, ForceMode2D.Impulse);
    }

    void Hit()
    {
        GameManager.instance.lives = Mathf.Max(0, GameManager.instance.lives - 1);
    }
    
    void Heal()
    {
        GameManager.instance.lives = Mathf.Min(3, GameManager.instance.lives + 1);
    }

    void StartInvincible()
    {
        GameManager.instance.isInvincible = true;
        Invoke("StopInvincible", 5f);
    }
    
    void StopInvincible()
    {
        GameManager.instance.isInvincible = false;
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
            if (!GameManager.instance.isInvincible)
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
