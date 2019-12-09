using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrigger : MonoBehaviour
{
    public DoorController DC;

    private void OnTriggerEnter(Collider other)
    {
//        Debug.Log("enter");
        if (DC.AutoCloseBack == true)
        {
            DC.Disable = true;
            DC.doorAnimator.SetTrigger("autoCloseTrigger");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        DC.back = true;
        DC.front = false;

        
    }

    private void OnTriggerExit(Collider other)
    {
        

        DC.back = false;
        DC.front = false;
    }
}
