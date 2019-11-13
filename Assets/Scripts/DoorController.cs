using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool doorFrontOpen;
    public bool doorBackOpen;
    public bool DisablePlayerCon;
    public bool AutoCloseFront;
    public bool AutoCloseBack;
    public static bool Disable;
    public static bool FdoorAutoClose;
    public static bool BdoorAutoClose;
    public static bool front = false;
    public static bool back = false;

    public static Animator doorAnimator;

    // Start is called before the first frame update
    void Start()
    {
        FdoorAutoClose = AutoCloseFront;
        BdoorAutoClose = AutoCloseBack;
        Disable = DisablePlayerCon;

        /*if (AutoCloseFront == true) 
        {
            FdoorAutoClose = true;
        }
        else if (AutoCloseFront == false)
        {
            FdoorAutoClose = false;
        }

        if (AutoCloseBack == true)
        {
            BdoorAutoClose = true;
        }
        else if (AutoCloseBack == false)
        {
            BdoorAutoClose = false;
        }

        if (DisablePlayerCon == true)
        {
            Disable = true;
        }
        else if (DisablePlayerCon == false)
        {
            Disable = false;
        }*/


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
       

        if (Disable == false)
        {
            
            if (front == true)
            {
                doorAnimator.SetBool("front", true);
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    doorAnimator.SetTrigger("pressE");
                }


            }

            if (back == true)
            {
                doorAnimator.SetBool("front", false);
                if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
                {
                    doorAnimator.SetTrigger("pressE");
                }
            }
        }
    }
}
