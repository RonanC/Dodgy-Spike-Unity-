using UnityEngine;
using System.Collections;
// to use UI elements
using UnityEngine.UI;
// social
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

// phone is 480 x 800
// reduced by 70% for browser to: 336 x 560 (reduced this in html page)

// this is the main class for the play scene
// it constantly spawns the walls and spikes 
// it determines if the game is over
// determines if checkpoints are reached

public class GameController : MonoBehaviour {
	// social




	// wait 3 seconds initially between spike spawning (decreases as spike speed increases)
	private float spawnWait = 1; // 3
	// wait 1 second before showing game over text
	private float gameOverWait = 1f;
	// sets intial side to left (switches each time)
	private int side = 1;
	// add an initial speed to the spike
	private int initialSpeed = 8; // 4
	// speed of each spike increases each time
	//[HideInInspector]
	private float waveSpeed = 0;
	// a mix of wavespeed and initial speed
	private float speed; // max speed 17

	// game over flag, if true then end game
	[HideInInspector]
	public bool gameOver;

	//private bool restart;



	// checks if using windows phone or desktop
	[HideInInspector]
	public bool desktop;

	// wave number (used for achivements
	private int waveNum = 0;

	// spike positions
	private float[] spikePos = {-3.0f, -2.5f, -2.05f};
	// spike array
	private GameObject[] spikeArray = new GameObject[3];
	// spike to be instantiated
	private GameObject spikeRand;

	// reference to player object
	private PlayerController playerController;
	//private SocialScript socialScript;

	// for bonus text (locks the bonus method)
	private bool bonusOpen = true;
	private int bonusLevel;

	public string [] bonusWords;

	// walls
	public GameObject wallL, wallR;
	// spikes
	public GameObject spike0, spike1, spike2;

	// canvas elements
	public Text txtScore;
	public Text txtRestart;
	public Text txtGameOver;
	public Text txtBonus;
	public Text txtTilt, txtTest;

	// local storage
	private int highScore;
	// total score ever!
	private int totalScore;
	private int score;

	// speed up game for android
	//float gameSpeedMult = 10f;


	private bool deviceAndroid;

	// set variables & objects, load form local storage and start coroutines
	void Start (){


		// Select the Google Play Games platform as our social platform implementation
		//GooglePlayGames.PlayGamesPlatform.Activate();


		// Authenticate (sign in)
		//SignIn ();
//
//				// incremental (500 for rookie)
//				PlayGamesPlatform.Instance.IncrementAchievement(
//					"CgkIjoqE3s8VEAIQDg", 4, (bool success) => {
//			if(success){
//				txtTest.text = "incremented";
//			}
//			else{
//				txtTest.text = "not inc :(";
//			}
//				});

//		PlayGamesPlatform.Instance.IncrementAchievement(
		//			"CgkIjoqE3s8VEAIQDg",  4, (bool success) => {
//			Debug.Log("Incremental");
//			txtTest.text = "incremented";
//			// handle success or failure
//		});

		// keep screen from dimming
		Screen.sleepTimeout = SleepTimeout.NeverSleep;

		// retrieve scores from local storage
		highScore = PlayerPrefs.GetInt("highScore");
		totalScore = PlayerPrefs.GetInt ("totalScore");

		txtTest.text = totalScore.ToString ();

		// retrieve playerObject
		GameObject playerControllerObject = GameObject.FindWithTag ("Player");
		if (playerControllerObject != null){
			playerController = playerControllerObject.GetComponent <PlayerController>();
		}
		if (playerController == null){
			Debug.Log ("Cannot find 'PlayerController' script");
		}


		// retrieve socialObject
//		GameObject socialScriptObject = GameObject.FindWithTag ("Social");
//		if (socialScriptObject != null){
//			socialScript = socialScriptObject.GetComponent <SocialScript>();
//		}
//		if (socialScript == null){
//			Debug.Log ("Cannot find 'SocialScript' script");
//		}

		// add spikes to spike array
		spikeArray [0] = (GameObject)spike0;
		spikeArray [1] = (GameObject)spike1;
		spikeArray [2] = (GameObject)spike2;

		// determine if playing on a windows phone or other (desktop)
		if (Application.platform == RuntimePlatform.WP8Player) { // RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXWebPlayer
			desktop = false;
			Debug.Log("Desktop is false");
		}
		else if(Application.platform == RuntimePlatform.Android){
			deviceAndroid = true;
		}
		else{
			desktop = true;
			Debug.Log("Desktop is true");
		}

		// set variables to blank
		gameOver = false;
		//restart = false;
		txtRestart.text = "";
		txtGameOver.text = "";
		txtBonus.text = "";
		txtTilt.text = "";
		bonusOpen = true;

		//waveWait = 0;
		score = 0;
		bonusLevel = 0;

		// set score
		UpdateScore ();




		// start coroutines (walls & waves)
		StartCoroutine(SpawnWalls ());
		StartCoroutine (SpawnWaves ());
	}

