﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelObject
{
	public GameObject spawnObject; //objeto a se instanciar

	public int qtd = 1; //indica quantos deste objeto serão instanciados
	public float startWait = 0f; //tempo antes de começar
	public float spawnWait = 1f; //tempo entre cada instanciamento
	public Vector3 spawnValues; //armazena os valores da posição para instanciar

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
			yield return new WaitForSeconds (spawnWait); //espera X segundos
		}
	}
}

[System.Serializable]
public class LevelConfig
{
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
			Debug.Log (monoBehaviour);
			monoBehaviour.StartCoroutine(levelObjects [count].Spawn ());

			//while (!levelObjects[count].terminou)
			//	yield return null; //continua execução do loop no próximo frame
		}

		gameController.pauseWaves = false; //retorna a aparição dos outros inimigos

		yield return null;
	}
}