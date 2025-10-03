// LineObjectPositionController.cs
using UnityEngine;

public class LineObjectPositionController : MonoBehaviour
{

    //The LineObjectSoundController.cs Script
    public LineObjectSoundController lineObjectSoundController;

    // Drawing Bools & Vars
    public bool theDrawingIsTrue = false;
    public int clickCount = 1;

    // The Line Renderer in the prefab
    public LineRenderer lineRenderer;

    // Line Material Variables
    public Material lineMaterial;
    public Material newlyCreatedMaterial;
    public float lineWidth = 0.1f;
    public Color currentColor;

    // Vector 3 vars
    public Vector3 theMouseMovementPosition;

    // To calculate the Mouse Velocity
    private Vector3 prevMousePosition;
    private Vector3 unSmoothedVelocity;
    private Vector3 smoothedVelocity;

    public float smoothedMouseVelocityFloat = 0f;
    public float unSmoothedMouseVelocityFloat = 0f;
    public float smoothingFactor = 0.1f;

    //WWise Sound Game Object 
    public GameObject WWiseSoundObject;
    public Transform WWiseSoundObjectTransform;
    public float delayForSpeedTimer = 0.02f;
    public float speedTimer = 0f;
    public int thePositionCountForSpeed;
    public float speedMultiplier = 1.4f;

    public bool stopTheSoundMove = false;

    private void Start()
    {
        newlyCreatedMaterial = Instantiate(lineMaterial);
        lineRenderer.material = newlyCreatedMaterial;
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.positionCount = 0;

        // Assign a random color here so that each instance has a different color
        currentColor = new Color(Random.value, Random.value, Random.value);
        newlyCreatedMaterial.color = currentColor;

        delayForSpeedTimer = 3.6f / GlobalVars.theBPM;
    }

    // Update is called once per frame
    void Update()
    {
        if (clickCount <= 1 && theDrawingIsTrue)
        {
            stopTheSoundMove = false;
            speedMultiplier = 1.2f;
            // Calculate a new position based on mouse input
            Vector3 newPosition = lineRenderer.GetPosition(lineRenderer.positionCount - 1) + theMouseMovementPosition;

            if(prevMousePosition != newPosition)
            {
                // Get the mouse velocity as a Vector 3 and get the float magnitude
            unSmoothedVelocity = (newPosition - prevMousePosition) / Time.deltaTime;

            // Smooth the velocity
            smoothedVelocity = Vector3.Lerp(smoothedVelocity, unSmoothedVelocity, smoothingFactor);

            // To keep it reasonable and be able to send it to the RTPC Change
            smoothedMouseVelocityFloat = Mathf.Clamp(smoothedVelocity.magnitude, 0f, 100f);

            // As ChangenewlyCreatedMaterialAttributes() does its own Linear Interpolation, we need the unsmoothed version
            unSmoothedMouseVelocityFloat = Mathf.Clamp(unSmoothedVelocity.magnitude, 0f, 100f);

            prevMousePosition = newPosition;

            // Send the velocity value to the ChangenewlyCreatedMaterialAttributes()
            ChangenewlyCreatedMaterialAttributes(unSmoothedMouseVelocityFloat, unSmoothedMouseVelocityFloat, unSmoothedMouseVelocityFloat);

      

            // Continue drawing
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, newPosition);

            //Debug.Log("ACTUAL : " + lineRenderer.positionCount);
            }
            
        }
        else
        {   if(lineRenderer.positionCount > 1000)
            {
                speedMultiplier = 4.0f;
            }
            else if(lineRenderer.positionCount > 2500)
            {
                speedMultiplier = 8.0f;
            }

            else
            {
                speedMultiplier = 2.8f;
            }



            if(thePositionCountForSpeed >= (lineRenderer.positionCount - 3))
            {
                stopTheSoundMove = true;
                lineObjectSoundController.destroyThisObject = true;
            }
        }

        
        speedTimer += Time.deltaTime;
        //if((speedTimer * (4.5f + (smoothedMouseVelocityFloat/55))) > delayForSpeedTimer)
        if((speedTimer * (speedMultiplier + smoothedMouseVelocityFloat/40.0f)) > delayForSpeedTimer)
        {
            Debug.Log("SPEEDD : " + thePositionCountForSpeed + "TheCurrent Index : " + (lineRenderer.positionCount - 4) + "Velocity: " + (smoothedMouseVelocityFloat/40.0f));
            if(!stopTheSoundMove)
            {
                thePositionCountForSpeed ++;
            }
            speedTimer = 0.0f;
        }
        
        // Get the last drawn point's position
        Vector3 theSoundPosition = lineRenderer.GetPosition(thePositionCountForSpeed - 3);
            
        // Drag the line sound to the new position
        WWiseSoundObjectTransform.position = theSoundPosition;
        WWiseSoundObjectTransform.LookAt(theSoundPosition);
    }



    private void ChangenewlyCreatedMaterialAttributes(float colourFloat, float smoothnessFloat, float metallicFloat)
    {
        // Ensure the values are within the valid range that is 0 - 100
        colourFloat = Mathf.Clamp(colourFloat, 0.0f, 222.0f);
        smoothnessFloat = Mathf.Clamp(smoothnessFloat, 0.0f, 222.0f);
        metallicFloat = Mathf.Clamp(metallicFloat, 0.0f, 222.0f);

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
        newlyCreatedMaterial.color = currentColor;

        // Assign the new smoothness value
        newlyCreatedMaterial.SetFloat("_Smoothness", smoothnessFloat);

        // Assign the new metallicness value
        newlyCreatedMaterial.SetFloat("_Metallic", metallicFloat);
    }
}
