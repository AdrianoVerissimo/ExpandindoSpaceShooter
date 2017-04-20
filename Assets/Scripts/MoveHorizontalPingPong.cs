using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontalPingPong : MonoBehaviour {

	public float speed = 1;
	public bool doTilt = true;
	public float tilt = 10f;

	private Rigidbody rigidBody;

	private float rotationSpeed;

	void Start()
	{
		SetHorizontalSpeed ();

		rigidBody = GetComponent<Rigidbody> ();
	}

	void FixedUpdate()
	{
		if (transform.position.x >= 4.3f || transform.position.x <= -4.3f)
		{
			speed *= -1;
			SetHorizontalSpeed ();
		}
		//faz efeito de rotação na nave
		/*rotationSpeed = Mathf.MoveTowards(rotationSpeed, 4, Time.deltaTime * speed);

		Vector3 rbVelocity = rigidBody.velocity;

		rigidBody.rotation = Quaternion.Euler (0.0f, 0.0f, rotationSpeed * -tilt);*/
	}

	//define a velocidade e direção horizontal
	void SetHorizontalSpeed()
	{
		GetComponent<Rigidbody> ().velocity = transform.right * speed;
	}
}
