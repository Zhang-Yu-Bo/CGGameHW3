using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueBox;

    private Queue<string> _name = new Queue<string>();
    private Queue<string> _sentences = new Queue<string>();

    // Start is called before the first frame update
    void Start()
    {
        if (this._sentences == null)
            this._sentences = new Queue<string>();
        if (_name == null)
            _name = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        this.dialogueBox.SetActive(true);
        _name.Clear();
        foreach (string speaker in dialogue.name)
            _name.Enqueue(speaker);
        this._sentences.Clear();
        foreach (string sentence in dialogue.sentences)
            this._sentences.Enqueue(sentence);

        this.DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (this._sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        nameText.text = _name.Dequeue();
        string sentence = this._sentences.Dequeue();

        StopAllCoroutines();
        StartCoroutine(this.TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return null;
        }
    }

    void EndDialogue()
    {
        this.dialogueBox.SetActive(false);
    }
}
