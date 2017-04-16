using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerBoss1 : MonoBehaviour {

	public GameObject shot;
	public Transform[] shotSpawn;
	public float fireRate;
	public float delayBeforeShots;
	public float pauseAfterShots;
	public int numberOfShots = 5;

	private bool shotSide = true;
	private bool shotMiddle = false;
	private AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		if (shotSpawn.Length == 0)
			return;
		
		audioSource = GetComponent<AudioSource> ();
		StartCoroutine (Fire ());
		//InvokeRepeating ("Fire", delay, fireRate);
	}

	IEnumerator Fire()
	{
		yield return new WaitForSeconds (delayBeforeShots); //espera X segundos para começar

		while (true)
		{
			for (int count = 0; count < numberOfShots; count++)
			{
				if (shotSide)
				{
					Instantiate (shot, shotSpawn [0].position, shotSpawn [0].rotation);
					Instantiate (shot, shotSpawn [1].position, shotSpawn [1].rotation);
				}
				else if (shotMiddle)
				{
					Instantiate (shot, shotSpawn [2].position, shotSpawn [2].rotation);
					Instantiate (shot, shotSpawn [3].position, shotSpawn [3].rotation);
				}
				audioSource.Play ();

				yield return new WaitForSeconds (fireRate); //espera X segundos para começar
			}

			ChangeShotType ();

			yield return new WaitForSeconds (pauseAfterShots); //espera X segundos para começar
		}
	}

	void ChangeShotType()
	{
		shotSide = !shotSide;
		shotMiddle = !shotMiddle;
	}
}
