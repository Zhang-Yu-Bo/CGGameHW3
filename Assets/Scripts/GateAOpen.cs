using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateAOpen : MonoBehaviour
{
	public Animator gateL, gateR;
    // Start is called before the first frame update
    void Start()
    {
		gateL.enabled = false;
		gateR.enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	void OnTriggerStay(Collider other)
	{
		if(other.tag=="Player")
		{
			if(Input.GetKey(KeyCode.E))
			{
				gateL.enabled = true;
				gateR.enabled = true;
			}
		}
	}
}
