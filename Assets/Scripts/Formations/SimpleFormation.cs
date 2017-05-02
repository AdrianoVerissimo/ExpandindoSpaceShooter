using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFormation : MonoBehaviour
{

	public int qtd = 1; //indica quantos deste objeto serão instanciados
	public float startWait = 0f; //tempo antes de começar
	public float spawnWait = 1f; //tempo entre cada instanciamento
	public Vector3 spawnValues; //armazena os valores da posição para instanciar

	public GameObject spawnObject; //objeto a se instanciar

	private GameController gameController; //script da controladora do jogo

	// Use this for initialization
	void Start ()
	{
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();

		StartCoroutine(Spawn ());
	}

	IEnumerator Spawn()
	{
		gameController.pauseWaves = true; //pausa a aparição dos outros inimigos

		yield return new WaitForSeconds (startWait); //espera X segundos para começar

		Vector3 spawnPosition = new Vector3(Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z); //posição X aleatória
		Quaternion spawnRotation = Quaternion.identity; //sem rotação

		//enquanto houverem inimigos para instanciar
		for (int count = 0; count < qtd; count++)
		{
			Instantiate (spawnObject, spawnPosition, spawnRotation); //instancia o asteróide
			yield return new WaitForSeconds (spawnWait); //espera X segundos
		}

		gameController.pauseWaves = false; //retorna a aparição dos outros inimigos
	}
}
