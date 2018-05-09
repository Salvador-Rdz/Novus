//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour {

	public void Quit () {
		
		Debug.Log("Se ha salido del juego");
		Application.Quit(); //Esto te saca del juego (si estas en el editor no jala :P )
	}

	public void Retry () {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex); //Todo esto es para que vuelva a cargar la escena
	}
}
