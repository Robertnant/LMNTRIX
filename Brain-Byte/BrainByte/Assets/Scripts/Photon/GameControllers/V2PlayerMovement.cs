﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class V2PlayerMovement : MonoBehaviour
{
    // for rotation with mouse
    public float mouseSensitivity = 100f;
    private float xRotation = 0f;

    private PhotonView PV;
    private Animator animator;
    private Rigidbody rigid;
    private Transform transform;
    private Transform myCharacterTransform;     // Transform of the actual character used for Avatar
    private AvatarSetup avatarSetup;

    public float groundDistance = 0.3f;
    public float JumpForce = 500;
    public LayerMask whatIsGround;
    public float jumpRate = 1.25f;
    private float nextJumpTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        avatarSetup = GetComponent<AvatarSetup>();

        PV = GetComponent<PhotonView>();
        transform = GetComponent<Transform>();    // Player avatar's transform

    }

    // Update is called once per frame
    void Update()
    {
        if (PV.IsMine)
        {
            if (animator == null)
                animator = avatarSetup.animator;

            if (rigid == null)
                rigid = avatarSetup.myCharacter.GetComponent<Rigidbody>();

            if (myCharacterTransform == null)
                myCharacterTransform = avatarSetup.myCharacter.transform;

            // Use of previous if statements. Sometimes this script is ran before the one of PlayerSetup
            // Hence, the variable myCharacter is null if that is the case

            animator.SetFloat("Speed", Input.GetAxis("Horizontal"));
            animator.SetFloat("TurningSpeed", Input.GetAxis("Vertical"));

            // rotation with mouse
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation += mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            myCharacterTransform.Rotate(Vector3.up * mouseX);

            if (Time.time >= nextJumpTime)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    rigid.AddForce(Vector3.up * JumpForce);
                    animator.SetTrigger("Jump");
                    nextJumpTime = Time.time + 1f / jumpRate;
                    Debug.Log($"Player {GetComponent<AvatarSetup>().myCharacter.name} jumped");
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
            // Continuously set avatar's transform to the one of character
            // (which is controlled by animator)

            // Recent change
            transform.position = myCharacterTransform.position;
            transform.rotation = myCharacterTransform.rotation;
        }
    }
}