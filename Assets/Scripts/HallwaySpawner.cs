using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usage: Place at the beginning of a room to spawn a new room prefab at the end of it
[RequireComponent(typeof(Collider))]
public class HallwaySpawner : MonoBehaviour
{
    public GameObject[] Rooms; //DO NOT PUT IN THE CURRENT ROOM THIS IS IN
    public Transform ExitLocationRight, ExitLocationLeft;
    public float ExitRotation;
    public bool PivotOnRight, PivotOnLeft;

    public bool isFirstRoom;
    [HideInInspector] public GameObject PreviousRoom;

    

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        
        if (other.CompareTag("Player"))
        {
            //Initially spawns room on the right corner of the exit door
            int randRoom = Random.Range(0, Rooms.Length);
            GameObject newRoom = Instantiate(Rooms[randRoom], ExitLocationRight.position, Rooms[randRoom].transform.rotation);
            
            newRoom.transform.Rotate(0, ExitRotation, 0);
            
            HallwaySpawner roomScript = newRoom.GetComponentInChildren<HallwaySpawner>();
            roomScript.PreviousRoom = gameObject;

            //If, instead, the new room's pivot is on the left side of the doorway, move the new room over a bit to the other exit corner
            if (roomScript.PivotOnLeft)
            {
                newRoom.transform.position = ExitLocationLeft.position;
            }
            else
            {
                newRoom.transform.position = ExitLocationRight.position;
            }

            if (!isFirstRoom && PreviousRoom != null)
            {
                StartCoroutine(WaitToDestroyPrevious());
            }
        }
    }

    IEnumerator WaitToDestroyPrevious()
    {
        triggered = true;
        yield return new WaitForSeconds(2f);
        Destroy(PreviousRoom);
    }
}
