using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[System.Serializable]
public class LevelConfig
{
	public bool isBoss = false, bossDefeated = false;
	public GameObject[] levelObjects;
}

public class GameController : MonoBehaviour {

	public GameObject[] hazards; //array com os asteróides passados

	public Vector3 spawnValues;
	public Vector3 bossSpawnValues;

	public int hazardCount = 1;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

	public Text finalScoreText;
	public Text highScoreText;

	public int currentLevel = 0;
	public LevelConfig[] levelConfig;

 	private int score;
	private bool gameOver;
	private bool restart;

	void Start()
	{
		gameOver = false;
		restart = false;

		restartText.text = "";
		gameOverText.text = "";

		finalScoreText.text = "";
		highScoreText.text = "";

		score = 0;
		UpdateScore ();

		//executa uma função em paralelo, sem travar a execução até que ela seja terminada
		StartCoroutine(SpawnWaves ());
	}

	//em cada frame
	void Update()
	{
		//marcou para reiniciar o jogo
		if (restart) {
			//apertou a tecla 'R'
			if (Input.GetKeyDown (KeyCode.R)) {
				//Application.LoadLevel (Application.loadedLevel); -> removido por ser descontinuado
				SceneManager.LoadScene("Main"); //recarrega a scene do jogo
			}
		}
	}

	//cria um asteróide em posição x aleatória e fora da tela
	IEnumerator SpawnWaves()
	{
		yield return new WaitForSeconds (startWait); //espera X segundos para começar

		//loop para ficar sempre executando
		while (true) {

			int levelObjectsCount = levelConfig[currentLevel].levelObjects.Length;

			//para x asteróides
			for (int i = 0; i < levelObjectsCount; i++) {

				//pega uma posição aleatória da array de asteróides
				//GameObject hazard = hazards[Random.Range(0, hazards.Length)];
				GameObject hazard = levelConfig[currentLevel].levelObjects[i];

				Vector3 spawnPosition;
				if (levelConfig [currentLevel].isBoss)
					spawnPosition = bossSpawnValues;
				else
					spawnPosition = new Vector3(Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);

				//define uma posição de acordo com o que foi passado
				Quaternion spawnRotation = Quaternion.identity; //sem rotação
				Instantiate (hazard, spawnPosition, spawnRotation); //instancia o asteróide

				//espera x segundos para instanciar outro asteróide
				yield return new WaitForSeconds (spawnWait);
			}

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

			//espera x segundos até começar outra onda de asteróides
			yield return new WaitForSeconds (waveWait);



			//se marcado que houve game over
			if (gameOver) {
				restartText.text = "Pressione 'R' para reiniciar o jogo."; //exibe texto para reiniciar o jogo
				restart = true; //marca que ocorrerá o reinício do jogo
				break;
			}
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
		scoreText.text = "Score: " + score;
	}

	//exibe e ativa o Game Over
	public void GameOver(bool venceu = false)
	{
		if (!venceu)
			gameOverText.text = "Game Over!";
		else
			gameOverText.text = "You Win!";
		
		gameOver = true;

		finalScoreText.text = "Score: " + score.ToString();
		highScoreText.text = "High Score: " + score.ToString();

	}

	public void DefeatedBoss(bool defeat)
	{
		if (!levelConfig [currentLevel].isBoss)
			return;
		
		levelConfig [currentLevel].bossDefeated = defeat;
	}
}
