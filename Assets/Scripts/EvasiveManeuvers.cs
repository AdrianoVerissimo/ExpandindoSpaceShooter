using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvasiveManeuvers : MonoBehaviour {

	public Vector2 startWait;
	public Vector2 maneuverTime;
	public Vector2 maneuverWait;
	public float dodge;
	public float smoothing;
	public float tilt;
	public Boundary boundary;

	private float currentSpeed;
	private float targetManeuver;
	private Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody> ();
		currentSpeed = rb.velocity.z;
		StartCoroutine (Evade ());
	}

	//método responsável por fazer manobras evasivas
	IEnumerator Evade()
	{
		//tempo de espera antes de começar a se esquivar
		yield return new WaitForSeconds(Random.Range(startWait.x, startWait.y));

		while (true)
		{
			//pega velocidade que fará a manobra, levando sempre em consideração a posição X atual
			//se X estiver negativo, significa que está no lado esquerdo da tela: fazer manobra virando para direita
			//se X estiver positivo, significa que está no lado direito da tela: fazer manobra virando para esquerda
			targetManeuver = Random.Range (1, dodge) * -Mathf.Sign(transform.position.x);

			//espera um tempo até parar a manobra
			yield return new WaitForSeconds (Random.Range(maneuverTime.x, maneuverTime.y));
			targetManeuver = 0; //reseta tempo de manobra
			//espera um tempo até começar outra manobra
			yield return new WaitForSeconds (Random.Range(maneuverWait.x, maneuverWait.y));
		}
	}

	void FixedUpdate () {
		//faz um valor crescer estabelecendo um limite que não poderá ultrapassar
		//cresce a velocidade X até alcançar targetManeuver, com smoothing determinando a velocidade em que cresce
		float newManeuver = Mathf.MoveTowards (rb.velocity.x, targetManeuver, Time.deltaTime * smoothing);
		rb.velocity = new Vector3 (newManeuver, 0.0f, currentSpeed); //estabelece nova velocidade

		//impede com que a nave saia da tela
		rb.position = new Vector3 (
			Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
			0.0f,
			Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
		);

		//faz efeito de rotação na nave
		rb.rotation = Quaternion.Euler (0.0f, 0.0f, rb.velocity.x * -tilt);
	}
}