	IEnumerator firstGameRoutine(){
		// flash the text three times
		for (int t = 0; t < 3; t++) {
			txtTilt.text = "<- Tilt ->";
			yield return new WaitForSeconds (1 / 1.8f);
			txtTilt.text = "";
			yield return new WaitForSeconds (1 / 1.8f);
		}
		
//		// dodge the walls and spikes
//		for (int t = 0; t < 1; t++) {
//			txtTilt.text = "Spikes & Walls\nare not your friends";
//			yield return new WaitForSeconds (1 / 1.8f);
//			txtTilt.text = "";
//			yield return new WaitForSeconds (1 / 1.8f);
//		}	
	}

	void Update() {
		// start wave bonus coroutine (flashing text)
		StartCoroutine(WaveBonus());
	}


	// add given score value to the score
	public void AddScore (int newScoreValue){
		score += newScoreValue;
		UpdateScore ();
	}

	// show new score in text field
	void UpdateScore (){
		txtScore.text = score.ToString();


//		PlayerPrefs.SetInt("lastScore", score);
//		PlayerPrefs.SetInt ("totalScore", score + totalScore);
		//	socialScript.checkpoints ();
		//socialScript.incremental ();
	}

	// set game over to true and start wait timer
	public void  GameOver(){
		gameOver = true;
		StartCoroutine(GameOverWait ());
	}

	// update scores, show game over text, wait and load menu scene
	IEnumerator GameOverWait(){
		int tempScore = totalScore + score;
		// set last score value to local storage
		PlayerPrefs.SetInt("lastScore", score);
		PlayerPrefs.SetInt ("totalScore", tempScore); //totalScore + score
		PlayerPrefs.SetInt ("firstGame", 1);


//		Debug.Log("before if authenticated");
//		if (Authenticated) {
//			Debug.Log("inside if authenticated");
//			incremental();
//
//		}
//		Debug.Log("after if authenticated");
		// if new high score, set new value to variable and local storage
		if(score > highScore){
			highScore = score;
			PlayerPrefs.SetInt("highScore", highScore);
		}  

		// wait for given amount of time
		yield return new WaitForSeconds (gameOverWait);

		// show game over text
		txtGameOver.text = "Game\nOver";

		// wait for 2 seconds
		yield return new WaitForSeconds (1);

		// load menu scene
		Application.LoadLevel ("Menu");
	}

	// checks if gameover, chooses random spike, sets side, sets wait time and spawn speeds and instantiates spike
	IEnumerator SpawnWaves (){
		// first game display tilt instructions
		if(PlayerPrefs.GetInt("firstGame") == 0){
			//StartCoroutine(firstGameRoutine());
			for (int t = 0; t < 2; t++) {
				txtTilt.text = "<- Tilt ->";
				yield return new WaitForSeconds (1 / 1.8f);
				txtTilt.text = "";
				yield return new WaitForSeconds (1 / 1.8f);
			}
		}
		//int lastSpike = 0;

		// keep spawning waves of spikes and walls
		while (true) {
			// while the player has not crashed
			if(gameOver == false){
				// increase wave number (use for achievements)
				waveNum++;
			
				//for (int i = 0; i < spikeCount; i++) {
				
				// opens the bonus flag
				//bonusOpen = true;


				// sets the spawn side (flips each time)
				float spawnSide;
				// sets the spawn rotation
				float spawnRotation;

				// chooses a random spike out of 3
				int randNum;
				if(score <= 5){
					// normal spikes until score is 5
					randNum = 0;
				}
				else {//if(score <= 20){
					// medium spikes until score is 20
					randNum = Random.Range(0,2);
				}
//				else{
//					// hard spikes occur
//					randNum = Random.Range(0,3);
//				}
//				// makes sure hard spikes don't generate twice in a row
//				if(lastSpike == 2 && randNum == 2){
//					randNum = Random.Range(0,2);
//				}
				// sets last spike (used for above if method)
				//lastSpike = randNum;
				// sets the spike
				spikeRand = spikeArray[randNum];	//Mathf.Round	
				
				// sets the side
				if (side == 1) {
					spawnSide = spikePos[randNum];
					spawnRotation = 0.0f;
				} else {
					spawnSide = spikePos[randNum] * -1.0f;
					spawnRotation = 180f;
				}

				int randSide = Random.Range(0,2);

				if(randSide == 0){
					side = -1;
				}else{
					side = 1;
				}

				// side *= -1;
				// spawns the spike
				Vector2 spawnPosition = new Vector2 (spawnSide, 17);
				GameObject clone;
				clone = (GameObject)Instantiate (spikeRand, spawnPosition, Quaternion.Euler (0, spawnRotation, 0));

				// IMPORTANT
				// spawns the spike in relation to its speed
				//if(spawnWait >= -0.4){ //0.60

				if(score < 50){

				//if(playerController.rotateSpeed < 60f){
					waveSpeed += 0.01f;
					speed = waveSpeed + initialSpeed;
					

					// when spawn wait == 0.52 it is impossible (reached 48 POINTS), WAVE SPEED 10
					spawnWait -= 0.01f; //-= 0.2f;
				
					if(score < 10){
						playerController.rotateSpeed *= 1.05f;
					}
					else if(score < 20){
						playerController.rotateSpeed *= 1.025f;
					}
				//}
				}

				// sets the speed of the spike
				clone.rigidbody2D.velocity = new Vector2 (0, -1 * speed * 2);

				// speeds player up each spike, also capped at 20 in waveBonus script
				if (playerController.speed < 3) {
					playerController.speed *= 1.008f; //1.1
				}
				bonusOpen = true;
			} // if gameover
				
			// wait for 3 seconds initially between spikes, then reduced as spikes speed increases
			yield return new WaitForSeconds (spawnWait);
			//} // for
		} // while
	} // IEnumerator

