using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotator : MonoBehaviour {

	public float tumble = 1;

	void Start()
	{
		//define uma velocidade angular aleatória, determinando a velocidade através da variável tumble
		GetComponent<Rigidbody> ().angularVelocity = Random.insideUnitSphere * tumble;
	}
}
