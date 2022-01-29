using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDControls : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    private bool facingright = true;
    private float moveDirection;
    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveDirection = Input.GetAxis("Horizontal");

        if (moveDirection != 0)
        {
            animator.SetBool("ghostwalk",true);
        }
        else
        {
            animator.SetBool("ghostwalk", false);
        }

        if (moveDirection > 0 && !facingright)
        {
            FlipCharacter();
        }
        else if (moveDirection < 0 && facingright)
        {
            FlipCharacter();
        }    
        rb.velocity = new Vector2(moveDirection * speed, rb.velocity.y);
    }

    private void FlipCharacter()
    {
        facingright = !facingright;
        transform.Rotate(0f, 180f, 0f);
    }
}
