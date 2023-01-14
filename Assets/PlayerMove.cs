using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 0.4f;

    public Rigidbody2D body;

    Animator animator;

    Vector2 move;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        move.x = Input.GetAxisRaw("Horizontal");
        move.y = Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        if (move.x != 0 && move.y != 0)
            move *= 0.7f;
        if (body != null)
            body.velocity = move * speed;
        if (move != Vector2.zero)
            animator.SetBool("isWalking", true);
        else
            animator.SetBool("isWalking", false);
        if (move.x > 0)
            transform.localScale = Vector2.one;
        else if (move.x < 0)
            transform.localScale = new Vector2(-1, 1);
    }
}
