using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Stage1Frame : MonoBehaviour
{
    public GameObject stage1Desc;
    public GameObject stage1Logo;

    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
        stage1Desc.SetActive(false);
        stage1Logo.SetActive(true);
        coroutine = WaitStage(2.0f);
        StartCoroutine(coroutine);

    }

    private IEnumerator WaitStage(float waitTime)
    {
    yield return new WaitForSeconds(waitTime);
       stage1Logo.SetActive(false);
       stage1Desc.SetActive(true);
    }

    public void MainScene()
    {
        SceneManager.LoadScene("MainScene");
    }


}
