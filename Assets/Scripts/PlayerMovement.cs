using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] public float jetpackFuel;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private bool transformed = false;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float JumpCooldown;
    private float horizontalInput;
    private bool canWallJump = false;
    

    private void Awake()
    {
        // grabs references for rigidbody and animator
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    { 
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
        else if (Input.GetKey(KeyCode.Space) && !isGrounded() && JumpCooldown > 0.2f && jetpackFuel > 0 && !onWall())
        {
            body.velocity = new Vector2(0, jumpPower / 2);
            if(transformed == false)
            {
                anim.SetTrigger("transform");
                transformed = true;
            }
            else
            {
                anim.SetTrigger("rocket");
            }
        }
        else
            JumpCooldown += Time.deltaTime;
        
    }

    private void Jump()
    {
        if (isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        JumpCooldown = 0;
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
}
