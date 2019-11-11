using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontDoorOpen : MonoBehaviour
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
        DoorBehavior.doorAnim.SetBool("Front", true); //when player enter the front side of the door, set the bool to true so it will open from this side

        if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            DoorBehavior.doorAnim.SetTrigger("OpenDoor");//when player press E or left click their mouse the door open
        }

        if (DoorBehavior.doorAnim.enabled == false)//if the door is already opened then the animation should be disabled because of the animation event
        {
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                DoorBehavior.doorAnim.enabled = true; //so when the door is already opened and the player press E or left click their mouse the door closed
            }
        }
    }



  

}
