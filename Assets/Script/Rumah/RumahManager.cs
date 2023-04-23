using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RumahManager : MonoBehaviour
{
    public Image karakter;
    public Button exclamation, btnCancel;
    public Image investasiPopUp;
    // Start is called before the first frame update
    void Start()
    {
        exclamation.onClick.AddListener(PopUpInvestasi);
        btnCancel.onClick.AddListener(closePopUp);

        //karakter.sprite = globalVar.characterSprite;
        karakter.sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("characterSprite"));
        investasiPopUp.enabled = false;
        btnCancel.gameObject.SetActive(false);
    }

    public void MainSceneKarakter()
    {
        SceneManager.LoadScene("MainScene");
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
}
