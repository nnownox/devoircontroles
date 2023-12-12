using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPcam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        // bloquer le curseur au milieu de l'écran et faire en sorte qu'on ne le voit pas 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // get mouse input. je le fais pour les deux axes X  et Y
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        // ajouter l input a la rotation : on ajoute pour le Y et On enleve pour l axe X 
        yRotation += mouseX;
        xRotation -= mouseY;

        // maintenant on limite notre camera pour qu'elle ne puisse pas bouger a plus de 90 degres
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate la camera et son orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
    }
}
