using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SIPFManager : MonoBehaviour
{
    public Button exclamation, btnCancel;
    public Image investasiPopUp;
    // Start is called before the first frame update
    void Start()
    {
        exclamation.onClick.AddListener(PopUpInvestasi);
        btnCancel.onClick.AddListener(closePopUp);

        investasiPopUp.gameObject.SetActive(false);
        //btnCancel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MainSceneKarakter()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void PopUpInvestasi()
    {
        investasiPopUp.gameObject.SetActive(true);
        exclamation.gameObject.SetActive(false);
    }

    private void closePopUp()
    {
        investasiPopUp.gameObject.SetActive(false);
        exclamation.gameObject.SetActive(true);
    }

    public void OpenLinkISIPF()
    {
        Application.OpenURL("https://www.indonesiasipf.co.id/");
    }
}
