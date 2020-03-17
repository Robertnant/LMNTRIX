using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newcode : MonoBehaviour {

    public Animator anim;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();
		
	}
	
	// Update is called once per frame
	void Update () {
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
