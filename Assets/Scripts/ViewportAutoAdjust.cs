using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script utilizado para calcular e estabelecer um determinado viewport independente da resolução que foi selecionada
public class ViewportAutoAdjust : MonoBehaviour {

	public float gameWidth; //largura desejada para o viewport
	public float gameHeight; //altura desejada para o viewport

	private int screenWidth; //largura da tela do jogo
	private int screenHeight; //altura da tela do jogo

	void Start () 
	{
		screenWidth = Screen.width;
		screenHeight = Screen.height;

		SetViewPort ();
	}

	void Update()
	{
		//se as dimensões da tela do jogo foram alteradas, atualizar viewport
		if (screenWidth != Screen.width || screenHeight != Screen.height)
		{
			screenWidth = Screen.width;
			screenHeight = Screen.height;
			SetViewPort ();
		}
	}

	//define o viewport baseando-se na largura e altura definidas
	public void SetViewPort()
	{
		// set the desired aspect ratio (the values in this example are
		// hard-coded for 16:9, but you could make them into public
		// variables instead so you can set them at design time)
		float targetaspect = gameWidth / gameHeight;

		// determine the game window's current aspect ratio
		float windowaspect = (float)screenWidth / (float)screenHeight;

		// current viewport height should be scaled by this amount
		float scaleheight = windowaspect / targetaspect;

		// obtain camera component so we can modify its viewport
		Camera camera = GetComponent<Camera>();

		// if scaled height is less than current height, add letterbox
		if (scaleheight < 1.0f)
		{  
			Rect rect = camera.rect;

			rect.width = 1.0f;
			rect.height = scaleheight;
			rect.x = 0;
			rect.y = (1.0f - scaleheight) / 2.0f;

			camera.rect = rect;
		}
		else // add pillarbox
		{
			float scalewidth = 1.0f / scaleheight;

			Rect rect = camera.rect;

			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;

			camera.rect = rect;
		}
	}
}
