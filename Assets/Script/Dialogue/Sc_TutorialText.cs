using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sc_TutorialText : MonoBehaviour
{
    public Dialogue dialogue;
    public DialogueManager dialogueManager;
    bool isStartingDialog = false;
    private string sceneName;

    private void Start()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
    // Update is called once per frame
    void Update()
    {
        if (dialogueManager.isFinished)
        {
            isStartingDialog = false;
        }

        //cek apakah tutorial sudah dilakukan atau belum, kalau belum jalankan
        if (sceneName.Equals("MainScene"))
        {
            if (!PlayerPrefs.HasKey("Tutorial"))
            {
                Debug.Log("Tes2");
                if (!isStartingDialog)
                {
                    dialogueManager.StartDialogue(dialogue);
                    isStartingDialog = true;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        dialogueManager.DisplayNextSentence();
                        FindObjectOfType<AudioManager>().Play("BtnQuiz");
                    }
                }
            }
            else //kalau tutorial sebelumnya sudah dilakukan
            {
                dialogueManager.dialogueCanvas.SetActive(false);
            }
        }
        else if (sceneName.Equals("Stage2Scene"))
        {
            if (!PlayerPrefs.HasKey("TutorialS2"))
            {
                if (!isStartingDialog)
                {
                    dialogueManager.StartDialogue(dialogue);
                    isStartingDialog = true;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        dialogueManager.DisplayNextSentence();
                        FindObjectOfType<AudioManager>().Play("BtnQuiz");
                    }
                }
            }
            else //kalau tutorial sebelumnya sudah dilakukan
            {
                dialogueManager.dialogueCanvas.SetActive(false);
            }
        }
        else if (sceneName.Equals("Stage3Scene"))
        {
            if (!PlayerPrefs.HasKey("TutorialS3"))
            {
                if (!isStartingDialog)
                {
                    dialogueManager.StartDialogue(dialogue);
                    isStartingDialog = true;
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        dialogueManager.DisplayNextSentence();
                        FindObjectOfType<AudioManager>().Play("BtnQuiz");
                    }
                }
            }
            else
            {
                dialogueManager.dialogueCanvas.SetActive(false);
            }
        }
    }

    
}
