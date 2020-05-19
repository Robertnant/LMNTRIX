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

    // for attack modes
    public float attackSelectionRate = 2f;
    private float nextSelectionTime = 0;
    public string[] attackModes;
    public int attackMode;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        // set up attack modes
        attackModes = new string[] { "Punch", "Pistol", "Semi-auto", "Knife" };
        attackMode = 0;     // default
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Horizontal"));
        animator.SetFloat("TurningSpeed", Input.GetAxis("Vertical"));

        // Change weapon or attack mode
        if (Time.time >= nextSelectionTime)
        {
            ChangeAttackMode();
        }

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

    void ChangeAttackMode()
    {
        // with num keys
        if (Input.anyKeyDown)
        {
            for (int i = 1; i <= attackModes.Length; i++)
            {
                if (Input.inputString == i.ToString())
                {
                    attackMode = i - 1;
                    nextSelectionTime = Time.time + 1f / attackSelectionRate;
                    Debug.Log("Selected attack mode/weapon: " + attackModes[attackMode]);
                }
            }
        }

        // with mouse scroll wheel
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // up
        {
            attackMode = attackMode + 1 < attackModes.Length ? attackMode + 1 : 0;
            nextSelectionTime = Time.time + 1f / attackSelectionRate;
            Debug.Log("Selected attack mode/weapon: " + attackModes[attackMode]);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // down
        {
            attackMode = attackMode - 1 >= 0 ? attackMode - 1 : attackModes.Length - 1;
            nextSelectionTime = Time.time + 1f / attackSelectionRate;
            Debug.Log("Selected attack mode/weapon: " + attackModes[attackMode]);
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
