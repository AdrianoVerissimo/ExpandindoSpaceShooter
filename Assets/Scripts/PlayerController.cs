using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe utilizada para definir limites de movimentação
[System.Serializable]
public class Boundary
{
	public float xMin, xMax, zMin, zMax;
}

//classe com controles e ações da nave
public class PlayerController : MonoBehaviour {

	public float speed = 1; //velocidade de movimentação da nave
	public float tilt = 1; //usado para tombar a nave ao se mover horizontalmente
	public Boundary boundary; //limites de movimentação
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate, nextFire;

	void Update()
	{
		//se apertou botão e já passou tempo suficiente para atirar
		if (Input.GetButton ("Fire1") && Time.time >= nextFire) {
			GetComponent<AudioSource> ().Play ();
			nextFire = Time.time + fireRate; //atualiza tempo necessário para atirar
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation); //instancia o tiro
		}
	}

	void FixedUpdate()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal"); //input horizontal
		float moveVertical = Input.GetAxis ("Vertical"); //input vertical

		//define a movimentação da nave de acordo com os inputs e velocidade
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		GetComponent<Rigidbody> ().velocity = movement * speed;

		//limita as áreas em que a nave pode ir
		//Mathf.Clamp() arredonda um valor de acordo com valores mínimos e máximos passados por parâmetro
		GetComponent<Rigidbody> ().position = new Vector3
		(
			Mathf.Clamp(GetComponent<Rigidbody> ().position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(GetComponent<Rigidbody> ().position.z, boundary.zMin, boundary.zMax)
		);

		//faz a nave rotacionar horizontalmente de acordo com a velocidade do eixo X
		GetComponent<Rigidbody> ().rotation = Quaternion.Euler (
		0.0f,
		0.0f,
		GetComponent<Rigidbody> ().velocity.x * -tilt);
	}
}
