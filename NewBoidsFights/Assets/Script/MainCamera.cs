using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public float speed = 10;
    void Start()
    {
        
    }

    
    void Update()
    {
        Movement();
        HandleMovement();
    }


    void Movement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * speed);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * speed);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * speed);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * speed);
        }
    }
    
    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(Vector3.forward * speed);
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.Translate(Vector3.back * speed);
        }
    }
}
