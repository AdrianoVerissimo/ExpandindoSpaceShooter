using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelObject
{
	public GameObject spawnObject; //objeto a se instanciar

	public int qtd = 1; //indica quantos deste objeto serão instanciados

	public float startWait = 0f; //tempo antes de começar
	public float spawnWait = 1f; //tempo entre cada instanciamento
	public float endWait = 1f; //tempo após acabar

	public Vector3 spawnValues = new Vector3 (0f, 0f, 16f); //armazena os valores da posição para instanciar

	public bool terminou; //indica se os inimigos dessa onda terminaram

	private MonoBehaviourObject monoBehaviour;

	public IEnumerator Spawn()
	{
		monoBehaviour = GameObject.Find ("MonoBehaviourObject").GetComponent<MonoBehaviourObject> ();

		yield return new WaitForSeconds (startWait); //espera X segundos para começar

		Vector3 spawnPosition = new Vector3(Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); //posição X aleatória
		Quaternion spawnRotation = Quaternion.identity; //sem rotação

		//enquanto houverem inimigos para instanciar
		for (int count = 0; count < qtd; count++)
		{
			GameObject.Instantiate (spawnObject, spawnPosition, spawnRotation); //instancia o asteróide

			if (count + 1 < qtd)
				yield return new WaitForSeconds (spawnWait); //espera X segundos
		}

		yield return new WaitForSeconds (endWait); //espera X segundos

		terminou = true; //indica que a onda de inimigos parou de ser instanciada
	}
}

[System.Serializable]
public class LevelConfig
{
	public float levelStartWait = 1f;
	public float levelEndWait = 1f;

	public bool isBoss = false, bossDefeated = false;
	public LevelObject[] levelObjects;

	private GameController gameController;
	private MonoBehaviourObject monoBehaviour;

	public IEnumerator SpawnWave()
	{
		monoBehaviour = GameObject.Find ("MonoBehaviourObject").GetComponent<MonoBehaviourObject> ();

		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>(); //script da controladora do jogo

		gameController.pauseWaves = true; //pausa a aparição dos outros inimigos

		int objectCount = levelObjects.Length;


		for (int count = 0; count < objectCount; count++)
		{
			monoBehaviour.StartCoroutine(levelObjects [count].Spawn ());

			while (!levelObjects[count].terminou)
				yield return null; //continua execução do loop no próximo frame
		}

		gameController.pauseWaves = false; //retorna a aparição dos outros inimigos

		yield return null;
	}

	public int GetEnemiesCount()
	{
		int value = 0;
		foreach (var item in levelObjects)
		{
			value += item.qtd;
		}
		return value;
	}
}
