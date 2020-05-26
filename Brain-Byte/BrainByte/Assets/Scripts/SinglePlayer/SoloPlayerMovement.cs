   using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

public class SoloPlayerMovement : MonoBehaviour
{
    // for rotation with mouse
    public float defaultMouseSensitivity = 50f;
    public float mouseSensitivityX;
    public float mouseSensitivityY = 1.5f;
    public float clamp = 0.001f;
    public Transform playerCamera;
    public float maxTopRotation = -20f;
    public float minBottomRotation = 15f;

    private Animator animator;
    private Rigidbody rigid;
    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;
    public float jumpRate = 1.25f;
    private float nextJumpTime = 0f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody>();

        // lock and hide cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Horizontal"));
        animator.SetFloat("TurningSpeed", Input.GetAxis("Vertical"));

        // rotation with mouse
        float mouseX = Input.GetAxis("Mouse X") * defaultMouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * defaultMouseSensitivity * Time.deltaTime;
        
        float negPosAngle = playerCamera.eulerAngles.x;     // logical: cause by rotating up and down, the camera rotates around x axis (Physics)
        negPosAngle = negPosAngle > 180 ? negPosAngle - 360 : negPosAngle;

        Debug.Log(negPosAngle); 
        
        if (negPosAngle >= minBottomRotation)
            playerCamera.eulerAngles = new Vector3(minBottomRotation - clamp + 360, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
        else if (negPosAngle <= maxTopRotation)
            playerCamera.eulerAngles = new Vector3(maxTopRotation + clamp, playerCamera.eulerAngles.y, playerCamera.eulerAngles.z);
        else
            playerCamera.Rotate(Mathf.Clamp(mouseY, -mouseSensitivityY, mouseSensitivityY), 0, 0, Space.Self);

        // For X axis

        transform.Rotate((Vector3.up * mouseX));


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

        // To be ran by every player (regardless of if PV.isMine)
        LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

        if (levelLoader.completeLevelUI.activeSelf || levelLoader.gameOverUI.activeSelf)
        {
            animator.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            this.enabled = false;
        }

    }

}
