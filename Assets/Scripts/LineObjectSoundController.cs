using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineObjectSoundController : MonoBehaviour
{   
    public GameObject theLineMusicObject;
    public GameObject theLineItself;

    public AK.Wwise.Event theLineSounds;
    public List<AK.Wwise.Event> theLineSoundsList = new List<AK.Wwise.Event>();

    private bool currentlyPlayingSound = false;
    public bool destroyThisObject = false;
    public bool syncedWithClock = false;

    public int randomIndex;

    void OnEnable()
    {
        //GlobalVars.clockBool = false;
        AkSoundEngine.PostEvent("Reset_MissionBriefing", theLineMusicObject);
        
        if(GlobalVars.soloActive)
        {
            randomIndex = Random.Range(0, (theLineSoundsList.Count-1));
        }
        else
        {
            randomIndex = Random.Range(0, (theLineSoundsList.Count));
        }

        if(randomIndex == 2)
        {
            GlobalVars.soloActive = true;
        }
    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
            if(GlobalVars.clockBool)
        {
            syncedWithClock = true;
        }

        if(!currentlyPlayingSound && syncedWithClock)
        {
            currentlyPlayingSound = true;
            theLineSoundsList[randomIndex].Post(theLineMusicObject, (uint)AkCallbackType.AK_EndOfEvent, TheSectionEnd, null);
        }
    }

    private void TheSectionEnd(object in_cookie, AkCallbackType in_type, object in_info)
    {
        if(destroyThisObject)
        {
            Destroy(theLineItself);
            if(randomIndex == 2)
            {
                GlobalVars.soloActive = false;
            }
        }
        currentlyPlayingSound = false;
    }


}
