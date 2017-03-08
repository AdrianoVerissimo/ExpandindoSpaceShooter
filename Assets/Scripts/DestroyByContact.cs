using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;

	private GameController gameController;

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
	}

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log (other.name); //exibe no console nome do objeto que destruiu o asteróide

		//se colidir com boundary, abortar script
		if (other.CompareTag ("Boundary") || other.CompareTag("Enemy"))
			return;

		if (explosion != null)
		{
			Instantiate (explosion, transform.position, transform.rotation); //instancia explosão
		}

		//se quem colidiu foi o jogador
		if (other.CompareTag ("Player"))
		{
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation); //instancia explosão do jogador
			gameController.GameOver(); //ativa o Game Over
		}

		//add pontos
		gameController.AddScore (scoreValue);

		//destrói o tiro e o asteróide
		//desde que esteja no mesmo frame, não importa a ordem que o Destroy é utilizado: o objeto não é destruído imediatamente,
		//e sim marcado para ser destruído no final do frame
		Destroy (other.gameObject);
		Destroy (gameObject);
	}
}
