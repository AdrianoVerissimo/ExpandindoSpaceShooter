using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotPowerUp : MonoBehaviour {

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player");
		if (player == null)
		{
			Debug.Log ("O objeto do jogador não foi encontrado.");
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player"))
		{
			Destroy(gameObject);
			PlayerController playerController = player.GetComponent<PlayerController> ();
			playerController.UpgradeShot (2);
		}
	}
}
