using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FrameManager : MonoBehaviour
{
    //introduction
    public GameObject mulaiIntroduction;
    public Button Mulaibtn;
    public GameObject profileInvestorlogo;
    public GameObject profileInvestorDesc;

    private IEnumerator coroutine;

  
    

  

    void Start()
    {
       profileInvestorDesc.SetActive(false);
       profileInvestorlogo.SetActive(false);
       mulaiIntroduction.SetActive(true);
    }
    public void introductionPage()
    {
        mulaiIntroduction.SetActive(false);
        profileInvestorlogo.SetActive(true);
        coroutine = WaitProfile(2.0f);
        StartCoroutine(coroutine);
    }


    public void CharapickScene()
    {
        SceneManager.LoadScene("CharapickScene");
    }

    private IEnumerator WaitProfile(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        profileInvestorlogo.SetActive(false);
        profileInvestorDesc.SetActive(true);
    }

    public void Stage2Scene()
    {
        SceneManager.LoadScene("Stage2Scene");
    }

}
