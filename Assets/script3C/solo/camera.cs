using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float rotationSpeed = 10f;
    public GameObject player;
    //private float rotationAroundYAxis = 0;
    //private float rotationAroundXAxis = 0;
    [SerializeField] Camera cam;

    public float zoom;


    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(player.transform.position, Vector3.up, rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse X"));
        transform.Rotate(Vector3.right * rotationSpeed * Time.deltaTime * Input.GetAxis("Mouse Y"));
        
        if(cam)
        {
            cam.fieldOfView  -= Input.GetAxis("Mouse ScrollWheel") * zoom;
        }
        else
        {
            cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * zoom; 
        }

        //rotation.y = Mathf.Clamp (Vector3.up, 0, 90);
    }
}
