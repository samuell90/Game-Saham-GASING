using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    //UI
    public GameObject panelStage, stage3Btn;
    public GameObject mainCity;
    public GameObject quiz1;
    public GameObject quiz2;
    public int indexbtnQuiz = 0, indexStage2Berhasil = 0; //ccek quiz sudah atau blm
    public TextMeshProUGUI uangText;
    public Text[] stringNama = new Text[2];
    public Image[] karakterSprite = new Image[2];

    public Sprite[] buttonSprite = new Sprite[2];
    public Button quizBtn;

    public GameObject TemplateBackground;

    private int uang=0;

    //stage 1
    private IEnumerator coroutine;
    public Animator gedungSIPF,Rumah, UMN, gedungPhilip;
    public Image[] bangunan;

    //save scene
    private int currentSceneIndex;

    void Start()
    {
        animasiGedung();
        bangunanAwal();
        //cek index btn quiz atau stage 2
        indexbtnQuiz = PlayerPrefs.GetInt("indexQuiz");
        indexStage2Berhasil = PlayerPrefs.GetInt("indexStage2Berhasil");
        indexStage2();
        indexButtonQuiz();

        if (PlayerPrefs.HasKey("Uang"))
        {
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
            uang = PlayerPrefs.GetInt("Uang");
        }
        quiz1.SetActive(false);
        quiz2.SetActive(false);
        mainCity.SetActive(true);
        panelStage.SetActive(false);

        //ambil data gambar dan nama saat panel quiz
        stringNama[0].text = PlayerPrefs.GetString("playerName");
        stringNama[1].text = PlayerPrefs.GetString("playerName");
        karakterSprite[0].sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("characterSprite")+"-head");
        karakterSprite[1].sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("characterSprite")+"-head");

        if (PlayerPrefs.GetString("RumahVisited").Equals("true") && PlayerPrefs.GetString("GaleriVisited").Equals("true") 
            && PlayerPrefs.GetString("PhillipVisited").Equals("true") && PlayerPrefs.GetString("BEIVisited").Equals("true"))
        {
            quizBtn.interactable = true;
        }
        else
        {
            quizBtn.interactable = false;
        }
    }

    public void saveSceneLocation()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt("SavedScene", currentSceneIndex);
    }

    public void indexButtonQuiz()
    {
        if (indexbtnQuiz == 0)
        {
            quizBtn.image.sprite = buttonSprite[0];
        }
        else if (indexbtnQuiz == 1)
        {
            quizBtn.image.sprite = buttonSprite[1];
        }
    }

    public void animasiGedung()
    {
        if (PlayerPrefs.GetString("BEIVisited").Equals("true"))
        {
            gedungSIPF.SetBool("isSIPF", false);
        }

        if (PlayerPrefs.GetString("RumahVisited").Equals("true"))
        {
            Rumah.SetBool("isRumah", false);
        }

        if (PlayerPrefs.GetString("GaleriVisited").Equals("true"))
        {
            UMN.SetBool("isUMN", false);
        }

        if (PlayerPrefs.GetString("PhillipVisited").Equals("true"))
        {
            gedungPhilip.SetBool("isPhillip", false);
        }
    }

    public void indexStage2()
    {
        if (indexStage2Berhasil == 0)
        {
            stage3Btn.SetActive(false);
        }
        else if(indexStage2Berhasil == 1)
        {
            stage3Btn.SetActive(true);
        }
    }

    public void bangunanAwal()
    {
        if (!PlayerPrefs.GetString("firstbuilding").Equals("true"))
        {
            bangunan[0].raycastTarget = false;
            bangunan[1].raycastTarget = false;
            bangunan[2].raycastTarget = false;
        }
        else
        {
            bangunan[0].raycastTarget = true;
            bangunan[1].raycastTarget = true;
            bangunan[2].raycastTarget = true;
        }
        
    }

    public void stage2()
    {
        saveSceneLocation();
        SceneManager.LoadScene("Stage2Scene");
    }

    public void stage3()
    {
        saveSceneLocation();
        SceneManager.LoadScene("Stage3Scene");
    }
    public void btnQuiz()
    {
        if (indexbtnQuiz == 0)
        {
            mainCity.SetActive(false);
            quiz1.SetActive(true);
            //index pas dia mencet quiz maka button quiz berganti pilih
            indexbtnQuiz = 1;
            PlayerPrefs.SetInt("indexQuiz", indexbtnQuiz);
        }

        else if (indexbtnQuiz == 1)
        {
            panelStage.SetActive(true);
        }
        
    }

    public void RumahKarakter()
    {
        if (!PlayerPrefs.HasKey("RumahVisited"))
        {
            uang += 2500000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("RumahVisited", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        
        SceneManager.LoadScene("RumahScene");
        
    }

    public void GaleriUmnKarakter()
    {
        if (!PlayerPrefs.HasKey("GaleriVisited"))
        {
            uang += 2500000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("GaleriVisited", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        SceneManager.LoadScene("GaleriUMNScene");
        
    }

    public void PhillipKarakter()
    {
        if (!PlayerPrefs.HasKey("PhillipVisited"))
        {
            uang += 2500000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("PhillipVisited", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        SceneManager.LoadScene("PhillipScene");
    }

    public void SippfKarakter()
    {
        if (!PlayerPrefs.HasKey("BEIVisited"))
        {
            uang += 2500000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("BEIVisited", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        PlayerPrefs.SetString("firstbuilding", "true");
        SceneManager.LoadScene("BEIScene");
        
        
    }
    public void closePanelStage()
    {
        panelStage.SetActive(false);
    }

    // public void OpenProfile()
    // {
    //     profilePage.SetActive(true);
    // }
    
    // public void CloseProfile()
    // {
    //     profilePage.SetActive(false);
    // }

    public void quizjawabanBenar()
    {
        if (!PlayerPrefs.HasKey("quiz1jawabanBenar"))
        {
            uang += 25000000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("quiz1jawabanBenar", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        quiz2.SetActive(true);
        quiz1.SetActive(false);
        
        
    }
    public void quizjawabanSalah()
    {
        quiz2.SetActive(true);
        quiz1.SetActive(false);
    }

    public void quiz2jawabanBenar()
    {
        if (!PlayerPrefs.HasKey("quiz2jawabanBenar"))
        {
            uang += 25000000;
            PlayerPrefs.SetInt("Uang", uang);
            PlayerPrefs.SetString("quiz2jawabanBenar", "true");
            uangText.text = string.Format("{0:n0}", PlayerPrefs.GetInt("Uang"));
        }
        SceneManager.LoadScene("Stage2Frame");
    }
    public void quiz2jawabanSalah()
    {
        SceneManager.LoadScene("Stage2Frame");
    }

    // private IEnumerator WaitStage(float waitTime)
    // {
    //     yield return new WaitForSeconds(waitTime);
    //     
    // }

    /* save and load
        PlayerPrefs.SetInt("", values);
        PlayerPrefs.Save();

        load
        PlayerPrefs.GetInt("");
    */
}
