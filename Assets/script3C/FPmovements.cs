using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPmovements : MonoBehaviour
{
    [Header("Movement")]
    // variable pour la vitesse de nos deplacements
    public float moveSpeed;

    public float groundDrag;

    //check if the player is on the ground et seulement a ce moment la on appliquera le drag car c est bizarre de l avoir en etant dans les airs
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    // reference au rigidbody de mon player
    Rigidbody rb;

    private void Start()
    {
        // assign the rigidbody and freeze its rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 
    }

    private void Update()
    {
        //ground check 
        //pour faire le ground check, on fait en sorte que le raycast ( situe a la position de notre joueur ) aille vers le sol
        //pour verifier si il touche quelque chose. la longueur de notre raycast sera la moitie de la taille de notre joueur + un petit peu plus au cas ou 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        // handle drag, appliquer le drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
        MyInput();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    // on cree une fonction input similaire a celle qu on a fait dans le script de la cam mais cette fois ci lie en clavier 
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    // creation de la fonction pour les mouvements de notre player
    private void MovePlayer()
    {
        // calculate movement direction 
        // bouger dans la direction vers laquelle on regarde
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // ajouter reellement de la force afin de pouvoir bouger notre personnage.
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limite la velocite si besoin 
        // normalized est utilise avec les Vector3 afin que leurs valeurs soient set a 1. cela est utilise souvent pour jouer avec la magnitude, afin de ralentir un peu les mouvements
        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

}
