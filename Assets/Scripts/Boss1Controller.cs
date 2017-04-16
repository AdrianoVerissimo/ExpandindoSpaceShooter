using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Controller : MonoBehaviour {

	public float initialStopZ = 12f; //indica a posição Z que o chefe parará enquanto aparece

	//referências para scripts
	private Mover mover;
	private MoveHorizontalPingPong moveHorizontalPingPong;
	private WeaponControllerBoss1 weaponControllerBoss1;

	private bool aparecer = true; //indica se o chefe ainda está aparecendo
	private bool errorLoadObject = false; //indica se ocorreu algum erro ao ler script

	// Use this for initialization
	void Start ()
	{
		try
		{
			//pega scripts
			mover = GetComponent<Mover>();
			moveHorizontalPingPong = GetComponent<MoveHorizontalPingPong>();
			weaponControllerBoss1 = GetComponent<WeaponControllerBoss1> ();

			//houve erro ao ler encontrar algum script
			if (mover == null)
				throw new UnityException("É necessário usar o script 'Mover.cs'.");
			if (moveHorizontalPingPong == null)
				throw new UnityException("É necessário usar o script 'MoveHorizontalPingPong.cs'.");
			if (weaponControllerBoss1 == null)
				throw new UnityException("É necessário usar o script 'WeaponControllerBoss1.cs'.");

		}
		catch (System.Exception ex)
		{
			//exibe mensagem de erro
			if (ex.Message != null)
				Debug.Log (ex.Message);
			
			errorLoadObject = true; //marca que houve erro em leitura de script
		}

		//encerra
		if (errorLoadObject)
			return;

		//habilita/desabilita scripts
		mover.enabled = true;
		moveHorizontalPingPong.enabled = false;
		weaponControllerBoss1.enabled = false;
	}
	
	void FixedUpdate ()
	{
		//encerra
		if (errorLoadObject)
			return;

		//chegou na posição desejada
		if (aparecer && transform.position.z <= initialStopZ)
		{
			//habilita/desabilita scripts
			aparecer = false;
			mover.enabled = false;
			moveHorizontalPingPong.enabled = true;
			weaponControllerBoss1.enabled = true;
		}
	}
}
