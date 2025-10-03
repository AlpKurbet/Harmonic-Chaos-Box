using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{

    public bool theClockSee = false;
    private float waitTime;
    void Start()
    {
        waitTime = getTheFloat(30.0f / GlobalVars.theBPM);
        StartCoroutine(ClockTicker());
        Debug.Log("THE WAIT TIME : " + waitTime);
    }

    private IEnumerator ClockTicker()
    {
        while (true) 
        {
            //Debug.Log("THE DIVIDEND IS : " + (60.0f / TheBPM));
            yield return new WaitForSeconds(waitTime); 
            GlobalVars.clockBool = !GlobalVars.clockBool;
            theClockSee = !theClockSee;
        }

    }
       //This gets the whole number part and the frst two dcima part of a float ex: 8.678674 -> 8.67
      private float getTheFloat(float number)
      {
        int integerPart = Mathf.FloorToInt(number);

            // Get the first decimal of the number
        int firstDecimal = Mathf.FloorToInt((number - integerPart) * 100);

        // Combine the integer part and the first decimal to get the desired result
        float result = integerPart + (float)firstDecimal / 100;  
        return result;
      }

   
}
