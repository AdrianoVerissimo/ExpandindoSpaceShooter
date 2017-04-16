using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int life = 1; //energia

	private GameController gameController;
	private Animator damageAC;

	void Start()
	{
		//busca a instancia de Game Controller
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		//se encontrou, pegar essa referência
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent<GameController>();
		}
		//se a referência não foi pega, então exibir mensagem de log
		if (gameController == null)
		{
			Debug.Log ("Não foi possível encontrar o script 'GameController'.");
		}

		damageAC = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other)
	{
		//se colidir com boundary, abortar script
		if (other.CompareTag ("Boundary") || other.CompareTag("Enemy"))
			return;

		//diminui energia
		life--;
		if (life > 0 && damageAC != null)
		{
			damageAC.SetTrigger("Damage");
			Debug.Log ("oi");
		}

		//só gera explosão se possui uma e não tem mais energia
		if (explosion != null && life == 0)
		{
			Instantiate (explosion, transform.position, transform.rotation); //instancia explosão
		}

		//se quem colidiu foi o jogador
		if (other.CompareTag ("Player"))
		{
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation); //instancia explosão do jogador
			gameController.GameOver(); //ativa o Game Over
		}

		//não tem mais energia
		if (life == 0)
		{
			//add pontos
			gameController.AddScore (scoreValue);
			Destroy (gameObject);
		}

		Destroy (other.gameObject);	
	}
}
