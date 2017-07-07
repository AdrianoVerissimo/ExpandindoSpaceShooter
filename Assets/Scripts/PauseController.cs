using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	public Canvas canvasPause;

	private bool paused = false;

	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.P))
		{
			Pause (!paused);
		}
	}

	public void Unpause()
	{
		Pause (false);
	}

	public void Pause(bool value = true)
	{
		paused = value;

		if (paused)
		{
			Time.timeScale = 0;
			canvasPause.gameObject.SetActive (true);
		}
		else
		{
			Time.timeScale = 1;
			canvasPause.gameObject.SetActive (false);
		}
	}

	public bool GetPaused()
	{
		return paused;
	}
}
