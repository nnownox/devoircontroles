using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class personnagecontroller : MonoBehaviour
{
    //je commence par créer mes variables 
    public float speed = 10f; // vitesse lorsque j utiliserai getforce sur mon personnage
    private Vector3 movement;// la variable mouvement est vector3
    private bool isGrounded = false;
    private bool isDashing = false; 
    // public float rotationSpeed = 10f;
    private GameObject personnage;
    public GameObject camera;
    // 
    public float thrust = 10.0f;


    // pour modifier la gravite = project settings -> physic -> clotch gravity Y
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame.  
    void Update()
    {
        // mouvements basiques 
        if (Input.GetKey("w"))
        {
            // pour faire en sorte d avancer en fonction des axes de ma camera et non de ma scene 
            Vector3 mouvement = camera.transform.forward;
            mouvement.y = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + mouvement * 5;
        }
        if (Input.GetKey("s"))
        {
            Vector3 mouvement = camera.transform.forward;
            mouvement.y = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + mouvement * -5; 
        }
        if (Input.GetKey("a"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + camera.transform.right * -5;
        }
        if (Input.GetKey("d"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + camera.transform.right * 5;
        }

        //GetComponent<Rigidbody>().velocity = new Vector3(GetComponent<Rigidbody>().velocity.x, 0, GetComponent<Rigidbody>().velocity.y);
        // pour faire en sorte que mon personnage ne puisse sauter que lorsquil est en contact avec le sol. donc pas de double saut. mettre le tag ground sur les objets ou on saute 
        if (Input.GetKeyDown("space") && isGrounded == true)
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, 5, 0);
            isGrounded = false;
        }

        // course. quand je presse des mouvements plus le click gauche, la velocite augmente 

        if (Input.GetKey("w") && Input.GetButton("Left click"))
        {
            Vector3 mouvement = camera.transform.forward;
            mouvement.y = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + mouvement * 20;
        }
        if (Input.GetKey("s") && Input.GetButton("Left click"))
        {
            Vector3 mouvement = camera.transform.forward;
            mouvement.y = 0;
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + mouvement * -20;
        }
        if (Input.GetKey("a") && Input.GetButton("Left click"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) + camera.transform.right * -20; 
        }
        if (Input.GetKey("d") && Input.GetButton("Left click"))
        {
            GetComponent<Rigidbody>().velocity = new Vector3(0, GetComponent<Rigidbody>().velocity.y, 0) +camera.transform.right * 20; 
        }

        // dash. j ajoute de la force lorsque j appuie sur le e 

        if (Input.GetKeyDown("e"))
        {
            StartCoroutine(MaFonction());
            //GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, thrust), ForceMode.Impulse);
        }

        // crouch ( when grounded + get
        if (isGrounded == true && Input.GetKeyDown("q"))
        {
            if (transform.localScale == new Vector3(transform.localScale.x, 0.5f, transform.localScale.z))
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 2, transform.localScale.z);
            }
            else
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y / 2, transform.localScale.z);
            }
        }
  
    }

    void OnCollisionEnter(Collision col) // permet de detecter lorsqu un collider rentre en collision avec un autre collider
    {
        if (col.gameObject.tag == "ground")
        {
            isGrounded = true;
        }
    }

    //pour creer ma coroutine je créé ma fonction avec mon action. IEnumerator pour avoir quelque chose quon attend et qui nous retourne, car sinon notre coroutine attend pour rien
    private IEnumerator MaFonction()
    {
        GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, thrust), ForceMode.Impulse);
        isDashing = true;
       
        yield return new WaitForSeconds(2);

        isDashing = false;

    }
}

