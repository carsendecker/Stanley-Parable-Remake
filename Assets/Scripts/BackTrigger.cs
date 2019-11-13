using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackTrigger : MonoBehaviour
{
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
