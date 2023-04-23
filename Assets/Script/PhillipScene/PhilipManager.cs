using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PhilipManager : MonoBehaviour
{
    public GameObject panelDeskripsi;
    public Button exclamation, clsBtn;
    void Start()
    {
        panelDeskripsi.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Mainmenu()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void panelDeskripsiOn()
    {
        panelDeskripsi.SetActive(true);
        exclamation.gameObject.SetActive(false);
    }

    public void closeBtn()
    {
        panelDeskripsi.SetActive(false);
        exclamation.gameObject.SetActive(true);
    }

    public void OpenLink()
    {
        Application.OpenURL("https://www.phillip.co.id/");
    }
}
