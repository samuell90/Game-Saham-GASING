using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoNotDestroy : MonoBehaviour
{
    public AudioClip popSound;
    public AudioSource AudioSource;

    void Start() 
    {
        AudioSource.clip = popSound;
    }
    private void Awake()
    {
        GameObject[] musicObj = GameObject.FindGameObjectsWithTag("GameMusic");
        

        if (musicObj.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    public void playPop()
    {
        AudioSource.Play();
    }
}
