using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Put on Main Camera
// Lets player use mouse to turn the camera (FPS camera)
public class MouseLook : MonoBehaviour
{
    public float rotateSensitivity;

    private float verticalAngle = 0f; //store verticalAngle in a global variable, not local
    // prevents wraparound between 180 to -180 degree rotation

    private void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Returns 0 if not moving mouse
        float mouseX = Input.GetAxis("Mouse X"); //Horiz. mouse velocity
        float mouseY = Input.GetAxis("Mouse Y"); // Vertical mouse velocity
        
        transform.parent.Rotate(0, mouseX * rotateSensitivity, 0f);

        verticalAngle -= mouseY * rotateSensitivity;
        verticalAngle = Mathf.Clamp(verticalAngle, -80f, 80f);
        
        transform.localEulerAngles = new Vector3(verticalAngle, transform.localEulerAngles.y, 0);
    }
}
