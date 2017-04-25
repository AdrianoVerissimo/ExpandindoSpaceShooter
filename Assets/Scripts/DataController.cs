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

	public void LoadPlayerProgress()
	{
		playerProgress = new PlayerProgress ();

		playerProgress.highScore = GetHighScore ();
	}

	public void SetHighScore(int score)
	{
		PlayerPrefs.SetInt ("HighScore", score);
	}

	public int GetHighScore()
	{
		if (PlayerPrefs.HasKey ("HighScore"))
			return PlayerPrefs.GetInt ("HighScore");

		return 0;
	}

	public void SubmitHighScore(int score)
	{
		if (!PlayerPrefs.HasKey("HighScore") || score > PlayerPrefs.GetInt ("HighScore"))
			SetHighScore (score);
	}
}
