﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
	public Text scoreText;
	public Text gameplayHighScoreText;
	public Text gameOverText;

	public Text finalScoreText;
	public Text highScoreText;

	public Text txtTiros;
	public Text txtHits;

	public Text shootHitText;
	public Text enemiesDestroyedText;
	public Text StageScoreText;

	public int currentLevel = 0;
	public LevelConfig[] levelConfig; //array que contém a configuração de cada level/onda

	public bool pauseWaves = false; //define se pausará a instanciação dos objetos

 	private int score = 0;
	private int stageScore = 0;
	private int hitScore = 0;
	private int enemiesScore = 0;

	public int shootCount = 0;
	public int shootHit = 0;

	public int enemiesCount = 0;
	public int enemiesDestroyed = 0;

	public Canvas canvasGameOver;

	private bool gameOver;

	private DataController dataController;
	private PlayerController playerController;

	void Start()
	{
		gameOver = false;

		gameOverText.text = "";

		finalScoreText.text = "";
		shootHitText.text = "";
		enemiesDestroyedText.text = "";
		StageScoreText.text = "";
		highScoreText.text = "";

		try
		{
			GameObject objPlayer = GameObject.FindGameObjectWithTag ("Player");
			if (!objPlayer)
				throw new UnityException("Objeto Player não encontrado. Encerrando execução de script.");
			playerController = objPlayer.GetComponent<PlayerController>();
			if (!playerController)
				throw new UnityException("Player Controller não encontrado. Encerrando execução de script.");
		}
		catch (System.Exception ex)
		{
			Debug.Log (ex.Message);
			return;
		}

		LoadData ();

		UpdateScore (score);

		enemiesCount = GetEnemiesCount ();

		//executa uma função em paralelo, sem travar a execução até que ela seja terminada
		StartCoroutine(SpawnWaves ());
	}

	//carrega informações de jogos anteriores
	void LoadData()
	{
		dataController = GameObject.FindGameObjectWithTag ("DataController").GetComponent<DataController>();
		dataController.LoadPlayerProgress ();

		gameplayHighScoreText.text = "Recorde: " + dataController.GetLocalHighScore ();
	}

	//em cada frame
	void Update()
	{
		/*txtTiros.text = enemiesCount.ToString();
		txtHits.text = enemiesDestroyed.ToString();*/
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
				currentLevel = levelConfig.Length - 1;

				//se está em um chefe e ele foi vencido
				if (levelConfig[currentLevel].isBoss && levelConfig[currentLevel].bossDefeated)
					GameOver (true); //mostrar que venceu a fase
			}

			//se marcado que houve game over
			if (gameOver)
				break; //encerrar loop de spawns

			yield return new WaitForSeconds (levelConfig[currentLevel].levelEndWait); //espera X segundos para começar
		}
	}

	//add pontuação
	public void AddScore(int newScoreValue)
	{
		score += newScoreValue;
		UpdateScore (score);
	}

	//atualiza texto da pontuação
	void UpdateScore(int value)
	{
		int highScore = dataController.GetLocalHighScore ();
		scoreText.text = "Score: " + value;

		//se pontuação passou o recorde, atualizar
		if (value > highScore)
			gameplayHighScoreText.text = "Recorde: " + value;
	}

	//exibe e ativa o Game Over
	public void GameOver(bool venceu = false)
	{
		hitScore = GetHitScore ();
		enemiesScore = GetEnemiesDestroyedScore ();
		stageScore = score + hitScore + enemiesScore;

		UpdateScore (stageScore);
		dataController.SubmitHighScore (stageScore); //envia recorde

		if (venceu)
			gameOverText.text = "Voce Venceu!";
		else
			gameOverText.text = "Game Over!";			
		
		gameOver = true;

		finalScoreText.text = "Pontos: " + score.ToString();
		shootHitText.text = GetShootHitPercent() + "% Precisao: " + hitScore;
		enemiesDestroyedText.text = GetEnemiesDestroyedPercent() + "% Inimigos Destruidos: " + enemiesScore;
		StageScoreText.text = "Pontos Finais: " + stageScore;
		highScoreText.text = "Recorde: " + dataController.GetLocalHighScore().ToString();

		//exibe menu de Game Over caso tenha sido atribuído
		if (canvasGameOver.gameObject)
			canvasGameOver.gameObject.SetActive (true);

		playerController.SetCanMove (false);

		//destrói a controladora do pause
		DestroyPauseController ();
	}

	public void DefeatedBoss(bool defeat)
	{
		if (!levelConfig [currentLevel].isBoss)
			return;
		
		levelConfig [currentLevel].bossDefeated = defeat;
	}

	public void AddShootCount(int value)
	{
		shootCount += value;
	}

	public void AddShootHit(int value)
	{
		shootHit += value;
	}

	/**
	 * Pega o percentual de tiros acertados
	 * */
	public float GetShootHitPercent()
	{
		if (shootHit == 0)
			return 0;
		
		float value = (shootHit * 100) / shootCount;
		return value;
	}

	/**
	 * Pega a pontuação final do estágio
	 * */
	public int GetHitScore()
	{
		float value = GetShootHitPercent () * 10;
		if (value == 0)
			return 0;
		
		return Mathf.RoundToInt (value);
	}

	public int GetEnemiesCount()
	{
		int value = 0;
		foreach (var item in levelConfig)
		{
			value += item.GetEnemiesCount ();
		}
		return value;
	}

	public void AddEnemiesDestroyed(int value)
	{
		enemiesDestroyed += value;
	}

	/**
	 * Pega o percentual de inimigos destruídos
	 * */
	public float GetEnemiesDestroyedPercent()
	{
		if (enemiesDestroyed == 0)
			return 0;
		
		float value = (enemiesDestroyed * 100) / enemiesCount;
		return value;
	}

	/**
	 * Pega a pontuação de acordo com os inimigos destruídos
	 * */
	public int GetEnemiesDestroyedScore()
	{
		float value = GetEnemiesDestroyedPercent () * 15;
		if (value == 0)
			return 0;
		
		return Mathf.RoundToInt (value);
	}

	/**
	 * Destrói a controladora do pause
	 * */ 
	public void DestroyPauseController()
	{
		GameObject objPause = GameObject.FindGameObjectWithTag ("PauseController");
		if (objPause)
			Destroy (objPause);
	}

	//retorna se o jogo acabou
	public bool IsGameOver()
	{
		return gameOver;
	}
}