using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehavior : MonoBehaviour
{
    public static Animator doorAnim; //create a public static animator called doorAnim


    // Start is called before the first frame update
    void Start()
    {
        doorAnim = GetComponent<Animator>(); //set doorAnim to the animator that attached to the floor
    }

    // Update is called once per frame
    void Update()
    {

    }

    void pauseAnimationEvent() //create a method for the animation event, put the in the middle of the animation, which is when the door is opened
    {
        doorAnim.enabled = false; //when hit the event, pause the animatioin so that the door stat open
    }
}
