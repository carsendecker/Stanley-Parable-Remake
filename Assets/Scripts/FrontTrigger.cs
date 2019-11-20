using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTrigger : MonoBehaviour
{
    public DoorController DC;
    private void OnTriggerEnter(Collider other)
    {
        
        if (DC.AutoCloseFront == true)
        {
            DC.Disable = true;
            DC.doorAnimator.SetTrigger("autoCloseTrigger");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        DC.back = false;
        DC.front = true;

    }

    private void OnTriggerExit(Collider other)
    {
       
        DC.back = false;
        DC.front = false;
    }



}
