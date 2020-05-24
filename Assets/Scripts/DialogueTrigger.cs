using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue[] dialogue;

    public void TriggerDialogue(int id)
    {
        FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue[id]);
    }

}
