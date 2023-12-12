using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    // script pour faire en sorte que la caméra bouge tout le temps avec le player 
    public Transform cameraPosition;
    void Update()
    {
        transform.position = cameraPosition.position; 
    }
}
