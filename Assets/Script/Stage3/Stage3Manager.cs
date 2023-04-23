using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.SceneManagement;
using TMPro;
using System.Text.RegularExpressions;
using System.Threading;

public class Stage3Manager : MonoBehaviour
{
    //Variabel UI
    public Image karakter;
    public Image warnaStatus;
    public TextMeshProUGUI namaKarakter;
    public TextMeshProUGUI avgPriceText;
    public TextMeshProUGUI priceText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI namaPerusahaanText;
    public TextMeshProUGUI ownedSahamText;
    public TMP_InputField volumeText;

    //variabel text untuk menampilkan fun facts
    public TextMeshProUGUI funFactsText;

    //variabel list untuk menampung fun facts
    List<string> funFactsString;

    //button left and right
    public Button leftBtn, rightBtn;
    public ScrollRect scrollSaham;

    //variabel tampilan
    [SerializeField] TextMeshProUGUI uangText;
    public TextMeshProUGUI profitText;
    public GameObject panelNotes;
    public Button[] saham = new Button[15];
    public TextMeshProUGUI[] sahamNameText = new TextMeshProUGUI[15];
    public Sprite[] spriteWarnaStatus = new Sprite[3];
    public Button sellBtn, buyBtn, notesBtn, close;
    public Sprite[] sahamColour = new Sprite[3];
    public Sprite[] statusColour = new Sprite[3];

    //variabel nilai yang digunakan
    public int randomNumberExample;
    private int selectedSahamIndex = 0;
    private int volTemp = 0;
    private int valueTemp = 0;
    private int uang = 0, modalAwal = 0;
    //continuescene
    private int currentSceneIndex;
    //tanggalrealtime
    public TextMeshProUGUI waktu;
    public TextMeshProUGUI hari;
    public GameObject loadingScreen;
    public Slider loadingSlider;
    private float loadingTime;
    private bool marketOpen=true;

    //arraylist berisi saham
    List<SahamS3> DaftarSahamS3;
    Thread _tHargaCurr;
    private IEnumerator coroutineFetchPrice;

    //cek koneksi internet
    public GameObject connectionPanel, notesPasarSaham;

    //private IEnumerator loading;
    public Text textPasarsaham;
    public GameObject panelbackStage1;


    void Start()
    {
        SeedFunFacts();
        SeedSaham();
        AddButtonListenerSaham();
        volumeText.onValueChanged.AddListener(delegate { ValueChanged(); });

        buyBtn.onClick.AddListener(delegate { BuySellSaham(true); });
        sellBtn.onClick.AddListener(delegate { BuySellSaham(false); });
        rightBtn.onClick.AddListener(delegate { ScrollSaham(true); });
        leftBtn.onClick.AddListener(delegate { ScrollSaham(false); });

        panelNotes.SetActive(false);
        panelbackStage1.SetActive(false);
        connectionPanel.SetActive(false);
        notesPasarSaham.SetActive(false);

        coroutineFetchPrice = ChangePrice(300.0f);
        StartCoroutine(coroutineFetchPrice);

        karakter.sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("characterSprite") + "-head");
        namaKarakter.text = PlayerPrefs.GetString("playerName");
        volumeText.characterValidation = TMP_InputField.CharacterValidation.Integer;
        volumeText.characterLimit = 3;

        uang = PlayerPrefs.GetInt("Uang");

        //mengecek apabila player memiliki modalawal atau belum
        if (PlayerPrefs.HasKey("modalAwal"))
        {
            modalAwal = PlayerPrefs.GetInt("modalAwal");
        }
        else
        {
            modalAwal = uang;
            PlayerPrefs.SetInt("modalAwal", modalAwal);
        }

