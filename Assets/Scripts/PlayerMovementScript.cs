using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 25f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform bulletSpawn;
    Animator myAnimator;
    Vector2 moveInput;
    Rigidbody2D myRigidbody2D;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;
    float gravityScaleAtStart;

    bool isAlive = true;
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive){return;}
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive){return;}
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))){ return;  }
        if (value.isPressed)
        {
            // myAnimator.SetTrigger("Jump");
            // myRigidbody2D.velocity = Vector2.up * jumpSpeed;
            myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive){return;}
        Instantiate(bullet, bulletSpawn.position, Quaternion.identity);
    }

    void Run()
    {
        myRigidbody2D.velocity = new Vector2(moveInput.x * runSpeed, myRigidbody2D.velocity.y);
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody2D.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myAnimator.SetBool("IsClimbing", false);
            myRigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }
        myRigidbody2D.gravityScale = 0f;
        myRigidbody2D.velocity = new Vector2(myRigidbody2D.velocity.x, moveInput.y * climbSpeed);

        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody2D.velocity.y) > Mathf.Epsilon;
        myAnimator.SetBool("IsClimbing", playerHasVerticalSpeed);
    }

    void Die()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            StartCoroutine(CO_PlayerDeath());
        }
    }

    IEnumerator CO_PlayerDeath()
    {
        //Find GameManager here instead of in the Start()
        GameSession gameManager = FindObjectOfType<GameSession>();
 
        myAnimator.SetTrigger("Dying");
        isAlive = false;
        myRigidbody2D.velocity = deathKick;
 
 
        yield return new WaitForSeconds(1f);
        gameManager.ProcessPlayerDeath();
    }
}
