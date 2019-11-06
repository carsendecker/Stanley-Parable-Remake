using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackDoorOpen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        DoorBehavior.doorAnim.SetBool("Front", false); //when player enter the back side of the door, set the bool to true so it will open from this side


        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            DoorBehavior.doorAnim.SetTrigger("OpenDoor");//when player press E or left click their mouse the door open
        }

        if (DoorBehavior.doorAnim.enabled == false)//if the door is already opened then the animation should be disabled because of the animation event
            {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))//so when the door is already opened and the player press E or left click their mouse the door closed
            {
                DoorBehavior.doorAnim.enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if you want the door to close automatically just un-comment the code below and delete line 28-34 and remove the condition for the transition from doorAnim -> DoorClosed

        /*if (DoorBehavior.doorAnim.enabled == false)
        {
            
                DoorBehavior.doorAnim.enabled = true;
            
        }*/

    }

}
