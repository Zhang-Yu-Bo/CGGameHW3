using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaB2 : MonoBehaviour
{
	public int whichDialogue = 0;

	private DialogueTrigger _myDialogueTrigger;
	private Dialogue[] _myDialogue;
	private GameObject _player;
	public int trigCount = 0;

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

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			if (_player == null)
				_player = other.gameObject;
			if (trigCount == 0) 
			{
				trigCount++;
			}
			else if(trigCount==1)
			{
				_myDialogueTrigger.TriggerDialogue(0);
				trigCount++;
			}

		}
	}
}
