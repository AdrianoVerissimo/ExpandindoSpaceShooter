using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByContact : MonoBehaviour {

	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int life = 1; //energia

	private GameController gameController;
	private GameObject objGameController;

	private Animator damageAC;

	void Start()
	{
		try
		{
			objGameController = GameObject.FindGameObjectWithTag("GameController");
			if (objGameController == null)
				throw new UnityException("É necessário ter o objeto 'Game Controller' para o chefe funcionar.");

			gameController = objGameController.GetComponent<GameController>();
			if (gameController == null)
				throw new UnityException("O objeto 'Game Controller' precisa ter o script 'GameController.cs'.");
		}
		catch (System.Exception ex)
		{
			if (ex.Message != null)
				Debug.Log (ex.Message);
		}

		damageAC = GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other)
	{
		//abandonar execução do script caso colida com algo que não deve colidir
		if (other.CompareTag ("Boundary") || other.CompareTag("Enemy") || other.CompareTag("Enemy Shot") || other.CompareTag("Boss"))
			return;

		//diminui energia
		life--;
		if (life > 0 && damageAC != null)
			damageAC.SetTrigger("Damage");

		//só gera explosão se possui uma e não tem mais energia
		if (explosion != null && life == 0)
		{
			Instantiate (explosion, transform.position, transform.rotation); //instancia explosão

			//se o objeto deste script não é um tiro de inimigo
			if (!gameObject.CompareTag("Enemy Shot"))
			{
				//add que 1 inimigo foi destruído
				gameController.AddEnemiesDestroyed (1);
			}
		}

		//se quem colidiu foi o jogador
		if (other.CompareTag ("Player")) {
			Instantiate (playerExplosion, other.transform.position, other.transform.rotation); //instancia explosão do jogador
			gameController.GameOver (false); //ativa o Game Over
		} else if (!gameObject.CompareTag("Enemy Shot")) //se o objeto deste script não é um tiro de inimigo
		{
			gameController.AddShootHit (1);
		}

		//não tem mais energia
		if (life <= 0)
		{
			//add pontos se não foi o jogador que colidiu
			if (!other.CompareTag ("Player"))
				gameController.AddScore (scoreValue);
			
			Destroy (gameObject);

			if (CompareTag("Boss"))
			{
				gameController.DefeatedBoss (true);
			}
		}

		Destroy (other.gameObject);	
	}
}
