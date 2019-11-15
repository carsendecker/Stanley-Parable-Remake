using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float Gravity;
    

    private Rigidbody rb;
    private CharacterController cc;
    private Vector3 tempVel;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        tempVel = Input.GetAxis("Horizontal") * transform.right;
        tempVel += Input.GetAxis("Vertical") * transform.forward;
    }

    //Sets the player velocity based on the input values
    private void FixedUpdate()
    {
        cc.Move(new Vector3(tempVel.x * MoveSpeed, -Gravity, tempVel.z * MoveSpeed) * Time.deltaTime);
//        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * MoveSpeed, rb.velocity.y, tempVel.z * MoveSpeed), 0.8f);
    }
}
