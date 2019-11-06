using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;

    private Rigidbody rb;
    private Vector3 tempVel;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tempVel = Input.GetAxis("Horizontal") * transform.right;
        tempVel += Input.GetAxis("Vertical") * transform.forward;
        
        //Interact with buttons
    }

    private void FixedUpdate()
    {
        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * MoveSpeed, rb.velocity.y, tempVel.z * MoveSpeed), 0.2f);
    }
}
