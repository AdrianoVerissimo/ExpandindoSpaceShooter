using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataController :MonoBehaviour
{
	private PlayerProgress playerProgress;

	void Start()
	{
		LoadPlayerProgress ();
	}

	//carrega as informações de jogos anteriores do jogador
	public void LoadPlayerProgress()
	{
		playerProgress = new PlayerProgress ();

		playerProgress.highScore = GetHighScore ();
	}

	//salva uma pontuação como recorde
	public void SetHighScore(int score)
	{
		PlayerPrefs.SetInt ("HighScore", score);
	}

	//pega um recorde salvo
	public int GetHighScore()
	{
		if (PlayerPrefs.HasKey ("HighScore"))
			return PlayerPrefs.GetInt ("HighScore");

		return 0;
	}

	//pega o recorde salvo na memória
	public int GetLocalHighScore ()
	{
		return playerProgress.highScore;
	}

	//retorna o recorde salvo na memória
	public void SetLocalHighScore(int score)
	{
		playerProgress.highScore = score;
	}

	//envia o recorde. Se for maior que o atual recorde, então atualizar
	public void SubmitHighScore(int score)
	{
		if (!PlayerPrefs.HasKey ("HighScore") || score > PlayerPrefs.GetInt ("HighScore"))
		{
			SetHighScore (score);
			SetLocalHighScore (score);
		}
	}
}
