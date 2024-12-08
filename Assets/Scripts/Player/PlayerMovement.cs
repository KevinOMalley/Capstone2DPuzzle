using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private bool transformed = false;

    private bool canJetpack = true;
    private bool isJetpackOn;
    private float jetpackPower = 10f;
    private float jetpackTime = 1f;
    private float jetpackCooldown = 1f;

    //Dashing
    private bool canDash = true;
    private bool isDashing;
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;

    private Battery batt;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float horizontalInput;
    private bool canWallJump = false;

    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform groundCheck;


    private void Awake()
    {
        // grabs references for rigidbody and animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        batt = GetComponent<Battery>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if (isJetpackOn)
        {
            return;
        }

        horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0) gameObject.transform.SetParent(null);

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

        // flip player when moving left-right
        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // set animator params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if(isGrounded())
        {
            transformed = false;
            canWallJump = true;
        }

        if (Input.GetKey(KeyCode.Space) && isGrounded())
            Jump();

        if (Input.GetKey(KeyCode.Q) && canJetpack && batt.currentCharges > 0)
        {
            StartCoroutine(Jetpack());
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        if (!isGrounded() && canWallJump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
            canWallJump = false;
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

    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }

    private IEnumerator Jetpack()
    {
        batt.TakeCharge(1);
        anim.SetTrigger("transform");
        anim.SetTrigger("rocket");
        canJetpack = false;
        isJetpackOn = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(0f, transform.localScale.y * jetpackPower);
        yield return new WaitForSeconds(jetpackTime);
        body.gravityScale = originalGravity;
        isJetpackOn = false;
        yield return new WaitForSeconds(jetpackCooldown);
        canJetpack = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale;
        body.gravityScale = 0f;
        body.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        body.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
