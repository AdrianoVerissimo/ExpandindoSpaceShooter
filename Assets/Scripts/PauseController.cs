﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
	public Canvas canvasPause = null; //canvas para o menu de pausa
	public Button firstButton;
	public bool enablePauseButton = true;

	private bool paused = false; //indica se o jogo foi pausado

	private PlayerController playerController;

	void Awake()
	{
		GameObject objPlayer = GameObject.FindGameObjectWithTag ("Player");
		try
		{
			if (objPlayer == null)
				throw new UnityException("Objeto player não encontrado em PauseController.");

			playerController = objPlayer.GetComponent<PlayerController> ();

			if(playerController == null)
					throw new UnityException("Script PlayerController.cs não encontrado em PauseController.");
		}
		catch (System.Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	void Update ()
	{
		if (enablePauseButton && Input.GetButtonDown ("Pause"))
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

			//adiciona um intervalo extra para se poder atirar novamente. Necessário para que não atire logo após sair do pause.
			playerController.UpdateNextFireTime(0.15f);

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