	// spawns the walls
	IEnumerator SpawnWalls(){
		// left
		Vector2 initWallSpawnPosL = new Vector2 (-5.5f, 10f);
		GameObject initWallLeft = (GameObject)Instantiate (wallL, initWallSpawnPosL, Quaternion.Euler (0, 0, 0));
		initWallLeft.rigidbody2D.velocity = new Vector2 (0, -1 * 4);
		initWallLeft.transform.localScale = new Vector3(5, 7);	//transform.localScale.y
		
		// right
		Vector2 initWallSpawnPosR = new Vector2 (5.5f, 10f);
		GameObject initWallRight = (GameObject)Instantiate (wallR, initWallSpawnPosR, Quaternion.Euler (0, 180, 0));
		initWallRight.rigidbody2D.velocity = new Vector2 (0, -1 * 4);
		initWallRight.transform.localScale = new Vector3(5, 7);	//transform.localScale.y

		while(true){
			// left wall
			Vector2 wallSpawnPosL = new Vector2 (-5.5f, 30f);
			GameObject wallLeft = (GameObject)Instantiate (wallL, wallSpawnPosL, Quaternion.Euler (0, 0, 0));
			wallLeft.rigidbody2D.velocity = new Vector2 (0, -1 * 4);
			
			// right wall
			Vector2 wallSpawnPosR = new Vector2 (5.5f, 30f);
			GameObject wallRight = (GameObject)Instantiate (wallR, wallSpawnPosR, Quaternion.Euler (0, 180, 0));
			wallRight.rigidbody2D.velocity = new Vector2 (0, -1 * 4);
			
			yield return new WaitForSeconds (5);
		}
	}

