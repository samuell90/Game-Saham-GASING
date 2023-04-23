using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;


public class Stage2Manager : MonoBehaviour
{
     public Image karakter;
     public Image warnaStatus;
     public TextMeshProUGUI namaKarakter;
     public TextMeshProUGUI avgPriceText;
     public TextMeshProUGUI priceText;
     public TextMeshProUGUI valueText;
     public TextMeshProUGUI namaPerusahaanText;
     public TextMeshProUGUI ownedSahamText;
     public TMP_InputField volumeText;

    //button left and right
    public Button leftBtn, rightBtn;
    public ScrollRect scrollSaham;
     [SerializeField] TextMeshProUGUI uangText;
     public TextMeshProUGUI profitText;
     private int uang=0, modalAwal=0;
     public GameObject panelNotes;
     public Button[] saham = new Button[15];
     public TextMeshProUGUI[] sahamNameText = new TextMeshProUGUI[15];
     public Sprite[] spriteWarnaStatus = new Sprite[3];
     public Button sellBtn, buyBtn, notesBtn, close;
     public Sprite[] sahamColour = new Sprite[3];
     public Sprite[] statusColour = new Sprite[3];
     public int randomNumberExample;
     private int selectedSahamIndex=0;
     private int volTemp=0;
     private int valueTemp = 0;
    //countdown
     public Text timerText;
     public float timeValue = 900;
     private int  ownedSaham;
     private int savedListCount;

     //continuescene
     private int currentSceneIndex;
     public int tenPercentModal, profit=0;

     //tanggalrealtime
     public TextMeshProUGUI waktu;
     public TextMeshProUGUI hari;
     public GameObject berhasil, gagal, finishGame, panelbackStage1;
     //index berhasil stage2
     public int indexStage2Berhasil = 0;
    //arraylist berisi saham
    List<SahamS2> DaftarSahamS2;



    void Start()
    {
        SeedSaham();
        AddButtonListenerSaham();
        volumeText.onValueChanged.AddListener(delegate { ValueChanged(); });

        buyBtn.onClick.AddListener(delegate { BuySellSaham(true); });
        sellBtn.onClick.AddListener(delegate { BuySellSaham(false); });
        rightBtn.onClick.AddListener(delegate { ScrollSaham(true); });
        leftBtn.onClick.AddListener(delegate { ScrollSaham(false); });

        finishGame.SetActive(false);
        panelNotes.SetActive(false);
        panelbackStage1.SetActive(false);
        
        karakter.sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("characterSprite")+"-head");
        namaKarakter.text = PlayerPrefs.GetString("playerName");
        volumeText.characterValidation = TMP_InputField.CharacterValidation.Integer;
        volumeText.characterLimit = 3;


        if (PlayerPrefs.HasKey("modalS2"))
        {
            uang = PlayerPrefs.GetInt("modalS2");
        }
        else
        {
            uang = PlayerPrefs.GetInt("Uang");
            PlayerPrefs.SetInt("modalS2", uang);
        }
        modalAwal = uang;
        

