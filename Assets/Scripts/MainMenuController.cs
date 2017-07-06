using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public DataController dataController;
	public Text highScoreLabel;

	private int highScore;

	// Use this for initialization
	void Start () {

		dataController.LoadPlayerProgress ();

		highScore = dataController.GetHighScore ();

		highScoreLabel.text = "Recorde: " + highScore.ToString();
	}
}
