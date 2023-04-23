using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class SahamS2
{
    public static void SetInts(string key, ICollection<int> collection)
    {
       PlayerPrefs.SetInt(key + ".Count", collection.Count);
       for (int i = 0; i < collection.Count; i++)
       {
           PlayerPrefs.SetInt(key + "[" + i + "]" , collection.ElementAt(i));
       }
    }

    public static ICollection<int> GetInts (string  key)
    {
        int count = PlayerPrefs.GetInt(key + ".Count", 0);
        int[] array = new int[count];

        for (int i=0; i < count; i++)
        {
            array[i] = PlayerPrefs.GetInt(key + "[" + i + "]");
        }
        return array;
    }

    public string namaPerusahaaan;
    public string industri;
    public string kodeSaham;
    public int hargaSaham;
    public int sumSaham;
    public int tickSaham; //tickSaham mengindikasikan berapa kali saham mengalami perubahan harga, untuk menghitung average saham

    private int statusSaham;
    //-1 berarti saham mengalami penurunan
    //0 berarti tidak ada perubahan
    //1 berarti saham mengalami kenaikan

    public int ownedSaham;

    public SahamS2() { }
    public SahamS2(string namaPerusahaanString, string industriString, string kodeSahamString, int hargaSahamInt)
    {
        namaPerusahaaan = namaPerusahaanString;
        industri = industriString;
        kodeSaham = kodeSahamString;
        hargaSaham = hargaSahamInt;
        sumSaham = 0;
        tickSaham = 0;
        ownedSaham = 0;
        statusSaham = 0;
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

    public int GetSumSaham()
    {
        return sumSaham;
    }

    public void IncrementTickSaham()
    {
        tickSaham++;
    }

    public void SetSumSaham(int sum)
    {
        sumSaham += sum;
    }

    public float AverageSaham()
    {
        if(tickSaham != 0)
        {
            return sumSaham / tickSaham;
        }
        else
        {
            return hargaSaham;
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

}
