using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ButtonAutoSelect : MonoBehaviour {

	private Button button;

	// Use this for initialization
	void Start () {
		button = GetComponent<Button> ();

		DoSelect ();
	}
	
	public void DoSelect()
	{
		if (button == null)
			return;

		button.Select ();
	}
}
