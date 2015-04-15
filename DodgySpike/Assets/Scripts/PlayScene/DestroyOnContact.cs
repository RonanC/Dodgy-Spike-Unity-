using UnityEngine;
using System.Collections;

// this class destroys any objects that come in contact with it (when the player touches the spike or walls)
// attached to the spikes(3) and boundary

public class DestroyOnContact : MonoBehaviour {
	public GameObject playerExplosion;
	private GameController gameController;

	// retrieves game controller
	void Start (){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null){
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null){
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	// if player touches a spike then gameover & explosion
	void OnTriggerEnter2D(Collider2D other){
		if(other.gameObject.tag == "Player" && gameObject.tag == "Enemy"){
			Destroy(other.gameObject);
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);

			gameController.GameOver ();
		}
	}
	
	// if player exits the boundary then gameover & explosion
	void OnTriggerExit2D(Collider2D other){
		if(other.gameObject.tag == "Player"){
			Destroy(other.gameObject);
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			
			gameController.GameOver ();
		}
	}
}
