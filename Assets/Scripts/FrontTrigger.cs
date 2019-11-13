using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontTrigger : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        DoorController.back = false;
        DoorController.front = true;
    }

    private void OnTriggerExit(Collider other)
    {
        DoorController.back = false;
        DoorController.front = false;
    }



}
