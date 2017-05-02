using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleFormation : MonoBehaviour
{

	public int qtd = 1;
	public float startWait = 0f;
	public float spawnWait = 1f;
	public Vector3 spawnValues;

	public GameObject spawnObject;

	// Use this for initialization
	void Start ()
	{
		StartCoroutine(Spawn ());
	}

	IEnumerator Spawn()
	{
		yield return new WaitForSeconds (startWait); //espera X segundos para começar

		Vector3 spawnPosition = new Vector3(Random.Range (-spawnValues.x, spawnValues.x), spawnValues.y, spawnValues.z);
		Quaternion spawnRotation = Quaternion.identity; //sem rotação

		for (int count = 0; count <= qtd; count++)
		{
			Instantiate (spawnObject, spawnPosition, spawnRotation); //instancia o asteróide
			yield return new WaitForSeconds (spawnWait); //espera X segundos
		}

	}
}
