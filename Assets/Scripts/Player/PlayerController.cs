using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carsen Decker
//USAGE: Put this on the player object in order to control its movements
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float Gravity;
    
//    private Rigidbody rb;
    private CharacterController cc;
    private Vector3 tempVel;
    
    void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Takes the input axis and adds it to a temporary velocity vector
        tempVel = Input.GetAxis("Horizontal") * transform.right;
        tempVel += Input.GetAxis("Vertical") * transform.forward;
    }

    //Sets the player velocity based on the input values
    private void FixedUpdate()
    {
        //Uses the character controller to move based on the axis inputs and set movement speed, and the set gravity as well
        cc.Move(new Vector3(tempVel.x * MoveSpeed, -Gravity, tempVel.z * MoveSpeed) * Time.deltaTime);
        
        
//        rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(tempVel.x * MoveSpeed, rb.velocity.y, tempVel.z * MoveSpeed), 0.8f);
    }
}
