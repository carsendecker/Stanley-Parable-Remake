using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Usage: Place at the beginning of a room to spawn a new room prefab at the end of it
[RequireComponent(typeof(Collider))]
public class HallwaySpawner : MonoBehaviour
{
    public GameObject[] Rooms; //DO NOT PUT THE CURRENT ROOM INTO THIS ARRAY TO PREVENT DUPLICATES
    public Transform ExitLocation;
    public float ExitRotation;

    //Do not touch, only public in inspector for the very first room to delete the stairwell
    public GameObject PreviousRoom;

    private bool triggered;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered)
            return;
        
        if (other.CompareTag("Player"))
        {
            //Spawns room at the exit door
            int randRoom = Random.Range(0, Rooms.Length);
            GameObject newRoom = Instantiate(Rooms[randRoom], ExitLocation.position, Rooms[randRoom].transform.rotation);

            //Rotates based on the current room's rotation, and if its exit faces a different direction than its entrance
            newRoom.transform.rotation = transform.parent.parent.rotation;
            newRoom.transform.Rotate(0, ExitRotation, 0);
            
            HallwaySpawner roomScript = newRoom.GetComponentInChildren<HallwaySpawner>();
            roomScript.PreviousRoom = transform.parent.parent.gameObject;

            if (PreviousRoom != null)
            {
                StartCoroutine(WaitToDestroyPrevious());
            }
        }
    }

    /// <summary>
    /// Destroys previous room in order to prevent the hallway from running into itself,
    /// and to not lag the scene with a bunch of stuff
    /// </summary>
    IEnumerator WaitToDestroyPrevious()
    {
        triggered = true;
        yield return new WaitForSeconds(2f);
        Destroy(PreviousRoom);
    }
}
