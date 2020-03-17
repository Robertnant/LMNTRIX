
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Animator anim;

    // Use this for initialization
    void Start()
    {

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("up"))
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            if (Input.GetKey("left"))
            {
                transform.Rotate(0, 90, 0);
            }
            else
            {
                if (Input.GetKey("right"))
                {
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    if (Input.GetKey("down"))
                    {
                        anim.SetBool("isWalking", true);
                    }
                    else
                    {
                        anim.SetBool("isWalking", false);
                    }
                }
            }
        }

    }
}






























/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator anim;
    // Use this for initialization
    public float speed;
    public float rotationSpeed = 100.0F;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        translation *= Time.deltaTime;
        rotation *= Time.deltaTime;
        transform.Translate(0, 0, translation);
        transform.Rotate(0, rotation, 0);
        
        if (translation != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}*/