	// when a ceratin score is achieved this method displays text (called on update)
	IEnumerator WaveBonus(){
		// level waves 20/50/100/200/300,400,500...




		// if bonus flag is open then check
		if (bonusOpen == true) {
			// turn flag off (read lock)
			bonusOpen = false;

			if(score == highScore+1){
				// flash
				//for (int t = 0; t < 1; t++) {
					txtBonus.text = "High Score";
					yield return new WaitForSeconds (1 / 1.8f);
					txtBonus.text = "";
					yield return new WaitForSeconds (1 / 1.8f);
				//}	
			}

			if (score == 10 || score == 20 || score == 75 || (score > 0 && score % 50 == 0)) {	//|| score == 50
				// no more checking at checkpoints (10 if statements, causes performance hit)
//				if (Authenticated) {
//					checkpoints();
//				}

				// increase the speed a bit
				//waveSpeed += 0.1f;
				//speed = waveSpeed + initialSpeed;
				//spawnWait -= 0.01f;
				
				// increase the rotation a bit and switch rotate direction
				//playerController.rotateSpeed *= 1.1f;
				//playerController.spinDirection *= -1;
				//playerController.speed *= 1.1f;

				// flash the text once
				//for (int t = 0; t < 1; t++) {
					if(bonusLevel < 9){
						txtBonus.text = bonusWords[bonusLevel];	//New Sprite\nUnlocked!
					}
					else{
						txtBonus.text = bonusWords[9];
					}

					yield return new WaitForSeconds (1 / 1.8f);
					txtBonus.text = "";
					yield return new WaitForSeconds (1 / 1.8f);
				//}
				bonusLevel++;

			}

			// called when equal to or over 200 points, every time 100 points is scored 
//			} else if ((score % 100) == 0 && score >= 200) {
//				// flash  text twice
//				for (int t = 0; t < 2; t++) {
//						txtBonus.text = "Chaos!";
//						yield return new WaitForSeconds (1 / 2.15f);
//						txtBonus.text = "";
//						yield return new WaitForSeconds (1 / 2.15f);
//				}
//				
//				// increase speed
//				waveSpeed += 0.1f;
//				speed = waveSpeed + initialSpeed;
//				spawnWait -= 0.01f;
//				
//				// set rotation speed and direction
//				playerController.rotateSpeed *= 1.1f;
//				playerController.spinDirection *= -1;
//				playerController.speed *= 1.1f;
//			} // if score 
		} // if bonusOpen

		// caps the player speed at 17
		if (playerController.speed >= 3) {
			playerController.speed = 3;
		}
	}	// method






	private int isMuted = 0;
	
	
	void OnApplicationPause(bool pauseStatus) {
		//txtTest.text = paused.ToString ();
		
		isMuted = PlayerPrefs.GetInt ("isMuted");
		
		if(isMuted == 1){
			AudioListener.pause = true;
		}
		else{
			AudioListener.pause = false;
		}
	}





	// social


	public bool Authenticated {
		get {
			Debug.Log("Authenticated");
			return Social.localUser.authenticated;	//Active.localUser.

		}
	}

	public void incremental(){
		// incremental (500 for rookie)
		PlayGamesPlatform.Instance.IncrementAchievement(
			"CgkIjoqE3s8VEAIQDQ",  score, (bool success) => {
			Debug.Log("Incremental");
			if(success){
			//txtTest.text = "incremented";
			}
			else{
				//txtTest.text = "not inc :(";
			}
			// handle success or failure
		});

		// incremental (500 for rookie)
		PlayGamesPlatform.Instance.IncrementAchievement(
			"CgkIjoqE3s8VEAIQDw",  score, (bool success) => {
			Debug.Log("Incremental");
			if(success){
				//txtTest.text = "incremented";
			}
			else{
				//txtTest.text = "not inc :(";
			}
			// handle success or failure
		});
	}
	
	public void checkpoints (){
		//score = PlayerPrefs.GetInt ("lastScore");
		
		// legendary
		if(score >= 350){
			// unlock achievement (achievement ID "Cfjewijawiu_QA")
			Social.ReportProgress("CgkIjoqE3s8VEAIQDA", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// guru
		if(score >= 300){
			Social.ReportProgress("CgkIjoqE3s8VEAIQCw", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// grumpy cats
		if(score >= 250){
			Social.ReportProgress("CgkIjoqE3s8VEAIQCg", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// coconuts
		if(score >= 200){
			Social.ReportProgress("CgkIjoqE3s8VEAIQCQ", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// bananas
		if(score >= 150){
			Social.ReportProgress("CgkIjoqE3s8VEAIQCA", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// chaos
		if(score >= 100){
			Social.ReportProgress("CgkIjoqE3s8VEAIQBw", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// insanity
		if(score >= 75){
			Social.ReportProgress("CgkIjoqE3s8VEAIQBg", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// wat
		if(score >= 50){
			Social.ReportProgress("CgkIjoqE3s8VEAIQBQ", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// cray cray
		if(score >= 20){
			Social.ReportProgress("CgkIjoqE3s8VEAIQBA", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
		
		// wow
		if(score >= 10){
			Social.ReportProgress("CgkIjoqE3s8VEAIQAg", 100.0f, (bool success) => {
				// handle success or failure
			});
		}
	}

	void SignIn(){
		if (!Social.localUser.authenticated) {
			Debug.Log ("if user is not signed in, then try to authenticate");
			// Authenticate
			Social.Active.localUser.Authenticate((bool success) => {
				if (success) {
					Debug.Log ("success");
					txtTest.text = "success";
				} else {
					Debug.Log ("failure");
					txtTest.text = "failure";
				}
			});
		}
	}

} // class
