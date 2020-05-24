using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateB : MonoBehaviour
{
	public int whichDialogue = 0;

	private DialogueTrigger _myDialogueTrigger;
	private Dialogue[] _myDialogue;
	private GameObject _player;
	public int trigCount = 0;

	public Animator gateBL, gateBR;

	// Start is called before the first frame update
	void Start()
    {
		_myDialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
		_myDialogue = _myDialogueTrigger.dialogue;
		gateBL.enabled = false;
		gateBR.enabled = false;
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
			if (!other.GetComponent<CharacterControl>().haveKey)
			{
				_myDialogueTrigger.TriggerDialogue(1);
			}
			else
			{
				if (trigCount == 0)
				{
					_myDialogueTrigger.TriggerDialogue(0);
					gateBL.enabled = true;
					gateBR.enabled = true;
					trigCount++;
				}
			}
		}
	}
}
