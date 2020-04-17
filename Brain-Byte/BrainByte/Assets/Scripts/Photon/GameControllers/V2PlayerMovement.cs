using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2PlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private Animator animator;
    private Rigidbody rigid;
    private Transform transform;
    private Transform myCharacterTransform;     // Transform of the actual character used for Avatar
    
    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = GetComponent<AvatarSetup>().myCharacter.GetComponent<Animator>();  //Gets animator from player avatar in use (example: detective)
        rigid = GetComponent<AvatarSetup>().myCharacter.GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();    // Player avatar's transform
        myCharacterTransform = GetComponent<AvatarSetup>().myCharacter.transform;

    }

    // Update is called once per frame
    void Update()
    {
        if(animator == null)
            animator = GetComponent<AvatarSetup>().myCharacter.GetComponent<Animator>();

        if(rigid == null)
            rigid = GetComponent<AvatarSetup>().myCharacter.GetComponent<Rigidbody>();

        // Use of previous if statements. Sometimes this script is ran before the one of PlayerSetup
        // Hence, the variable myCharacter is null if that is the case

        if (PV.IsMine)
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

            // Continuously set avatar's transform to the one of character
            // (which is controlled by animator)

            transform.position = myCharacterTransform.position;
        }
    }
}