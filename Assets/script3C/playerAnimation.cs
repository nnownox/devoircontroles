using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAnimation : MonoBehaviour
{

    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        //LANCEMENT DE LANIMATION CROUCHEIDLE
        if (Input.GetKey(KeyCode.LeftControl))
        {
            animator.SetBool("Crouch", true);
        }
        else
        {
            animator.SetBool("Crouch", false);
        }
        //Lancement de l'animation walk
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("AAAAA");

            animator.SetBool("Walk", true);
        }
        else
        {
            animator.SetBool("Walk", false);
        }
        if (Input.GetKey(KeyCode.S))
        {


            animator.SetBool("Backward", true);
        }
        else
        {
            animator.SetBool("Backward", false);
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {


            animator.SetBool("Run", true);

        }
        else
        {
            animator.SetBool("Run", false);

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {


            animator.SetBool("jump", true);

        }
        else
        {
            animator.SetBool("jump", false);

        }
        if (Input.GetKey(KeyCode.A))
        {


            animator.SetBool("Left", true);

        }
        else
        {
            animator.SetBool("Left", false);

        }
        if (Input.GetKey(KeyCode.D))
        {


            animator.SetBool("Right", true);

        }
        else
        {
            animator.SetBool("Right", false);

        }



    }
}
