   using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoloPlayerMovement : MonoBehaviour
    {
    private Animator animator;
    private Rigidbody rigid;
    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;
    public float jumpRate = 1.25f;
    private float nextJumpTime = 0;

    public Transform attackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayers;

    public float attackRate = 1.25f;
    private float nextAttackTime = 0;

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

        if (Time.time >= nextJumpTime)
        {
            if (Input.GetButtonDown("Jump"))
            {
                rigid.AddForce(Vector3.up * JumpForce);
                animator.SetTrigger("Jump");
                nextJumpTime = Time.time + 1f / jumpRate;
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
            

        // temporary till creation of single player combat script
        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }

    }

    void Attack()
    {
        // Play attack animation
        animator.SetTrigger("Punch");

        // Detect enemy
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRange, enemyLayers);

        // Damage enemy
        foreach(Collider enemy in hitEnemies)
        {
            Debug.Log("We hit " + enemy.name);
        }

    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
