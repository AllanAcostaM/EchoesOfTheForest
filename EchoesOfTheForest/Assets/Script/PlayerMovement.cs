using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class PlayerMovement:MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 10;
    public float moveSpeed = 2.0f;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        
        
            float moveX = Input.GetAxis("Horizontal"); // Movimiento en X
            float moveZ = Input.GetAxis("Vertical");   // Movimiento en Z
            float moveY = 0f;

            Vector3 movement = new Vector3(moveX, moveY, moveZ) * moveSpeed;
            rb.velocity = movement;
        
        
    }

    

    













}
