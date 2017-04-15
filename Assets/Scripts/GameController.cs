using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class LevelConfig
{
	public GameObject[] levelObjects;
}

public class GameController : MonoBehaviour {

	public GameObject[] hazards; //array com os asteróides passados

	public Vector3 spawnValues;
	public int hazardCount = 1;
	public float spawnWait;
	public float startWait;
	public float waveWait;

	public GUIText scoreText;
	public GUIText restartText;
	public GUIText gameOverText;

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

				//define uma posição de acordo com o que foi passado
				Vector3 spawnPosition = new Vector3 (Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
				Quaternion spawnRotation = Quaternion.identity; //sem rotação
				Instantiate (hazard, spawnPosition, spawnRotation); //instancia o asteróide

				//espera x segundos para instanciar outro asteróide
				yield return new WaitForSeconds (spawnWait);
			}

			currentLevel++;
			if (currentLevel >= levelConfig.Length)
				currentLevel = levelConfig.Length - 1;

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
	public void GameOver()
	{
		gameOverText.text = "Game Over!";
		gameOver = true;
	}
}
