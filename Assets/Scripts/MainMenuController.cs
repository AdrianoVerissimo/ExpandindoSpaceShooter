using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour {

	public DataController dataController;
	public Text highScoreLabel;

	private int highScore;

	void Awake()
	{
		Screen.SetResolution (600, 900, false);
	}

	// Use this for initialization
	void Start () {

		dataController.LoadPlayerProgress ();

		highScore = dataController.GetHighScore ();

		highScoreLabel.text = "Recorde: " + highScore.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
