using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool doorFrontOpen;
    public bool doorBackOpen;
    public static bool front = false;
    public static bool back = false;

    public static Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        doorAnimator = GetComponent<Animator>();
        if (doorFrontOpen == true)
        {
            doorBackOpen = false;
            doorAnimator.Play("OpenedFront");
            doorAnimator.SetBool("closed", false);
            Debug.Log("door open");
        }
        if(doorBackOpen == true)
        {
            doorFrontOpen = false;
            doorAnimator.Play("OpenedBack");
            doorAnimator.SetBool("closed", false);
            Debug.Log("door open");
        }

    }

    // Update is called once per frame
    void Update()
    {
        

        if(front == true)
        {
            doorAnimator.SetBool("front", true);
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                doorAnimator.SetTrigger("pressE");
            }
            
           
        }

        if(back == true)
        {
            doorAnimator.SetBool("front", false);
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                doorAnimator.SetTrigger("pressE");
            }
        }
    }
}
