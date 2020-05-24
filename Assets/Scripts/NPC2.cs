using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC2 : MonoBehaviour
{
    public int whichDialogue = 0;

    private DialogueTrigger _myDialogueTrigger;
    private Dialogue[] _myDialogue;
    private GameObject _player;
	public int trigCount = 0;

	public Animator gateAL, gateAR;
	public GameObject AreaB2;
    
    // Start is called before the first frame update
    void Start()
    {
        _myDialogueTrigger = gameObject.GetComponent<DialogueTrigger>();
        _myDialogue = _myDialogueTrigger.dialogue;
		gateAL.enabled = false;
		gateAR.enabled = false;
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
			if(!other.GetComponent<CharacterControl>().haveLetter)
			{
				_myDialogueTrigger.TriggerDialogue(1);
			}
			else
			{
				if (trigCount == 0)
				{
					_myDialogueTrigger.TriggerDialogue(0);
					gateAL.enabled = true;
					gateAR.enabled = true;
					trigCount++;
				}
				else if (trigCount == 1 && AreaB2.GetComponent<AreaB2>().trigCount == 2) 
				{
					_myDialogueTrigger.TriggerDialogue(2);
					trigCount++;
					other.GetComponent<CharacterControl>().suggest = true;
				}
			}
        }
    }
}
