using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Umnmanager : MonoBehaviour
{
    public Button exclamation, btnCancel;
    public Image investasiPopUp;
    // Start is called before the first frame update
    void Start()
    {
        exclamation.onClick.AddListener(PopUpInvestasi);
        btnCancel.onClick.AddListener(closePopUp);

        investasiPopUp.enabled = false;
        btnCancel.gameObject.SetActive(false);
    }

    private void PopUpInvestasi()
    {
        investasiPopUp.enabled = true;
        exclamation.gameObject.SetActive(false);
        btnCancel.gameObject.SetActive(true);
    }

    private void closePopUp()
    {
        investasiPopUp.enabled = false;
        btnCancel.gameObject.SetActive(false);
        exclamation.gameObject.SetActive(true);
    }

    public void MainSceneKarakter()
    {
        SceneManager.LoadScene("MainScene");
    }
}
