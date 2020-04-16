using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAnimation : MonoBehaviour
{

    private Animator animator;
    private Rigidbody rigid;
    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Horizontal"));
        animator.SetFloat("TurningSpeed", Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            rigid.AddForce(Vector3.up * JumpForce);
            animator.SetTrigger("Jump");
        }
        if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, groundDistance, whatIsGround))
        {
            animator.SetBool("Grounded", false);
            animator.applyRootMotion = true;
        }
        else
        {
            animator.SetBool("Grounded", true);
        }

    }

}