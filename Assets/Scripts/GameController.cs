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
		shootHitText.text = "";
		enemiesDestroyedText.text = "";
		StageScoreText.text = "";
		highScoreText.text = "";

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
		dataController.SetHighScore (0);
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
				SceneManager.LoadScene("Fase1"); //recarrega a scene do jogo
			}
		}

		txtTiros.text = enemiesCount.ToString();
		txtHits.text = enemiesDestroyed.ToString();
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
		UpdateScore (score);
	}

	//atualiza texto da pontuação
	void UpdateScore(int value)
	{
		int highScore = dataController.GetLocalHighScore ();
		scoreText.text = "Score: " + value;

		//se pontuação passou o recorde, atualizar
		if (value > highScore)
			gameplayHighScoreText.text = "High Score: " + value;
	}

	//exibe e ativa o Game Over
	public void GameOver(bool venceu = false)
	{
		hitScore = getHitScore ();
		enemiesScore = getEnemiesDestroyedScore ();
		stageScore = score + hitScore + enemiesScore;

		UpdateScore (stageScore);
		dataController.SubmitHighScore (stageScore); //envia recorde

		if (!venceu)
			gameOverText.text = "Game Over!";
		else
			gameOverText.text = "You Win!";
		
		gameOver = true;

		finalScoreText.text = "Score: " + score.ToString();
		shootHitText.text = getShootHitPercent() + "% Precision: " + hitScore;
		enemiesDestroyedText.text = getEnemiesDestroyedPercent() + "% Enemies Destroyed: " + enemiesScore;
		StageScoreText.text = "Stage Score: " + stageScore;
		highScoreText.text = "High Score: " + dataController.GetLocalHighScore().ToString();
	}

	public void DefeatedBoss(bool defeat)
	{
		if (!levelConfig [currentLevel].isBoss)
			return;
		
		levelConfig [currentLevel].bossDefeated = defeat;
	}

	public void addShootCount(int value)
	{
		shootCount += value;
	}

	public void addShootHit(int value)
	{
		shootHit += value;
	}

	/**
	 * Pega o percentual de tiros acertados
	 * */
	public float getShootHitPercent()
	{
		float value = (shootHit * 100) / shootCount;
		return value;
	}

	/**
	 * Pega a pontuação final do estágio
	 * */
	public int getHitScore()
	{
		float value = getShootHitPercent () * 10;
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
	public float getEnemiesDestroyedPercent()
	{
		float value = (enemiesDestroyed * 100) / enemiesCount;
		return value;
	}

	/**
	 * Pega a pontuação de acordo com os inimigos destruídos
	 * */
	public int getEnemiesDestroyedScore()
	{
		float value = getEnemiesDestroyedPercent () * 15;
		return Mathf.RoundToInt (value);
	}
}
