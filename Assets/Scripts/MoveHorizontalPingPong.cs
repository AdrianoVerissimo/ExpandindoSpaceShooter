using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveHorizontalPingPong : MonoBehaviour {

	public float speed = 1;

	void Start()
	{
		SetHorizontalSpeed ();
	}

	void FixedUpdate()
	{
		if (transform.position.x >= 4.3f || transform.position.x <= -4.3f)
		{
			speed *= -1;
			SetHorizontalSpeed ();
		}
	}

	//define a velocidade e direção horizontal
	void SetHorizontalSpeed()
	{
		GetComponent<Rigidbody> ().velocity = transform.right * speed;
	}
}
