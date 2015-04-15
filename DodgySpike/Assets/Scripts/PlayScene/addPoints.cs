using UnityEngine;
using System.Collections;

// adds a chosen number of points to the score whenever the spike passes the score line

public class addPoints : MonoBehaviour {
	public int scoreValue;
	private GameController gameController;
	public GameObject pointSound;

	// selects the game controller object
	void Start (){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null){
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null){
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// if a spike (enemy) object touches the scoreline and the game is not over add a score via the game controller
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Enemy" && gameController.gameOver == false){
			gameController.AddScore (scoreValue);
			//audio.Play ();
			Instantiate(pointSound);
		}
	}
}
