using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");
        if (DoorController.BdoorAutoClose == true)
        {
            DoorController.Disable = true;
            DoorController.doorAnimator.SetTrigger("autoCloseTrigger");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        DoorController.back = true;
        DoorController.front = false;

        
    }

    private void OnTriggerExit(Collider other)
    {
        

        DoorController.back = false;
        DoorController.front = false;
    }
}
