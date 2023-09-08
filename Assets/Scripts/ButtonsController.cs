using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class ButtonsController : MonoBehaviour
{

    public MeshRenderer droneMesh;

    public TMP_Text droneVisibleText;
    public TMP_Text droneTypeText;

    public GameObject drone1;
    public GameObject drone2;

    public bool theDroneFlag = false;
    
    void Start()
    {
        droneMesh.enabled = true;
    }

    
    void Update()
    {   
        //To make drone 1 visible or invisible
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(droneMesh.enabled)
            {
                droneVisibleText.text = "Drone Not Visible ";
               
                droneMesh.enabled = false;
            }
            else if(!droneMesh.enabled)
            {
                droneVisibleText.text = "Drone Visible ";
               
                droneMesh.enabled = true;
            }
        }
            //For drone switching
            if (Input.GetKeyDown(KeyCode.X))
        {
            theDroneFlag = !theDroneFlag;

            if(theDroneFlag)
            {
               droneTypeText.text = "Currrent Drone : Harmonic Flow";
               AkSoundEngine.SetRTPCValue("DroneVolume", 100f); 
               droneVisibleText.enabled = false; 
               drone2.SetActive(true);
               drone1.SetActive(false);
            }
            else 
            {
                droneTypeText.text = "Currrent Drone : InHarmonic Rumble";
               AkSoundEngine.SetRTPCValue("DroneVolume", 0f); 
               droneVisibleText.enabled = true; 
               drone2.SetActive(false);
               drone1.SetActive(true);
            }
        }

           if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application 
            Application.Quit();
         
        }
    }


}
