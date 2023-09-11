// MouseDrawLineController.cs
using System.Collections;
using UnityEngine;

public class MouseDrawLineController : MonoBehaviour
{
    // The script on the prefab
    LineObjectPositionController lineObjectPositionController;

    // Variables To Position the Line Related to the FPS Controller
    public Camera fpsCamera;
    public Transform fpsCameraTransform;
    public float distance = 5.0f;

    // Flags
    private bool isDrawing = false;

    // The New Line Game Object
    public GameObject theNewlyDrawnLinePrefab;

    private void Start()
    {
        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Check if the left mouse button is pressed and not already drawing
        if (Input.GetMouseButtonDown(0) && !isDrawing)
        {
            
            // Instantiate the prefab in front of the player
            GameObject theNewlyDrawnLine = Instantiate(theNewlyDrawnLinePrefab);
            lineObjectPositionController = theNewlyDrawnLine.GetComponent<LineObjectPositionController>();
            lineObjectPositionController.theDrawingIsTrue = true; 


            Transform prefabTransform = theNewlyDrawnLine.transform;
            Vector3 targetPosition = fpsCameraTransform.position + fpsCameraTransform.forward * distance;
            prefabTransform.position = targetPosition;
            prefabTransform.LookAt(fpsCameraTransform);
            prefabTransform.Rotate(Vector3.up, 180.0f);

            // Start drawing
            isDrawing = true;
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            
            // Stop drawing
            isDrawing = false;
            lineObjectPositionController.clickCount ++;
        }

        if (isDrawing)
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate the movement vector in world space and send it to the newly created prefab
            Vector3 mouseMovement = new Vector3(mouseX, mouseY, 0);
            lineObjectPositionController.theMouseMovementPosition = mouseMovement ;
        }
    }
}