        MoneyChangeValue();
        OwnedSahamChangeValue();
        StatusImageChange();
        ChooseSaham(selectedSahamIndex);
        BuySellAvailability();
        InvokeRepeating("ChangeValue", 0, 30); //waktu berubah sahamnya 
    }

    private void ScrollSaham(bool right)
    {
        //if bool value true, scroll right
        //if false, scroll left
        if (right)
        {
            scrollSaham.horizontalNormalizedPosition = 1;
        }
        else
        {
            scrollSaham.horizontalNormalizedPosition = 0;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //Saham1();
        if (PlayerPrefs.HasKey("TimerOn"))
        {
            CountTime();
            Debug.Log(timeValue);
        }
        showRealTime();
    }

    public void showRealTime()
    {
        System.DateTime dateTime = System.DateTime.UtcNow;
        string hari_date = dateTime.ToLongDateString();
        string waktu_now = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm tt");
        hari.text = hari_date;
        waktu.text = waktu_now;
    }    

    public void CountTime()
    {
        if (timeValue > 0)
        {
            timeValue -= Time.deltaTime;
        }
        else
        {
            timeValue = 0;
            
        }

        if (timeValue < 180)
        {
            timerText.color = Color.red;
        }
        else
        {
            timerText.color = Color.white;
        }
        DisplayTime(timeValue);
        //PlayerPrefs.SetFloat("timeValue", timeValue);
        if (timeValue < 1) // waktu kurang dari 1 detik
        {
            if(profit < tenPercentModal)
            {
                finishGame.SetActive(true);
                berhasil.SetActive(false);
                gagal.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetInt("Uang", (PlayerPrefs.GetInt("Uang") + profit));

                if (PlayerPrefs.HasKey("modalAwal"))
                {
                    PlayerPrefs.SetInt("modalAwal", (PlayerPrefs.GetInt("modalAwal") + profit));
                }

                finishGame.SetActive(true);
                gagal.SetActive(false);
                berhasil.SetActive(true);
                indexStage2Berhasil = 1;
                PlayerPrefs.SetInt("indexStage2Berhasil", indexStage2Berhasil);
                enabled = false;
            } 
            
        }
    }

    public void DisplayTime(float timeToDisplay)
    {
        if (timeToDisplay < 0)
        {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
    }

    private void BuySellAvailability()
    {
        //buy
        //if value > money ? buy button disabled
        if(valueTemp > uang || valueTemp == 0)
        {
            buyBtn.interactable = false;
        }
        else
        {
            buyBtn.interactable = true;
        }

        //sel
        //if volume > owned ? sell button disabled
        if (volTemp > DaftarSahamS2[selectedSahamIndex].GetOwnedSaham() || volTemp == 0)
        {
            sellBtn.interactable = false;
        }
        else
        {
            sellBtn.interactable = true;
        }
    }

    private void MoneyChangeValue()
    {
        //uangText.text = uang.ToString();
        uangText.text = string.Format("{0:n0}", uang);
    }

    public void OwnedSahamChangeValue()
    {   
        ownedSaham = DaftarSahamS2[selectedSahamIndex].GetOwnedSaham();
        ownedSahamText.text = ownedSaham.ToString();
        
    }

    public void saveOwnedSaham()
    {
        for (int i = 0; i < DaftarSahamS2.Count; i++)
        {
          PlayerPrefs.SetInt("ownedSaham" + i, DaftarSahamS2[i].GetOwnedSaham());
        }
       
        //PlayerPrefs.SetInt("count", DaftarSahamS2.Count);
    }
    public void loadOwnedSaham()
    {
        //savedListCount =  PlayerPrefs.GetInt("count");
        for (int i = 0; i < DaftarSahamS2.Count ; i++) 
        {
            ownedSaham =  PlayerPrefs.GetInt("ownedSaham" + i, DaftarSahamS2[i].GetOwnedSaham());
        }
        ownedSahamText.text = ownedSaham.ToString();
    }

    private void AddButtonListenerSaham()
    {
        for (int i = 0; i < 15; i++)
        {
            int id = i;
            saham[i].onClick.AddListener(delegate { ChooseSaham(id); });
        }

        for (int i = 0; i < 15; i++)
        {
            sahamNameText[i].text = DaftarSahamS2[i].GetKodeSaham().ToString();
        }
    }

    private void SeedSaham()
    {
        DaftarSahamS2 = new List<SahamS2>();
        DaftarSahamS2.Add(new SahamS2("PT Elang Transportasi Utama", "Transportasi & Logistik", "ELTU", 5200));
        DaftarSahamS2.Add(new SahamS2("PT Nusantara Mediacom Indonesia", "Industri", "NUMI", 7800));
        DaftarSahamS2.Add(new SahamS2("PT Aneka Mineral Tambang", "Bahan Baku", "AMTB", 12325));
        DaftarSahamS2.Add(new SahamS2("PT Banten Farma Indonesia", "Kesehatan", "BAFI", 4875));
        DaftarSahamS2.Add(new SahamS2("PT Multimedia Resources Indonesia", "Energi", "MURI", 3850));
        DaftarSahamS2.Add(new SahamS2("PT Multimedia City", "Properti", "MNCT", 2975));
        DaftarSahamS2.Add(new SahamS2("PT Indonesia Konstruksi Jaya", "Infrastruktur", "INKJ", 1975));
        DaftarSahamS2.Add(new SahamS2("PT Bank Dana Asia", "Keuangan", "BBDA", 8575));
        DaftarSahamS2.Add(new SahamS2("PT Multimedia Elektronik Indonesia", "Teknologi", "MMEI", 3650));
        DaftarSahamS2.Add(new SahamS2("PT Asia Lestari Multi Finance", "Produk Investasi Tercatat", "ALMF", 2275));
        DaftarSahamS2.Add(new SahamS2("PT Sepatu Nusantara Indah", "Barang Konsumer Primer", "SENI", 3675));
        DaftarSahamS2.Add(new SahamS2("PT Nusantara Food Indonesia", "Barang Konsumer Non-Primer", "NUFI", 4350));
        DaftarSahamS2.Add(new SahamS2("PT Bank Multimedia Nusantara", "Keuangan", "BBMN", 14750));
        DaftarSahamS2.Add(new SahamS2("PT Tangerang Realty", "Properti", "TGRE", 9650));
        DaftarSahamS2.Add(new SahamS2("PT Sehat Maju Terus", "Kesehatan", "SHMT", 7975));
        
    }

    public void PanelNotesBtn()
    {
        Time.timeScale = 0;
        panelNotes.SetActive(true);
        notesBtn.interactable = false;
        sellBtn.interactable = false;
        buyBtn.interactable = false;
    }

    public void ChangeValue()
    {
        for(int i=0;i<DaftarSahamS2.Count;i++)
        {
            randomNumberExample = Random.Range(-1, 4);
            if (randomNumberExample == -1)
            {
                //ubah nilai saham
                if(DaftarSahamS2[i].hargaSaham < 5000)
                {
                    DaftarSahamS2[i].hargaSaham -= 100;
                }
                else if(DaftarSahamS2[i].hargaSaham >= 5000 && DaftarSahamS2[i].hargaSaham < 10000)
                {
                    DaftarSahamS2[i].hargaSaham -= 150;
                }
                else if(DaftarSahamS2[i].hargaSaham >= 10000)
                {
                    DaftarSahamS2[i].hargaSaham -= 200;
                }
                
                DaftarSahamS2[i].SetStatusSaham(-1);

                saham[i].image.sprite = sahamColour[0];
            }

            else if (randomNumberExample == 0)
            {
                //ubah nilai saham
                saham[i].image.sprite = sahamColour[1];
                DaftarSahamS2[i].SetStatusSaham(0);
            }

            else
            {
                //ubah nilai saham
                if (DaftarSahamS2[i].hargaSaham < 5000)
                {
                    DaftarSahamS2[i].hargaSaham += 100;
                }
                else if (DaftarSahamS2[i].hargaSaham >= 5000 && DaftarSahamS2[i].hargaSaham < 10000)
                {
                    DaftarSahamS2[i].hargaSaham += 150;
                }
                else if (DaftarSahamS2[i].hargaSaham >= 10000)
                {
                    DaftarSahamS2[i].hargaSaham += 200;
                }
                saham[i].image.sprite = sahamColour[2];
                DaftarSahamS2[i].SetStatusSaham(1);
            }
            DaftarSahamS2[i].SetSumSaham(DaftarSahamS2[i].hargaSaham);
            DaftarSahamS2[i].IncrementTickSaham();
            
        }
        ChooseSaham(selectedSahamIndex);
        ValueChanged();
        StatusImageChange();
        BuySellAvailability();
        ProfitCount();
    }

    private void StatusImageChange()
    {
        int currStatus = DaftarSahamS2[selectedSahamIndex].GetStatusSaham();
        if (currStatus == -1)
        {
            warnaStatus.sprite = spriteWarnaStatus[0];
            //warna merah
        }
        else if(currStatus == 0)
        {
            warnaStatus.sprite = spriteWarnaStatus[1];
            //warna kuning
        }
        else
        {
            warnaStatus.sprite = spriteWarnaStatus[2];
            //warna ijo
        }
    }


    public void CloseBtn()
    {
        notesBtn.interactable = true;
        // sellBtn.interactable = true;
        // buyBtn.interactable = true;
        panelNotes.SetActive(false);
        panelbackStage1.SetActive(false);
        Time.timeScale = 1;
    }

    public void panelbackstg1()
    {
        panelbackStage1.SetActive(true);
    }

    public void ChooseSaham(int j)
    {
        //reset volume setiap pemilihan saham baru
        ResetVolume();

        priceText.text = string.Format("{0:n0}", DaftarSahamS2[j].GetHargaSaham());
        avgPriceText.text = string.Format("{0:n0}", DaftarSahamS2[j].AverageSaham());
        namaPerusahaanText.text = DaftarSahamS2[j].GetNamaPerusahaan().ToString();
        selectedSahamIndex = j;

        BuySellAvailability();
        OwnedSahamChangeValue();
        StatusImageChange();
    }

    public void ValueChanged()
    {
        int.TryParse(volumeText.text.ToString(), out volTemp);
        valueTemp = (volTemp * 100 * DaftarSahamS2[selectedSahamIndex].GetHargaSaham());
        if(volTemp != 0)
        {
            volumeText.text = volTemp.ToString();
        }
        else
        {
            volumeText.text = "";
        }
        
        valueText.text = string.Format("{0:n0}", valueTemp);
        BuySellAvailability();
    }

    public void BuySellSaham(bool buy)
    {
        if (buy)
        {
            //owned + volume
            //uang - value
            DaftarSahamS2[selectedSahamIndex].GetOwnedSaham();
            DaftarSahamS2[selectedSahamIndex].BuySellSaham(volTemp);
            uang -= (DaftarSahamS2[selectedSahamIndex].GetHargaSaham() * volTemp * 100);
            //PlayerPrefs.SetInt("ModalAwal", uang);
            //carabaru //SahamS2.SetInts("ownedSaham", ownedSaham);
            
        }
        else
        {
            //owned + (-volume)
            //uang + value
            DaftarSahamS2[selectedSahamIndex].GetOwnedSaham();
            DaftarSahamS2[selectedSahamIndex].BuySellSaham(-volTemp);
            uang += (DaftarSahamS2[selectedSahamIndex].GetHargaSaham() * volTemp * 100);

            
        }
        ResetVolume();
        MoneyChangeValue();
        OwnedSahamChangeValue();
        BuySellAvailability();
    }

    public void goTOStage3()
    {
        SceneManager.LoadScene("Stage3Scene");
        
        //buat cek sementara aja, jadi ga nunggu profit
        // indexStage2Berhasil = 1;
        // PlayerPrefs.SetInt("indexStage2Berhasil", indexStage2Berhasil);
    }

    private void ProfitCount()
    {
        //(money + (owned saham * value * 100)) - (modal awal)
        //saham value = (owned saham * value * 100)

        int totalSahamValue = 0; //menampung value saham keseluruhan
        //int profit = 0; 
        
        for(int i =0; i < DaftarSahamS2.Count; i++)
        {
            totalSahamValue += (DaftarSahamS2[i].GetHargaSaham() * DaftarSahamS2[i].GetOwnedSaham() * 100);
        }

        profit = (uang + totalSahamValue) - modalAwal; 
        profitText.text = string.Format("{0:n0}", profit);
    
        // setting menang atau kalah
        tenPercentModal = (int)((double)modalAwal * 0.05); // 5% dari modal ,unity g bsa nentuin tipe :)

    }

    private void ResetVolume()
    {
        volumeText.text = "";
    }

    public void restartScene()
    {
        SceneManager.LoadScene("Stage2Scene");
    }

    public void gotoStage1()
    {
        SceneManager.LoadScene("MainScene");
    }
}
