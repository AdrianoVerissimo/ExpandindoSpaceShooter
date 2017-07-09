using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
	public Canvas canvasPause = null;

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

			//exibe o pause caso tenha sido atribuído um canvas para menu de pause
			if (canvasPause != null)
				canvasPause.gameObject.SetActive (true);
		}
		else
		{
			Time.timeScale = 1;

			//esconde o pause caso tenha sido atribuído um canvas para menu de pause
			if (canvasPause != null)
				canvasPause.gameObject.SetActive (false);
		}
	}

	public bool IsPaused()
	{
		if (Time.timeScale == 0)
			return true;
		
		return paused;
	}
}
