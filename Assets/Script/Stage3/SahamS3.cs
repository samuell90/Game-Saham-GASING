using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HtmlAgilityPack;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net;
using System.Text;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Threading;

public class SahamS3
{
    public string namaPerusahaaan;
    public string industri;
    public string kodeSaham;
    public int hargaSaham;
    public float avgSaham;
    private int statusSaham;
    //-1 berarti saham mengalami penurunan
    //0 berarti tidak ada perubahan
    //1 berarti saham mengalami kenaikan

    public int ownedSaham;

    public Thread _tHarga;
    public Thread _tAverage;

    public SahamS3() { }
    public SahamS3(string namaPerusahaanString, string industriString, string kodeSahamString, float avg)
    {
        namaPerusahaaan = namaPerusahaanString;
        industri = industriString;
        kodeSaham = kodeSahamString;
        
        ownedSaham = 0;
        statusSaham = 0;

        avgSaham = avg;
        hargaSaham = 0;

        //avgSaham = AverageSaham();
        //avgSaham = 0;
        _tHarga = new Thread(FetchHargaSaham);
       
        _tAverage = new Thread(AverageSaham);

        _tHarga.Start();
        _tAverage.Start();

    }

    public string GetNamaPerusahaan()
    {
        return namaPerusahaaan;
    }

    public string GetIndustri()
    {
        return industri;
    }

    public string GetKodeSaham()
    {
        return kodeSaham;
    }

    public int GetHargaSaham()
    {
        return hargaSaham;
    }

    public float GetAvgSaham()
    {
        return avgSaham;
    }

    public void AverageSaham()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load("https://finance.yahoo.com/quote/" + kodeSaham + ".JK/key-statistics");
        HtmlNode[] node = doc.DocumentNode.SelectNodes("//td[contains(@class, 'Fw(500)') and contains(@class, 'Ta(end)') and contains(@class, 'Pstart(10px)') and contains(@class, 'Miw(60px)')]").ToArray();
        
        if(!node[0].InnerText.Equals("N/A"))
        {
            string avgStr = node[14].InnerText.ToString();
            string avgClean = Regex.Replace(avgStr, "[^.0-9]", "");
            CultureInfo ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";
            avgSaham = float.Parse(avgClean, CultureInfo.InvariantCulture.NumberFormat);
        }
    }

    public int GetOwnedSaham()
    {
        return ownedSaham;
    }

    public void BuySellSaham(int vol)
    {
        //value vol plus -> beli saham
        //value vol minus -> jual saham
        ownedSaham += vol;
    }

    public int GetStatusSaham()
    {
        return statusSaham;
    }

    public void SetStatusSaham(int status)
    {
        statusSaham = status;
    }

    public void FetchHargaSaham()
    {
        HtmlWeb web = new HtmlWeb();
        HtmlDocument doc = web.Load("https://finance.yahoo.com/quote/" + kodeSaham + ".JK");
        HtmlNode[] node = doc.DocumentNode.SelectNodes("//fin-streamer[contains(@class, 'Fw(b)') and contains(@class, 'Fz(36px)') and contains(@class, 'Mb(-4px)') and contains(@class, 'D(ib)')]").ToArray();
        //HtmlNode[] node = doc.DocumentNode.SelectNodes("//span[contains(@data-reactid, '31')]").ToArray();

        if (!node[0].InnerText.Equals("N/A"))
        {
            string currPriceStr = node[0].InnerText.ToString();
            int trimPoint = currPriceStr.Length - 3;
            string PriceClean = System.Text.RegularExpressions.Regex.Replace(currPriceStr.Substring(0, trimPoint), "[^0-9]", "");
            //Debug.Log(PriceClean);
            int currPrice = int.Parse(PriceClean);
            hargaSaham = currPrice;
        }
        HtmlNode[] nodeChangeParent = doc.DocumentNode.SelectNodes("//fin-streamer[contains(@class, 'Fw(500)') and contains(@class, 'Pstart(8px)') and contains(@class, 'Fz(24px)')]").ToArray();
        //Debug.Log(nodeChangeParent);

        HtmlNode nodeChange = nodeChangeParent[0].SelectSingleNode("./span");

        string changeStr = nodeChange.InnerText.ToString();
        //Debug.Log(changeStr);

        //int trimpoint;
        //if (changeStr.Length > 12)
        //{
        //    trimpoint = changeStr.Length - 12;
        //}
        //else
        //{
        //    trimpoint = changeStr.Length - 10;
        //}

        //string changeSign = changeStr.Substring(0, trimpoint);
        //string changeClean = System.Text.RegularExpressions.Regex.Replace(changeSign, "[^0-9-+]", "");
        //Debug.Log(changeClean);

        string changeClean = changeStr.Substring(0, changeStr.Length - 3);
        Debug.Log(changeClean);

        int change = int.Parse(changeClean);

        Debug.Log(change);

        if (change > 0)
        {
            //harga naik
            statusSaham = 1;
            //Debug.Log("harga naik, status = " + statusSaham);
        }
        else if (change < 0)
        {
            //harga turun
            statusSaham = -1;
        }
        else
        {
            statusSaham = 0;
        }
    }

}
