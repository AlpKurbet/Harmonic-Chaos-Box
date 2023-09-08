using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AK.Wwise;
using TMPro;

public class TheSwitchController : MonoBehaviour
{   
    //For the screen notification
    public TMP_Text stepText;

    public string wwiseSwitchGroupName = "Steps";
    public string[] switchNames = { "cement", "clock", "metallic", "kick"};

    private int theCurrentSwitch = 0;

    private void Start()
    {
        
    }

    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
        
        theCurrentSwitch++;

            if (theCurrentSwitch >= switchNames.Length)
            {
             theCurrentSwitch = 0;
            }

        ChangeTheSwitch();
        }
    }

    private void ChangeTheSwitch()
    {
        if (theCurrentSwitch >= 0 && theCurrentSwitch < switchNames.Length)

        {
            AkSoundEngine.SetSwitch(wwiseSwitchGroupName, switchNames[theCurrentSwitch], gameObject);
            stepText.text = "Current Step Sound : " + switchNames[theCurrentSwitch];
        }
    }
}
