using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
	public Canvas canvasPause = null; //canvas para o menu de pausa
	public Button firstButton;
	public bool enablePauseButton = true;

	private bool paused = false; //indica se o jogo foi pausado

	void Update ()
	{
		if (enablePauseButton && Input.GetKeyDown (KeyCode.P))
		{
			Pause (!paused);
		}
	}

	//despausa o jogo
	public void Unpause()
	{
		Pause (false);
	}

	//pausa ou despausa o jogo dependendo do que for passado
	public void Pause(bool value = true)
	{
		paused = value;

		if (paused)
		{
			Time.timeScale = 0;

			//exibe o pause caso tenha sido atribuído um canvas para menu de pause
			if (canvasPause != null)
			{
				canvasPause.gameObject.SetActive (true);
			}
		}
		else
		{
			Time.timeScale = 1;

			//esconde o pause caso tenha sido atribuído um canvas para menu de pause
			if (canvasPause != null)
				canvasPause.gameObject.SetActive (false);
		}
	}

	//verifica se o jogo está pausado
	public bool IsPaused()
	{
		if (Time.timeScale == 0)
			return true;
		
		return paused;
	}
}
