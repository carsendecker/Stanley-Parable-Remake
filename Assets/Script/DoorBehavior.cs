using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public Animator doorAnim;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            doorAnim.SetTrigger("OpenDoor");
        }

        if(doorAnim.enabled == false)
        {
            if(Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                doorAnim.enabled = true;
            }
        }

    }

    void pauseAnimationEvent()
    {
        doorAnim.enabled = false;
    }
}
