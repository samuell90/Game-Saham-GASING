using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManagerS2 : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    protected Queue<string> sentences;
    public GameObject dialogueCanvas;
    public bool isFinished;
    // Start is called before the first frame update
    void Start()
    {
        isFinished = false;
        sentences = new Queue<string>();
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
        DisplayNextSentence();
    }


    public virtual void DisplayNextSentence()
    {
        if(sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogText.text = sentence;
    }

    public virtual void EndDialogue()
    {
        dialogueCanvas.SetActive(false);
        isFinished = true;
        PlayerPrefs.SetInt("TutorialS2", 1);
    }
}
