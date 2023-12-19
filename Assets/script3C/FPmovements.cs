using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//mettre les enum en dehors des classes car ils ne sont pas du meme type
//cest automatiquement du plus petit au plus grand, on peut leur assigner manuellement une valeur ronde mais pas besoin car la plupart du temps l ordi l interprete tt seul 
public enum MovementState
{
    walking,
    sprinting,
    crouching,
    air
}

public class FPmovements : MonoBehaviour
{
    [Header("Movement")]
    // variable pour la vitesse de nos deplacements
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;


    public float groundDrag;

    // variables destinees a la creation de mon saut 
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYscale;
    private float startYScale;

    // choisir une key pour notre saut 
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    // choisir une key pour notre sprint
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    //check if the player is on the ground et seulement a ce moment la on appliquera le drag car c est bizarre de l avoir en etant dans les airs
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    // reference au rigidbody de mon player
    Rigidbody rb;

    public MovementState state;

   
    private void Start()
    {
        // assign the rigidbody and freeze its rotation
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        //ground check 
        //pour faire le ground check, on fait en sorte que le raycast ( situe a la position de notre joueur ) aille vers le sol
        //pour verifier si il touche quelque chose. la longueur de notre raycast sera la moitie de la taille de notre joueur + un petit peu plus au cas ou 
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        StateHandler();
        MyInput();
        SpeedControl();

        // handle drag, appliquer le drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
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

        Debug.Log(grounded);
        // quand sauter
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        // debut du crouch
        if(Input.GetKey(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYscale, transform.localScale.z);
            // rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }
        // arreter de s accroupir 
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z); 
        }
    }

    // je cree les conditions de mes enum pour quils puissent chacun se declencher quand telle ou telle condition est active 
    private void StateHandler()
    {
        // mode crouching
        if(Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }
        // mode - sprint 
        if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // mode - Walking
        else if(grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // mode - air
        else
        {
            state = MovementState.air;
        }
    }


    // creation de la fonction pour les mouvements de notre player
    private void MovePlayer()
    {
        // calculate movement direction 
        // bouger dans la direction vers laquelle on regarde
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        print(moveSpeed);

        // on slope
        if(OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force); 
            }
        }
       
        // on ground 
        else if(grounded)
        {
            // ajouter reellement de la force afin de pouvoir bouger notre personnage.
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }

        // in air
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiter la vitesse sur les pentes
        if (OnSlope() && !exitingSlope)
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed; 
            }
        }

        //limiter la vitesse sur le sol et dans les airs 
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //limite la velocite si besoin 
            // normalized est utilise avec les Vector3 afin que leurs valeurs soient set a 1. cela est utilise souvent pour jouer avec la magnitude, afin de ralentir un peu les mouvements
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true; 
        Debug.Log("saut");
        // avant d appliquer nimporte quelle force, on veut s assurer que notre velocite en Y est set a 0 pour pouvoir sauter toujours a la meme hauteur 
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        // on utilise impulse car on veut avoir de la force qu au debut de notre saut. a noter que impulse est le mode par defaut donc pas besoin de l ajouter dans la logique
        // mais c est toujours bien de le voir ecrit pour comprendre hihi
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false; 
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector2.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized; 
    }

}
