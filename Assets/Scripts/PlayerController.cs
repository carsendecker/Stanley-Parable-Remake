using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;

    private Rigidbody rb;
    private Vector3 tempVel;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        tempVel = Input.GetAxis("Horizontal") * transform.right;
        tempVel += Input.GetAxis("Vertical") * transform.forward;
    }

    //Sets the player velocity based on the input values
    private void FixedUpdate()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * MoveSpeed, rb.velocity.y, tempVel.z * MoveSpeed), 0.2f);
    }
}
