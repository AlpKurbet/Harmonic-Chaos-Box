using System.Collections;
using UnityEngine;

public class MouseDrawLineController : MonoBehaviour
{
    //Acces The FadeController Script 
    public FadeController fadeController;

    //The WWise Switch Components
    public string wwiseSwitchGroupName = "LineEvent";
    private int speedExceedCounter = 0;
    
    //Line Material Variables
    public Material lineMaterial;
    public float lineWidth = 0.1f;
    public Color currentColor;

    //Line Renderer Variables 
    public LineRenderer lineRenderer;
    public Transform lineRendererTransform; 

    //Variables To Position the Line Related to the FPS Controller
    public Camera fpsCamera; 
    public Transform fpsCameraTransform; 
    public float distance = 5.0f; 

    //The WWise Events
    public AK.Wwise.Event theLineSounds;
    public AK.Wwise.Event stopTheLineSounds;


    //Flags
    public bool isDrawing = false;
    private bool mousePressed = false;
    private bool theMusicIsPlaying = false;
    private bool theSectionEnded = true;
    

    //The Line Music Game Object
    public GameObject lineMusic;
    public Transform lineMusicTransform;

    //To calcualte the Mouse Velocity
    private Vector3 prevMousePosition;
    private Vector3 unSmoothedVelocity;
    private Vector3 smoothedVelocity;

    private float smoothedMouseVelocityFloat = 0f;
    private float unSmoothedMouseVelocityFloat = 0f;
    public float smoothingFactor = 0.1f;

    private void Start()
    {
        //Set the line material's attributes to the line renderer, overwrite them
        lineRenderer.material = lineMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 0;

        //Reset the WWise Event
        AkSoundEngine.PostEvent("Reset_MissionBriefing", lineMusic);

        // Lock the mouse cursor to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Mouse position
        
    }

