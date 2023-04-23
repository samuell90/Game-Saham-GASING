using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Sc_SelectionManager : MonoBehaviour
{
    public Sprite[] character = new Sprite[6];
    private int selectedCharacter = 0;
    public Button rightArrow;
    public Button leftArrow;
    public Image imageCharacter;

    public TMP_InputField userNameField;
    public TMP_InputField userAgeField;
    public TMP_InputField userJobField;

    private string userName;
    private int userAge;
    private string userJob;

    public TextMeshProUGUI PlayerNameConfig;
    public TextMeshProUGUI PlayerAgeConfig;
    public TextMeshProUGUI PlayerJobConfig;

    public Button btnLanjut;
    public GameObject confirmationPanel;
    public Image finalSprite;

    private string characterSprite;

   
    // Start is called before the first frame update
    void Start()
    {
        //inisialisasi function onclick pada setiap button warna
        rightArrow.onClick.AddListener(RightArrowClick);
        leftArrow.onClick.AddListener(LeftArrowClick);

        confirmationPanel.SetActive(false);

        userNameField.onValueChanged.AddListener(delegate { ChangeUserInfo("userName"); });
        userAgeField.onValueChanged.AddListener(delegate { ChangeUserInfo("userAge"); });
        userJobField.onValueChanged.AddListener(delegate { ChangeUserInfo("userJob"); });

        

        characterSprite = character[0].name;
        PlayerPrefs.SetString("characterSprite", characterSprite);

        //validasi input
        userAgeField.characterValidation = TMP_InputField.CharacterValidation.Integer;
        userNameField.characterLimit = 12;
        userAgeField.characterLimit = 2;
        userJobField.characterLimit = 30;
    }

    void Update() 
    {
        //PlayerNameConfig.text = PlayerPrefs.GetString("playerName");
    }

    private void RightArrowClick()
    {
        if(selectedCharacter == 5)
        {
            selectedCharacter = 0;
            
        }
        else
        {
            selectedCharacter++;
        }
        
        imageCharacter.sprite = character[selectedCharacter];
        finalSprite.sprite = imageCharacter.sprite;
        characterSprite = finalSprite.sprite.name;
        PlayerPrefs.SetString("characterSprite", characterSprite);
    }

    private void LeftArrowClick()
    {
        if(selectedCharacter == 0)
        {
            //do something
            selectedCharacter = 5;
        }
        else
        {
            selectedCharacter--;
        }
        imageCharacter.sprite = character[selectedCharacter];
        finalSprite.sprite = imageCharacter.sprite;
        characterSprite = finalSprite.sprite.name;
        PlayerPrefs.SetString("characterSprite", characterSprite);
    }

    public void Lanjut()
    {
        PlayerNameConfig.text = "Nama : " + userNameField.text;
        PlayerAgeConfig.text = "Usia : " + userAgeField.text;
        PlayerJobConfig.text = "Pekerjaan : " + userJobField.text;

        if(userNameField.text != "" && userAgeField.text != "" && userJobField.text != "")
        {
            confirmationPanel.SetActive(true);
            btnLanjut.interactable = false;

            rightArrow.interactable = false;
            leftArrow.interactable = false;

            userNameField.enabled = false;
            userAgeField.enabled = false;
            userJobField.enabled = false;
        }

    }

    public void Batal()
    {
        confirmationPanel.SetActive(false);

        rightArrow.interactable = true;
        leftArrow.interactable = true;

        userNameField.enabled = true;
        userAgeField.enabled = true;
        userJobField.enabled = true;

        btnLanjut.interactable = true;
        
    }


    // public void Awake() 
    // {
    //     userName = PlayerPrefs.GetString ("playerName", "Player");
    //     userNameField.text = userName; 
    // }

    public void ChangeUserInfo(string userInfo)
    {
        if(userInfo.Equals("userName"))
        {
            userName = userNameField.text;
            PlayerPrefs.SetString("playerName", userName);
        }
        else if (userInfo.Equals("userAge"))
        {
            int.TryParse(userAgeField.text.ToString(), out userAge);
            userAgeField.text = userAge.ToString();
            
            PlayerPrefs.SetInt("playerAge", userAge);
        }
        else if(userInfo.Equals("userJob"))
        {
            userJob = userJobField.text;
            PlayerPrefs.SetString("playerJob", userJob);
        }
        
    }

    public void FrameScene()
    {
        SceneManager.LoadScene("Stage1Frame");
    }
}

