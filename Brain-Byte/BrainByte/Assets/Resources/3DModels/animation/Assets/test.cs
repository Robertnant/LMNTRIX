using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public Animator anim;
    private Rigidbody2D player;
   

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Rigidbody2D>();
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
                anim.SetBool("isWalking", true);
                //LEFT
            }
            else
            {
                if (Input.GetKey("right"))
                {
                    anim.SetBool("isWalking", true);//RIGHT
                }
                else
                {
                    if (Input.GetKey("down"))
                    {                     
                        anim.SetBool("isWalking", true);//DOWN
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
