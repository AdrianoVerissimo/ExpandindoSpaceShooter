using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour {

	public float lifetime;

	// Use this for initialization
	void Start () {
		//destrói um objeto depois de x lifetime
		Destroy (gameObject, lifetime);
	}
}
