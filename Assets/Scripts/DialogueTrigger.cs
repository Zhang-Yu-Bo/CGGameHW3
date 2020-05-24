using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    private bool _isTrigger = false;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue);
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (!this._isTrigger)
        {
            this.TriggerDialogue();
            this._isTrigger = true;
        }
    }
    
}
