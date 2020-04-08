    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoloPlayerMovement : MonoBehaviour
    {

        private CharacterController CControl;
        public float movementSpeed;
        public float rotationSpeed;

        // Start is called before the first frame update
        void Start()
        {
            CControl = GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {
                BasicMovement();
                BasicRotation();
        }

        void BasicMovement()
        {
            /*This is basically WASD
             * This will vary depending on if keyboard is
             * AZERTY, QWERTY or etc
             */

            if (Input.GetKey(KeyCode.UpArrow))
            {
                CControl.Move(transform.forward * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                CControl.Move(-transform.forward * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                CControl.Move(-transform.right * Time.deltaTime * movementSpeed);
            }

            if (Input.GetKey(KeyCode.RightArrow))
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
