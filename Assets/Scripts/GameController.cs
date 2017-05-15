using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
	public Text scoreText;
	public Text gameplayHighScoreText;
	public Text restartText;
	public Text gameOverText;

	public Text finalScoreText;
	public Text highScoreText;

	public int currentLevel = 0;
	public LevelConfig[] levelConfig; //array que contém a configuração de cada level/onda

	public bool pauseWaves = false; //define se pausará a instanciação dos objetos

 	private int score;
	private bool gameOver;
	private bool restart;

	private DataController dataController;

	void Start()
	{
		gameOver = false;
		restart = false;

		restartText.text = "";
		gameOverText.text = "";

		finalScoreText.text = "";
		highScoreText.text = "";

		LoadData ();

		score = 0;
		UpdateScore ();

		//executa uma função em paralelo, sem travar a execução até que ela seja terminada
		StartCoroutine(SpawnWaves ());
	}

	//carrega informações de jogos anteriores
	void LoadData()
	{
		dataController = GameObject.FindGameObjectWithTag ("DataController").GetComponent<DataController>();
		dataController.LoadPlayerProgress ();

		gameplayHighScoreText.text = "High Score: " + dataController.GetLocalHighScore ();
	}

	//em cada frame
	void Update()
	{
		//marcou para reiniciar o jogo
		if (restart) {
			//apertou a tecla 'R'
			if (Input.GetKeyDown (KeyCode.R)) {
				//Application.LoadLevel (Application.loadedLevel); -> removido por ser descontinuado
				SceneManager.LoadScene("Game"); //recarrega a scene do jogo
			}
		}
	}

	//cria um asteróide em posição x aleatória e fora da tela
	IEnumerator SpawnWaves()
	{
		//loop para ficar sempre executando
		while (true)
		{
			yield return new WaitForSeconds (levelConfig[currentLevel].levelStartWait); //espera X segundos para começar

			StartCoroutine(levelConfig[currentLevel].SpawnWave());

			//pausou instanciação
			while (pauseWaves)
				yield return null; //espera 1 frame

			//está em um chefe, o chefe não foi derrotado e não houve Game Over
			//o código se mantém aqui até o chefe ser derrotado ou o jogador perder
			while (levelConfig [currentLevel].isBoss && !levelConfig [currentLevel].bossDefeated && !gameOver) {
				yield return null; //continua execução do loop no próximo frame
			}

			//sobe um level
			currentLevel++;

			if (currentLevel >= levelConfig.Length)
			{
				GameOver (true);
				currentLevel = levelConfig.Length - 1;
			}

			//se marcado que houve game over
			if (gameOver) {
				restartText.text = "Pressione 'R' para reiniciar o jogo."; //exibe texto para reiniciar o jogo
				restart = true; //marca que ocorrerá o reinício do jogo

				break;
			}

			yield return new WaitForSeconds (levelConfig[currentLevel].levelEndWait); //espera X segundos para começar
		}
	}

	//add pontuação
	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore ();
	}

	//atualiza texto da pontuação
	void UpdateScore()
	{
		int highScore = dataController.GetLocalHighScore ();
		scoreText.text = "Score: " + score;

		//se pontuação passou o recorde, atualizar
		if (score > highScore)
			gameplayHighScoreText.text = "High Score: " + score;
	}

	//exibe e ativa o Game Over
	public void GameOver(bool venceu = false)
	{
		dataController.SubmitHighScore (score); //envia recorde

		if (!venceu)
			gameOverText.text = "Game Over!";
		else
			gameOverText.text = "You Win!";
		
		gameOver = true;

		finalScoreText.text = "Score: " + score.ToString();
		highScoreText.text = "High Score: " + dataController.GetLocalHighScore().ToString();

	}

	public void DefeatedBoss(bool defeat)
	{
		if (!levelConfig [currentLevel].isBoss)
			return;
		
		levelConfig [currentLevel].bossDefeated = defeat;
	}
}
