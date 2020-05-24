using UnityEngine;

public class NPC1 : MonoBehaviour
{
    public int whichDialogue = 0;

    private DialogueTrigger _myDialogueTrigger;
    private Dialogue[] _myDialogue;
    private GameObject _player;
    
    // Start is called before the first frame update
    void Start()
    {
        _myDialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        _myDialogue = _myDialogueTrigger.dialogue;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (_player == null)
                _player = other.gameObject;
            if (whichDialogue < _myDialogue.Length)
            {
                _myDialogueTrigger.TriggerDialogue(whichDialogue);
                whichDialogue++;
            }
        }
    }
}