    private void Update()
    {
    // Check if the left mouse button is pressed
    if (Input.GetMouseButtonDown(0))
    {

        //Pull the flag up to change the material's attributes once
        mousePressed = true;

        //Pull the flag up for continous drawing
        isDrawing = true;
        
 
        
        //Post the WWise Event
        theLineSounds.Post(lineMusic, (uint)AkCallbackType.AK_EndOfEvent, TheSectionEnd, null);
        


        //Reset the line
        if(theSectionEnded)
        {
            lineRenderer.positionCount = 0;     
        }
        theSectionEnded = false;
        //Debug.Log("Drawing");
    }

    // Check if the left mouse button is released
    if (Input.GetMouseButtonUp(0))
    {
        //Assign a random clolur here so that each instance has a different colour
        currentColor = new Color(Random.value, Random.value, Random.value);
        lineMaterial.color = currentColor;

        //Fade out and stop only the WWise events on this game object
        fadeController.FadeTheVolume();
        
        //Pull the flag down to stop drawing
        isDrawing = false;

        //Reset the line when finished
        lineRenderer.positionCount = 0;     
        
        //Debug.Log("Stopped Drawing");
    }

        if (isDrawing)
        {
            

            if(unSmoothedMouseVelocityFloat >= 35.0f)
            {
                speedExceedCounter ++;
            }

            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            // Calculate the movement vector in world space
            Vector3 mouseMovement = new Vector3(mouseX, mouseY, 0);

            // Convert the mouse movement to world space by taking into account the camera's orientation
            
            //mouseMovement = fpsCameraTransform.TransformDirection(mouseMovement);

            // Calculate a new position based on mouse input
            Vector3 newPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1) + mouseMovement;

            
            //Get the mouse velocity as a Vector 3 and get the float magnitude
            unSmoothedVelocity = (newPosition - prevMousePosition) / Time.deltaTime;

            
            // Smooth the velocity
            smoothedVelocity = Vector3.Lerp(smoothedVelocity, unSmoothedVelocity, smoothingFactor);

            //To keep it reasonable and be able to send it to the RTPC Change
            smoothedMouseVelocityFloat = Mathf.Clamp(smoothedVelocity.magnitude, 0f, 100f);

            //As ChangeLineMaterialAttributes() does it's own Linear Interpolation, we need the unsmoothed version
            unSmoothedMouseVelocityFloat = Mathf.Clamp(unSmoothedVelocity.magnitude, 0f, 100f);


            //Set the RTPC to Velocity
            AkSoundEngine.SetRTPCValue("MouseVelocity", smoothedMouseVelocityFloat);

            //Send the velocity value to the ChangeLineMaterialAttributes()
            ChangeLineMaterialAttributes(unSmoothedMouseVelocityFloat,unSmoothedMouseVelocityFloat,unSmoothedMouseVelocityFloat);
            //Debug.Log("Mouse Velocity UnSmoothed: " + unSmoothedMouseVelocityFloat);
            //Debug.Log("Mouse Velocity Smoothed: " + smoothedMouseVelocityFloat);    

            prevMousePosition = newPosition;
            
            //Get the last drawn point's position
            Vector3 theTipOfTheLine = lineRenderer.GetPosition(lineRenderer.positionCount - 1);
            Debug.Log("THE LAST POINT: " + theTipOfTheLine);
            // Drag the line sound to the new position
            lineMusicTransform.position = theTipOfTheLine;
            lineMusicTransform.LookAt(theTipOfTheLine);
            lineMusicTransform.rotation = Quaternion.LookRotation(theTipOfTheLine - lineMusicTransform.position);

            Debug.Log("THE LINE MUSIC: " + lineMusicTransform.position);

            //Continue drawing
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);
            

        }
        else
        {
            if(theSectionEnded)
            {
                //This block keeps the line intact and in front of the camera as well as fix the orientations so that the axis' don't go reversed
                Vector3 targetPosition = fpsCameraTransform.position + fpsCameraTransform.forward * distance;
                lineRendererTransform.position = targetPosition;
                lineRendererTransform.LookAt(fpsCameraTransform);
                lineRendererTransform.rotation = fpsCameraTransform.rotation;
                
            }
        }
    }

    private void TheSectionEnd(object in_cookie, AkCallbackType in_type, object in_info)
    {
        //Adjust the flags at the end
        theSectionEnded = true;
        ChangeTheLineSound();
    }



private void ChangeLineMaterialAttributes(float colourFloat, float smoothnessFloat, float metallicFloat)
{
    // Ensure the values are within the valid range that is 0 - 100
    colourFloat = Mathf.Clamp(colourFloat, 0.0f, 100.0f);
    smoothnessFloat = Mathf.Clamp(smoothnessFloat, 0.0f, 100.0f);
    metallicFloat = Mathf.Clamp(metallicFloat, 0.0f, 100.0f);

    // Normalize the values
    colourFloat /= 100.0f;
    smoothnessFloat /= 100.0f;
    metallicFloat /= 100.0f;

    // Create a new color with HSV to RGB mapping
    Color targetColor = Color.HSVToRGB(colourFloat, 1.0f, 1.0f);

    // Smoothly interpolate between the current color and the target color
    currentColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * 5f);

    // Assign the attributes
    // Assign the new color
    lineMaterial.color = currentColor;

    // Assign the new smoothness value
    lineMaterial.SetFloat("_Smoothness", smoothnessFloat);

    // Assign the new metallicness value
    lineMaterial.SetFloat("_Metallic", metallicFloat);
}

private void ChangeTheLineSound()
{   
    if(speedExceedCounter > 70)
    {
    //Debug.Log("I go with Option 2 since the Speed Counter is :" + speedExceedCounter);
    AkSoundEngine.SetSwitch(wwiseSwitchGroupName, "Option2", lineMusic);
    }
    else
    {
    //Debug.Log("I go with Option 1 since the Speed Counter is :" + speedExceedCounter);    
    AkSoundEngine.SetSwitch(wwiseSwitchGroupName,"Option1", lineMusic);
    }
     //Reset the speedExceed Counter that controls the Switch
    speedExceedCounter = 0;
}
}

