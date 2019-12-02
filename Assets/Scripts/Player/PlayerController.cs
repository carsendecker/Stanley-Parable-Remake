using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Carsen Decker
//USAGE: Put this on the player object in order to control its movements
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float Gravity;
    public bool CanMove = true;
    
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
        if (CanMove)
        {
            //Uses the character controller to move based on the axis inputs and set movement speed, and the set gravity as well
            cc.Move(new Vector3(tempVel.x * MoveSpeed, cc.velocity.y + -Gravity, tempVel.z * MoveSpeed) * Time.deltaTime);
        }

    }
}
