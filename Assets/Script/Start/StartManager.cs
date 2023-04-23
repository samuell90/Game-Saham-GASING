using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartManager : MonoBehaviour
{
    // Start is called before the first frame update
    private int sceneToContinue;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetPrefs()
    {
        //PlayerPrefs.DeleteKey("Tutorial");
        //PlayerPrefs.DeleteKey("TutorialS2");
        //PlayerPrefs.DeleteKey("TutorialS3");
        //PlayerPrefs.DeleteKey("TimerOn");

        PlayerPrefs.DeleteAll();
    }

    public void IntroductionScene()
    {
        if (PlayerPrefs.HasKey("playerName"))
        {
            SceneManager.LoadScene("MainScene");
        }
        else
        {
            SceneManager.LoadScene("IntroductionFrame");
        }

        // frame scene berisi semacam introduction, dkk utk kedepannya
        
    }
}
