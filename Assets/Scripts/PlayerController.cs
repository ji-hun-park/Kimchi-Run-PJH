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
    
    public SpriteRenderer playerSpriteRenderer;

    public int jumpCount;
    
    //private bool isGrounded = true;

    void Awake()
    {
        jumpCount = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsJumpable() && GameManager.instance.state == GameState.Playing)
        {
            playerRigidBody.AddForceY(jumpForce, ForceMode2D.Impulse);
            jumpCount--;
            playerAnimator.SetInteger("State", 1);
        }
    }

    void ChangeColor(Color color)
    {
        if (!GameManager.instance.isInvincible)
        {
            CancelInvoke("ChangeWhite");
            playerSpriteRenderer.color = color;
            Invoke("ChangeWhite", 1f);
        }
    }

    public void KillPlayer()
    {
        CancelInvoke("ChangeWhite");
        playerSpriteRenderer.color = Color.black;
        playerCollider.enabled = false;
        playerAnimator.enabled = false;
        if (IsJumpable()) playerRigidBody.AddForceY(jumpForce * 0.5f, ForceMode2D.Impulse);
    }

    void Hit()
    {
        GameManager.instance.lives = Mathf.Max(0, GameManager.instance.lives - 1);
        ChangeColor(Color.red);
    }
    
    void Heal()
    {
        if (GameManager.instance.lives == GameManager.instance.maxHP) GameManager.instance.score += 10;
        GameManager.instance.lives = Mathf.Min(GameManager.instance.maxHP, GameManager.instance.lives + 1);
        ChangeColor(Color.green);
    }

    void GetScore()
    {
        GameManager.instance.score += 10;
        ChangeColor(Color.blue);
    }

    void MaxHPPlus()
    {
        if (!GameManager.instance.maxUp)
        {
            GameManager.instance.fourthHP.gameObject.SetActive(true);
            GameManager.instance.maxHP++;
            GameManager.instance.maxUp = true;
        }

        ChangeColor(Color.magenta);
    }

    void StartInvincible()
    {
        CancelInvoke("ChangeWhite");
        playerSpriteRenderer.color = Color.yellow;
        GameManager.instance.isInvincible = true;
        Invoke("StopInvincible", 5f);
    }
    
    void StopInvincible()
    {
        ChangeWhite();
        GameManager.instance.isInvincible = false;
    }

    void ChangeWhite()
    {
        playerSpriteRenderer.color = Color.white;
    }

    bool IsJumpable()
    {
        return jumpCount > 0;
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Platform")
        {
            if (jumpCount != -1)
            {
                playerAnimator.SetInteger("State", 2);
            }
            jumpCount = GameManager.instance.maxJump;
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
        else if (other.CompareTag("Jump"))
        {
            Destroy(other.gameObject);
            GameManager.instance.maxJump++;
        }
        else if (other.CompareTag("Score"))
        {
            Destroy(other.gameObject);
            GetScore();
        }
        else if (other.CompareTag("MaxUp"))
        {
            Destroy(other.gameObject);
            MaxHPPlus();
        }
    }
}
