using UnityEngine;

public class DialogueTriggerB : MonoBehaviour
{
	public Dialogue dialogue;
	public bool _isTrigger = false;
	public Animator gateAL, gateAR;

	void Start()
	{
		gateAL.enabled = false;
		gateAR.enabled = false;
		this.gameObject.GetComponent<DialogueTrigger>().enabled = false;
	}

	public void TriggerDialogue()
	{
		FindObjectOfType<DialogueManager>().StartDialogue(this.dialogue);
	}


	private void OnTriggerEnter(Collider other)
	{
		if (!this.gameObject.GetComponent<DialogueTrigger>().enabled)
		{
			this.TriggerDialogue();
			gateAL.enabled = true;
			gateAR.enabled = true;
		}
		_isTrigger = true;
	}
}
