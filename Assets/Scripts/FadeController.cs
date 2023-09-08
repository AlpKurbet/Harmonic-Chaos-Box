using System.Collections;
using UnityEngine;

public class FadeController : MonoBehaviour

{
    // Duration of the fade-out in seconds for the coder to adjust
    public float fadeOutDuration = 1.0f; 
    public GameObject lineMusic;
    public AK.Wwise.Event stopTheLineSounds;


    public void FadeTheVolume()
    {
        //Call fadeout
        StartCoroutine(FadeOutTheVolumeOfEvent());
    }

    private IEnumerator FadeOutTheVolumeOfEvent()
    {
        // Calculate the fade-out start and end volumes
        float startVolume = 1.0f;
        float endVolume = 0.0f;

        // Calculate the fade-out step based on the duration
        float theFadeSpeed = (startVolume - endVolume) / fadeOutDuration;

        // Start fading out all events
        float currentVolume = startVolume;
        while (currentVolume > endVolume)
        {
            // Set the master volume to the current volume !!MUST BE between 0-100!!
            AkSoundEngine.SetRTPCValue("EventVolume", currentVolume * 100f); 

            // Decrease the current volume by the fade step
            currentVolume -= fadeSpeed * Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // An assurance call to pull it to 0 if not happened before
        AkSoundEngine.SetRTPCValue("EventVolume", endVolume * 100f); 
        stopTheLineSounds.Post(lineMusic);
        // Turn on the volume back to 100
        AkSoundEngine.SetRTPCValue("EventVolume", 100f); 


    }
}
