using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    protected Queue<string> sentences;
    public GameObject dialogueCanvas;
    public bool isFinished;
    private Animator gedungSIPF,quizButton;
    private Animator sahamBtn, buysellBtn, buysellBtn2,timerText, volumeText;
    private Animator textPasarSaham;
    private string sceneName;
    
    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
        isFinished = false;
        sentences = new Queue<string>();

        if (sceneName.Equals("MainScene"))
        {
            GameObject gedungSIPFObjek = GameObject.FindGameObjectWithTag("SIPF");
            gedungSIPF = gedungSIPFObjek.GetComponent<Animator>();

            GameObject quizButtonObjek = GameObject.FindGameObjectWithTag("QUIZ");
            quizButton = quizButtonObjek.GetComponent<Animator>(); 
        }
        else if (sceneName.Equals("Stage2Scene"))
        {
            GameObject sahamButtonObjek = GameObject.FindGameObjectWithTag("SAHAM");
            sahamBtn = sahamButtonObjek.GetComponent<Animator>();

            GameObject buysellButtonObjek = GameObject.FindGameObjectWithTag("BUYSELL");
            buysellBtn = buysellButtonObjek.GetComponent<Animator>();

            GameObject buysellButtonObjek2 = GameObject.FindGameObjectWithTag("BUYSELL2");
            buysellBtn2 = buysellButtonObjek2.GetComponent<Animator>();

            GameObject timerTextObjek = GameObject.FindGameObjectWithTag("TimerText");
            timerText = timerTextObjek.GetComponent<Animator>();

            GameObject volumeTextObjek = GameObject.FindGameObjectWithTag("VOLUME");
            volumeText = volumeTextObjek.GetComponent<Animator>();
        }
        else if (sceneName.Equals("Stage3Scene"))
        {
            GameObject textPasarSahamObjek = GameObject.FindGameObjectWithTag("PASARSAHAM");
            textPasarSaham = textPasarSahamObjek.GetComponent<Animator>();
        }
    }

    public virtual void StartDialogue(Dialogue dialogue)
    {
        dialogueCanvas.SetActive(true);
        sentences.Clear();
        isFinished = false;

        foreach (string s in dialogue.sentences)
        {
            sentences.Enqueue(s);
        }
        Debug.Log("Sentences count : " + sentences.Count);
        DisplayNextSentence();
    }


    public virtual void DisplayNextSentence()
    {
        if (sceneName.Equals("MainScene"))
        {
            if (sentences.Count == 3)
            {
                gedungSIPF.SetBool("isDepth", true);
            }

            if (sentences.Count == 2)
            {
                quizButton.SetBool("isQuiz", true);
            }
            else
            {
                quizButton.SetBool("isQuiz", false);
            }
        }
        if (sceneName.Equals("Stage2Scene"))
        {
            if (sentences.Count == 5)
            {
                sahamBtn.SetBool("isSaham", true);
                volumeText.SetBool("isVolume",true);
            }
            else if (sentences.Count == 2)
            {
                sahamBtn.SetBool("isSaham", true);
            }
            else
            {
                sahamBtn.SetBool("isSaham", false);
                volumeText.SetBool("isVolume",false);
            }

            if (sentences.Count == 4)
            {
                buysellBtn.SetBool("isBuySell",true);
                buysellBtn2.SetBool("isBuySell2",true);
            }
            else
            {
                buysellBtn.SetBool("isBuySell",false);
                buysellBtn2.SetBool("isBuySell2",false);
            }

            if (sentences.Count == 3)
            {
                timerText.SetBool("isTimer",true);
            }
            else
            { 
                timerText.SetBool("isTimer",false);
            }
        }

        if (sceneName.Equals("Stage3Scene"))
        {
            if (sentences.Count == 2)
            {
                textPasarSaham.SetBool("isPasarSaham",true);
            }
            
            else
            {
                textPasarSaham.SetBool("isPasarSaham",false);
            }
        }

        if(sentences.Count == 0)
        {
            //Debug.Log("Sentences count : 0");
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogText.text = sentence;

    }

    public virtual void EndDialogue()
    {
        //Debug.Log("End of conversation");
        dialogueCanvas.SetActive(false);
        isFinished = true;
        if (sceneName.Equals("MainScene"))
        {
            PlayerPrefs.SetInt("Tutorial", 1);
        }
        else if(sceneName.Equals("Stage2Scene"))
        {
            PlayerPrefs.SetInt("TutorialS2", 1);
            PlayerPrefs.SetInt("TimerOn",1);
        }
        else
        {
            PlayerPrefs.SetInt("TutorialS3", 1);
        }
        
    }

    public void resetDialog()
    {
        PlayerPrefs.DeleteKey("Tutorial");
        PlayerPrefs.DeleteKey("TutorialS2");
        PlayerPrefs.DeleteKey("TutorialS3");
        PlayerPrefs.DeleteKey("TimerOn");

    }
}
