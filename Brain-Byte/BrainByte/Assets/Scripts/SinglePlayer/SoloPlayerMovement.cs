   using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class SoloPlayerMovement : MonoBehaviour
{
    // for rotation with mouse
    public float defaultMouseSensitivity = 50f;
    public float mouseSensitivityX;
    public float mouseSensitivityY = 1.5f;
    public float clamp = 0.01f;
    public Transform playerCamera;
    public float maxTopRotation = -20f;
    public float minBottomRotation = 10f;

    private Animator animator;
    private Rigidbody rigid;
    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;
    public float jumpRate = 1.25f;
    private float nextJumpTime = 0f;

    public Transform attackPoint;
    public float attackRange = 0.6f;
    public LayerMask enemyLayers;

    public float attackRate = 1.25f;
    private float nextAttackTime = 0f;

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

        // lock cursor
        Cursor.lockState = CursorLockMode.Locked;

        // set up attack modes
        attackModes = new string[] { "Punch", "Pistol", "Semi-auto", "Knife" };
        attackMode = 0;     // default
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Horizontal"));
        animator.SetFloat("TurningSpeed", Input.GetAxis("Vertical"));

        // rotation with mouse
        float mouseX = Input.GetAxis("Mouse X") * defaultMouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * defaultMouseSensitivity * Time.deltaTime;

        /*xRotation += mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);*/

        


        /* push the camera back into range of rotation if max or min range reached
         * Basically prevent user from doing 360° camera rotation*/
        
        float negPosAngle = playerCamera.eulerAngles.x;
        negPosAngle = negPosAngle > 180 ? negPosAngle - 360 : negPosAngle;

        Debug.Log(negPosAngle); // logical: cause by rotating up and down, the camera rotates around x axis (Physics)
        
        if (negPosAngle >= minBottomRotation)
            playerCamera.eulerAngles = new Vector3(minBottomRotation -clamp + 360, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
        else if (negPosAngle <= maxTopRotation)
            playerCamera.eulerAngles = new Vector3(maxTopRotation + clamp, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
        else
            playerCamera.Rotate(Mathf.Clamp(mouseY, -mouseSensitivityY, mouseSensitivityY), 0, 0, Space.Self);

        /*if (playerCamera.eulerAngles.x >= 0 && playerCamera.eulerAngles.x < minBottomRotation ||
        playerCamera.eulerAngles.x <= 360 && playerCamera.eulerAngles.x > maxTopRotation)
            playerCamera.Rotate(mouseY, 0, 0, Space.Self);
        else if (playerCamera.eulerAngles.x >= maxTopRotation)
            playerCamera.Rotate(1, 0, 0, Space.Self);
        else if (playerCamera.eulerAngles.x <= minBottomRotation)
            playerCamera.Rotate(-1, 0, 0, Space.Self);*/



        //playerCamera.eulerAngles = new Vector3(mouseY, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);

        //playerCamera.Rotate(mouseY, 0, 0);
        //playerCamera.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate((Vector3.up * mouseX));

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
