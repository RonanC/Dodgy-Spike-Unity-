using UnityEngine;
using System.Collections;

using UnityEngine.UI;

// this class

// serializing is about storing and showing information, properties need to be serialised in order to be viewed in the inspector
[System.Serializable]
public class Boundary{
	public float xMin, xMax;
}

public class PlayerController : MonoBehaviour {
	// our input will only be a value between 0 to 1, speed can multiply this
	public float speed;
	public Boundary boundary;

	public float rotateSpeed = 3f;
	public int spinDirection = 1;

	private GameController gameController;
//	private PersistantData persistantData;

	private float initPlayerSpeed = 15f;

	public Text txtTest;

	// retrieves the game objects via their tags
	void Start (){
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}

		// multiplys the speed if on desktop
//		if (gameController.desktop == true) {
//			speed *= 2;
//		}
	}


//		void Update(){
//			Touch touch = Input.touches[0];
//			
//			
//			if (Input.touchCount == 1 && Time.time > nextFire && touch.phase == TouchPhase.Began) {
//				// jump
//			}
//		}

	// used if we want to have a fire event
//	void Update(){
//		Touch touch = Input.touches[0];
//		
//		// Input.GetButton("Jump")
//		if (Input.touchCount == 1 && Time.time > nextFire && touch.phase == TouchPhase.Began) {
//			nextFire = Time.time + fireRate;
//			//GameObject clone = (we don;t need a reference to this object)..... (as GameObject)
//			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
//			// as GameObject; simply instatiate this object as a game object, now we can reference this object
//			audio.Play (); // plays current audio clip, we can change clips to be played
//		}
//		// object, vector(position), quenturnian(rotation) {scale is also vector3}   
//	}
	Vector3 movement;
	// needed for physics, moves player, rotates player every pyhsics step
	void FixedUpdate(){	
		// speed up rotation on touch
		//		Touch touch = Input.touches[0];
		//		if (Input.touchCount == 1 && touch.phase == TouchPhase.Began) {
		//			rotateSpeed *= 10;
		//		}
		//
		//		if (Input.touchCount == 1 && touch.phase == TouchPhase.Ended) {
		//			rotateSpeed /= 10;
		//		}


		// moves player by with keyboard or gyroscope
		//if (gameController.desktop) {
		//float h = Input.GetAxis ("Horizontal");
		//float v = Input.GetAxis ("Vertical");
		//move (h, v);
			//movement = new Vector2 ( Input.GetAxis ("Horizontal"), 0.0f);	


		//}
		//else{

		// phone
		float phoneInput = Input.acceleration.x;

		// pcs
		float pcInput = Input.GetAxis ("Horizontal");

		float xInput = phoneInput + pcInput;

		// .30 is tilt, changing to .4 to add more tilt
		if(xInput < -0.4){
			xInput = -0.4f;
		}
		else if(xInput > 0.4){
			xInput = 0.4f;
		}

		movement = new Vector2 (xInput, 0.0f);	// , 0.0f
	//	}

		// test accelleration 
		//txtTest.text = Input.acceleration.x.ToString ();

		// multiply movement by speed and add to velocity (moves player)
		rigidbody2D.velocity = movement * speed * initPlayerSpeed;
		//rigidbody2D.

		// keeps the player clamped within the x co-ordinates
		//rigidbody2D.position = new Vector2 (Mathf.Clamp (rigidbody2D.position.x, boundary.xMin, boundary.xMax),0.0f);

		// rotates the player by the rotate speed in the chosen direction, then by delta time to make it more smooth and realistic
		transform.Rotate (Vector3.forward * Time.deltaTime * rotateSpeed * spinDirection);
	}

	void move(float h, float v){
		movement.Set (h, 0f, v);

		movement = movement.normalized * speed * Time.deltaTime;

		rigidbody2D.MovePosition (transform.position + movement);
	}
}