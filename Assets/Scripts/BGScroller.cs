using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

	public float scrollSpeed;
	public float tileSizeZ;

	private Vector3 startPosition;

	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		//gera uma nova posição de acordo com a velocidade e o limite de altura passada como parâmetro
		float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ);
		//atualiza a posição do background de acordo com a nova posição gerada anteriormente
		//isso permite com que o background faça um loop, pois por ser o dobro da tela, ao chegar na metade dele mesmo ele repetirá
		transform.position = startPosition + Vector3.forward * newPosition;
	}
}