        MoneyChangeValue();
        OwnedSahamChangeValue();
        StatusImageChange();
        ChooseSaham(selectedSahamIndex);
        BuySellAvailability();
        //pasarWaktuSaham();
        CheckInternetConnection();
        InvokeRepeating("RandomizeFact",0 , 10); // funfact random tiap .. detik
    }

    private void CheckInternetConnection()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            connectionPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void notesPasarsahambtn()
    {
        notesPasarSaham.SetActive(true);
    }
    public void panelbackstg1()
    {
        panelbackStage1.SetActive(true);
    }

    public void ConfirmConnection()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
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
    private void SeedFunFacts()
    {
        funFactsString = new List<string>();
        funFactsString.Add("<b>JANUARY EFFECT</b> adalah sebuah teori tentang anomali musiman di mana harga saham cenderung meningkat lebih pesat di bulan Januari dibandingkan bulan-bulan lain");
        funFactsString.Add("<b>REBOUND</b> adalah kenaikan harga saham setelah harga saham mengalami penurunan harga secara drastis");
        funFactsString.Add("<b>BULLISH</b> adalah keadaan dimana harga saham cenderung mengalami penguatan secara berkelanjutan pada satu periode tertentu");
        funFactsString.Add("<b>BEARISH</b> adalah keadaan dimana harga saham <b>mengalami tren turun atau melemah</b> pada satu periode tertentu");
        funFactsString.Add("<b>WINDOW DRESSING</b> adalah strategi perusahaan untuk menarik hati investor dengan cara mempercantik kinerja keuangan sehingga harga saham naik di akhir tahun");
        funFactsString.Add("<b>IPO (INITIAL PUBLIC OFFERING)</b> adalah tahap ketika sebuah perusahaan menawarkan sahamnya kepada publik untuk pertama kalinya");
        funFactsString.Add("<b>UMA (Unusual Market Activity)</b> adalah perdagangan dan/atau pergerakan harga suatu saham dalam periode tertentu yang menurut BEI tidak wajar");
        funFactsString.Add("<b>AUTO REJECTION</b> adalah penolakan secara otomatis terhadap laju pergerakan harga suatu saham (naik atau turun) yang telah menembus batas harga maksimum dan minimum");
        funFactsString.Add("<b>CUT LOSS</b> adalah penjualan saham dengan harga lebih rendah disbanding harga beli untuk menghindari kerugian yang lebih besar");
        funFactsString.Add("<b>CORPORATE ACTION</b> adalah tindakan yang diambil oleh perusahaan yang berdampak langsung terhadap kepemilikan saham para investor");
    }

    public void RandomizeFact()
    {
        int arrLen = funFactsString.Count;
        int arrIdx = Random.Range(0, arrLen);

        funFactsText.text = funFactsString[arrIdx];
    }

    // Update is called once per frame
    void Update()
    {
        //CountTime();
        ShowRealTime();
        LoadingTimer();
        if(PlayerPrefs.HasKey("TutorialS3"))
        {
            pasarWaktuSaham();
        }
    }

    public void LoadingTimer()
    {
        loadingTime += Time.deltaTime;
        loadingSlider.value = loadingTime;

        if (loadingTime >= 60 && loadingTime <= 62)
        {
            loadingScreen.SetActive(false);
            //cek warna saham
            for (int i = 0; i < DaftarSahamS3.Count; i++)
            {
                ColorSaham(i);
            }
            ChooseSaham(selectedSahamIndex);
            ProfitCount();
        }
    }

    private void ColorSaham(int i)
    {
        if (DaftarSahamS3[i].GetStatusSaham() == 1)
        {
            saham[i].image.sprite = sahamColour[2];
        }
        else if (DaftarSahamS3[i].GetStatusSaham() == 0)
        {
            saham[i].image.sprite = sahamColour[1];
        }
        else
        {
            saham[i].image.sprite = sahamColour[0];
        }
    }

    public void ShowRealTime()
    {
        System.DateTime dateTime = System.DateTime.UtcNow;
        string hari_date = dateTime.ToLongDateString();
        string waktu_now = System.DateTime.UtcNow.ToLocalTime().ToString("HH:mm tt");
        hari.text = hari_date;
        waktu.text = waktu_now;
    }

    public void pasarWaktuSaham()
    {
        System.DateTime dateNow = System.DateTime.UtcNow.ToLocalTime();
        string dayNow = dateNow.ToString("dddd");
        string timeSesi1_start = "09:00:00";
        string timeSesi1_end = "15:00:00";
        string close_day1 = "Saturday", close_day2 = "Sunday";
        System.DateTime sesi1_start = System.DateTime.Parse(timeSesi1_start);
        System.DateTime sesi1_end = System.DateTime.Parse(timeSesi1_end);

        // menentukan waktu buka pasar saham
        if (dayNow != close_day1 && dayNow != close_day2 && dateNow > sesi1_start && dateNow < sesi1_end)
        {
            //pasar saham sedang buka
            textPasarsaham.text = "OPEN";
            textPasarsaham.color = Color.green;
            marketOpen = true;
        }
        else
        {
            //pasar saham sedang tutup
            textPasarsaham.text = "CLOSE";
            textPasarsaham.color = Color.red;
            marketOpen = false;
        }
    }

    private void BuySellAvailability()
    {
        //buy
        //if value > money ? buy button disabled
        if (valueTemp > uang || valueTemp == 0)
        {
            buyBtn.interactable = false;
        }
        else
        {
            buyBtn.interactable = true;
        }

        //sell
        //if volume > owned ? sell button disabled
        if (volTemp > DaftarSahamS3[selectedSahamIndex].GetOwnedSaham() || volTemp == 0)
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
        uangText.text = string.Format("{0:n0}", uang);
    }

    private void OwnedSahamChangeValue()
    {
        ownedSahamText.text = DaftarSahamS3[selectedSahamIndex].GetOwnedSaham().ToString();
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
            sahamNameText[i].text = DaftarSahamS3[i].GetKodeSaham().ToString();
        }
    }

    private void SeedSaham()
    {
        DaftarSahamS3 = new List<SahamS3>();

        DaftarSahamS3.Add(new SahamS3("PT Blue Bird, Tbk", "Transportasi & Logistik", "BIRD", 1226.25f));
        DaftarSahamS3.Add(new SahamS3("PT Astra International, Tbk", "Industri", "ASII", 5218.60f));
        DaftarSahamS3.Add(new SahamS3("PT Aneka Tambang, Tbk", "Bahan Baku", "ANTM", 2409.78f));
        DaftarSahamS3.Add(new SahamS3("PT Kalbe Farma, Tbk", "Kesehatan", "KLBF", 1410.72f));
        DaftarSahamS3.Add(new SahamS3("PT Adaro Energy, Tbk", "Energi", "ADRO", 1330.62f));
        DaftarSahamS3.Add(new SahamS3("PT Bumi Serpong Damai, Tbk", "Properti", "BSDE", 1040.04f));
        DaftarSahamS3.Add(new SahamS3("PT Adhi Karya, Tbk", "Infrastruktur", "ADHI", 913.49f));
        DaftarSahamS3.Add(new SahamS3("PT Bank Rakyat Indonesia, Tbk", "Keuangan", "BBRI", 3960.08f));
        DaftarSahamS3.Add(new SahamS3("PT Metrodata Electronics Tbk", "Teknologi", "MTDL", 2258.67f));
        DaftarSahamS3.Add(new SahamS3("PT Saratoga Investama Sedaya Tbk", "Produk Investasi Tercatat", "SRTG", 1581.52f));
        DaftarSahamS3.Add(new SahamS3("PT Ace Hardware Indonesia, Tbk", "Barang Konsumer Primer", "ACES", 1415.72f));
        DaftarSahamS3.Add(new SahamS3("PT Astra Agro Lestari, Tbk", "Barang Konsumer Non-Primer", "AALI", 8861.51f));
        DaftarSahamS3.Add(new SahamS3("PT Bank Central Asia, Tbk", "Keuangan", "BBCA", 6424.65f));
        DaftarSahamS3.Add(new SahamS3("PT Alam Sutera Realty", "Properti", "ASRI", 177.64f));
        DaftarSahamS3.Add(new SahamS3("PT Siloam International Hospitals", "Kesehatan", "SILO", 8364.67f));

        //check if previously already owned saham
        for(int i = 0; i < DaftarSahamS3.Count; i++)
        {
            //check owned saham
            if (!PlayerPrefs.HasKey(DaftarSahamS3[i].GetNamaPerusahaan()))
            {
                PlayerPrefs.SetInt(DaftarSahamS3[i].GetNamaPerusahaan(), 0);
            }
            else
            {
                DaftarSahamS3[i].ownedSaham = PlayerPrefs.GetInt(DaftarSahamS3[i].GetNamaPerusahaan());
            }
        }

    }
    public void PanelNotesBtn()
    {
        Time.timeScale = 0;
        panelNotes.SetActive(true);
        notesBtn.interactable = false;
        sellBtn.interactable = false;
        buyBtn.interactable = false;
    }
    private IEnumerator ChangePrice(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        while (true)
        {
            CheckInternetConnection();
            if (marketOpen)
            {
                for (int i = 0; i < DaftarSahamS3.Count; i++)
                {
                    //DaftarSahamS3[i].FetchHargaSaham();
                    _tHargaCurr = new Thread(DaftarSahamS3[i].FetchHargaSaham);
                    _tHargaCurr.Start();

                    ColorSaham(i);
                    ChooseSaham(selectedSahamIndex);
                    ValueChanged();
                    StatusImageChange();
                    BuySellAvailability();
                    ProfitCount();
                    yield return new WaitForSeconds(5f);
                }
            }
        }
    }
    private void StatusImageChange()
    {
        int currStatus = DaftarSahamS3[selectedSahamIndex].GetStatusSaham();
        if (currStatus == -1)
        {
            warnaStatus.sprite = spriteWarnaStatus[0];
            //warna merah
        }
        else if (currStatus == 0)
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
        notesPasarSaham.SetActive(false);
        Time.timeScale = 1;
    }
    public void ChooseSaham(int j)
    {
        //reset volume
        ResetVolume();
        //print harga saham dan average dengan format
        priceText.text = string.Format("{0:n0}", DaftarSahamS3[j].GetHargaSaham());
        avgPriceText.text = string.Format("{0:n}", DaftarSahamS3[j].GetAvgSaham());

        namaPerusahaanText.text = DaftarSahamS3[j].GetNamaPerusahaan().ToString();

        //mengupdate indeks saham yang sedang dipilih user
        selectedSahamIndex = j;

        ColorSaham(selectedSahamIndex);
        BuySellAvailability();
        OwnedSahamChangeValue();
        StatusImageChange();
    }
    public void ValueChanged()
    {
        int.TryParse(volumeText.text.ToString(), out volTemp);
        valueTemp = (volTemp * 100 * DaftarSahamS3[selectedSahamIndex].GetHargaSaham());
        if (volTemp != 0)
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
            DaftarSahamS3[selectedSahamIndex].BuySellSaham(volTemp);
            uang -= (DaftarSahamS3[selectedSahamIndex].GetHargaSaham() * volTemp * 100);        
        }
        else
        {
            //owned + (-volume)
            //uang + value
            DaftarSahamS3[selectedSahamIndex].BuySellSaham(-volTemp);
            uang += (DaftarSahamS3[selectedSahamIndex].GetHargaSaham() * volTemp * 100);
        }

        //saving owned saham to PlayerPref
        PlayerPrefs.SetInt(DaftarSahamS3[selectedSahamIndex].GetNamaPerusahaan(), DaftarSahamS3[selectedSahamIndex].GetOwnedSaham());

        //save jumlah uang milik player skrg
        PlayerPrefs.SetInt("Uang", uang);

        ResetVolume();
        MoneyChangeValue();
        OwnedSahamChangeValue();
        BuySellAvailability();
    }
    private void ProfitCount()
    {
        //(money + (owned saham * value * 100)) - (modal awal)
        //saham value = (owned saham * value * 100)
        int totalSahamValue = 0; //menampung value saham keseluruhan
        int profit = 0;

        for (int i = 0; i < DaftarSahamS3.Count; i++)
        {
            totalSahamValue += (DaftarSahamS3[i].GetHargaSaham() * DaftarSahamS3[i].GetOwnedSaham() * 100);
        }

        profit = (uang + totalSahamValue) - modalAwal;
        profitText.text = string.Format("{0:n0}", profit);
    }

    private void ResetVolume()
    {
        volumeText.text = "";
    }
    public void goStage1()
    {
        SceneManager.LoadScene("MainScene");
    }
}
