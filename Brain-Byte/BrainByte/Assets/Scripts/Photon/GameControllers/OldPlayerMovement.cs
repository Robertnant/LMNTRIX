using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OldPlayerMovement : MonoBehaviour
{
    private PhotonView PV;
    private CharacterController CControl;
    public float movementSpeed;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();
        CControl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(PV.IsMine)
        {
            BasicMovement();
            BasicRotation();
        }
    }

    void BasicMovement()
    {
        /*This is basically WASD
         * This will vary depending on if keyboard is
         * AZERTY, QWERTY or etc
         */

        if(Input.GetKey(KeyCode.Z))
        {
            CControl.Move(transform.forward * Time.deltaTime * movementSpeed); 
        }

        if (Input.GetKey(KeyCode.S))
        {
            CControl.Move(-transform.forward * Time.deltaTime * movementSpeed);
        }
        
        if (Input.GetKey(KeyCode.Q))
        {
            CControl.Move(-transform.right * Time.deltaTime * movementSpeed);
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            CControl.Move(transform.right * Time.deltaTime * movementSpeed);
        }

    }

    void BasicRotation()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * rotationSpeed;
        transform.Rotate(new Vector3(0, mouseX, 0));
    }
}
