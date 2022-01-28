using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GDControls : MonoBehaviour
{
    public float speed = 0;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private Vector2 movementVector;
    
    void OnMove(InputValue movementValue)
    {
        movementVector = movementValue.Get<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(movementVector*speed);
    }
}
