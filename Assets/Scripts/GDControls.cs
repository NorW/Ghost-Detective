using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GDControls : MonoBehaviour
{
    public GameObject interactIcon;//For the interaction Icon.

    public float speed;
    private Rigidbody2D rb;
    private bool facingright = true;
    private float moveDirection;
    private Animator animator;

    private Vector2 boxSize = new Vector2(0.1f, 1f);

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))//On E key press it will interact with the first object.
        {
            CheckInteractIcon();
        }

        moveDirection = Input.GetAxis("Horizontal");

        if (moveDirection != 0)
        {
            animator.SetBool("ghostwalk", true);
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

    public void OpenInteractableIcon()//Opens the interaction icon.
    {
        interactIcon.SetActive(true);
    }

    public void CloseInteractableIcon()//Closes the interaction icon.
    {
        interactIcon.SetActive(false);
    }

    private void CheckInteractIcon()//Checks if interaction is possible.
    {
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position, boxSize, 0, Vector2.zero);

        if(hits.Length > 0)
        {
            foreach(RaycastHit2D rc in hits)
            {
                if (rc.transform.GetComponent<Interactable>())
                {
                    rc.transform.GetComponent<Interactable>().Interact();
                    return;
                }
            }
        }
    }
}
