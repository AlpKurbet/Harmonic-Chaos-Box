using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroSceneController : MonoBehaviour
{
    public GameObject howToPanel;
    private bool activateToggle = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Quit the application
            Application.Quit();
         
        }
    }

    public void LoadTheMainScene()
    {
        //Stop all Wwise sounds and pass scene
        StopAllSounds();
        SceneManager.LoadScene(1);
    }

       private void StopAllSounds()
    {
        AkSoundEngine.StopAll();
    }

    public void EnableOrDisableTheHowToPanel()
    {
        if(activateToggle)
        {
            activateToggle = false;
            howToPanel.SetActive(false);
        }
        else
        {
            activateToggle = true;
            howToPanel.SetActive(true);
        }
        
    }
}
