//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;
//
////using GooglePlayGames;
//using UnityEngine.SocialPlatforms;

// google plugin for unity
// https://developers.google.com/games/services/integration/




//using UnityEngine;
//using System.Collections;
using UnityEngine.UI;
//using System;

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using GooglePlayGames;
//using GooglePlayGames.BasicApi.SavedGame;
//using System;
//using GooglePlayGames.BasicApi;

using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

using GooglePlayGames;
//using UnityEngine.SocialPlatforms;

public class SocialScript : MonoBehaviour {
	private int lastScore;// last
	private int highScore;
	// used to not post same score twice (as if a user logs in with a different account, they will not post score
	private int updateScore;
	//private int totalScore;


	public GameObject txtSocial;
	public Text txtTest, txtTest2;


	public GameObject signinButton;
	//public GameObject signinMessage;
	public GameObject achievementButton;
	public GameObject highScoreButton;


	//public Image imgPlay, imgDefault;

	//public Image imgUser;

	//public Sprite imgSprite;
	//public GameObject imgQuad;
	//private const float FontSizeMult = 0.05f;
	//private bool mWaitingForAuth = false;
	//private string mStatusText = "Ready.";
	
//	static bool sAutoAuthenticate = true;
	void Awake(){

		//DontDestroyOnLoad (this.gameObject);

	}
	
//	void OnEnable(){
//		OnSignIn ();
//	}

	// Use this for initialization
	void Start () {

		//DontDestroyOnLoad (this);

		//score = PlayerPrefs.GetInt ("lastScore");
		//highScore = PlayerPrefs.GetInt ("highScore");
		//totalScore = PlayerPrefs.GetInt ("totalScore");
		//txtTest.text = PlayerPrefs.GetInt ("totalScore").ToString();

		// Select the Google Play Games platform as our social platform implementation
		GooglePlayGames.PlayGamesPlatform.Activate();
		StartCoroutine (waitFunc(2f));

		// sign user in once menu is shown:
		//OnSignIn ();
		//StartCoroutine (waitFunc(2f));

		//OnSignIn ();



		//checkpoints ();
		//incremental ();
		reportDetails ();

		// checkouts lastScore for achievements rather then highscore
		lastScore = PlayerPrefs.GetInt ("lastScore");
		updateScore = PlayerPrefs.GetInt ("updateScore");

		if(Authenticated){
			// makes sure a new user doesn't use previous users scores ( also makes sure increments are not added twice)
			if (updateScore != lastScore){
				PlayerPrefs.SetInt("updateScore", lastScore);

				leaderboard ();
				checkpoints ();
				incremental ();
			}
		}
	}

	void reportDetails(){
		StartCoroutine (waitFunc(2f));
		OnSignIn ();
		StartCoroutine (waitFunc(2f));
		// if authenticated show leaderboard and achievement buttons
		if (Authenticated) {
			highScoreButton.SetActive(true);
			achievementButton.SetActive(true);
			signinButton.SetActive(false);
			txtSocial.SetActive(false);
		}

//		if(Authenticated){
//			//txtTest.text = "authenticated okay ya m8";
//			leaderboard ();
//			checkpoints ();
//			//GetTotal();
//		}
	}

	IEnumerator waitFunc(float time){
		yield return new WaitForSeconds(time);
		reportDetails ();
	}

	public void leaderboard(){
		int highScore2 = PlayerPrefs.GetInt ("highScore");
		int totalScore2 = PlayerPrefs.GetInt ("totalScore");
		//txtTest.text = totalScore2.ToString ();
		Debug.Log ("NB: retrieving leaderboards");
		// Post highest score to scoreboard (google manage checking highest)
		Social.ReportScore(highScore2, "CgkIjoqE3s8VEAIQAQ", (bool success) => {
			// handle success or failure
		});

		// total dodged
		Social.ReportScore(totalScore2, "CgkIjoqE3s8VEAIQDg", (bool success) => {
			// handle success or failure
		});

	}

	// trying to retrieve data
//	public void GetTotal(){
//		int dlTotal;
//		txtTest.text = "before1";
//		txtTest2.text = "before2";
//		Social.LoadScores("CgkIjoqE3s8VEAIQDg", scores => {
//			if (scores.Length > 0) {
//				txtTest.text = ("Got " + scores.Length + " scores");
//				Debug.Log ("Got " + scores.Length + " scores");
//				string myScores = "Leaderboard:\n";
//				foreach (IScore score in scores)
//					myScores += "\t" + score.userID + " " + score.formattedValue + " " + score.date + "\n";
//				Debug.Log (myScores);
//			}
//			else{
//				Debug.Log ("No scores loaded");
//				txtTest2.text = "tears";
//			}
//		}); 
//	
//		Social.lo
//		System.Action[IUserProfile] callback
//	}




//	public void incremental(){
//		score = PlayerPrefs.GetInt ("lastScore");
//
//		// incremental (500 for rookie)
//		PlayGamesPlatform.Instance.IncrementAchievement(
//			"CgkIjoqE3s8VEAIQDQ", score, (bool success) => {
//			// handle success or failure
//		});
//	}

	public void incremental(){
		// incremental (500 for rookie)
		PlayGamesPlatform.Instance.IncrementAchievement(
			"CgkIjoqE3s8VEAIQDQ",  lastScore, (bool success) => {
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
			"CgkIjoqE3s8VEAIQDw",  lastScore, (bool success) => {
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


	// Update is called once per frame
	void Update () {

//		if (mWaitingForAuth) {
//			Debug.Log ("mWaitingForAuth: return now...");
//			return;
//		}


		if (Social.localUser.authenticated) {
			Debug.Log ("Social.localUser.authenticated");

			//signinButton.GetComponentInChildren<Text>().text = "Sign Out";
			signinButton.SetActive(false);
		} else {
			//Debug.Log ("!Social.localUser.authenticated");

			//signinButton.GetComponentInChildren<Text>().text = "Sign In";
			signinButton.SetActive(true);

			//buttonLabel = "Authenticate";
			//mStatusText = "Ready";
		}

	}		

	public void OnSignIn() {
//		if(Authenticating) {
//			return;
//		}
//		//Beep();
//		
//		if (Authenticated) {
//			//Beep ();
//			//GameManager.Instance.SignOut();
//			((PlayGamesPlatform) Social.Active).SignOut();
//		} else {
//			Authenticate();
//		}

		if (!Social.localUser.authenticated) {
			Debug.Log ("!Social.localUser.authenticated");
			// Authenticate
		//	mWaitingForAuth = true;
			//mStatusText = "Authenticating...";
			Social.localUser.Authenticate((bool success) => {
				//mWaitingForAuth = false;
				if (success) {
					Debug.Log ("success");
					//txtTest.text = "success";
					highScoreButton.SetActive(true);
					achievementButton.SetActive(true);
					signinButton.SetActive(false);
					txtSocial.SetActive(false);
					//mStatusText = "Welcome " + Social.localUser.userName;
					//GetTotal();
				} else {
					Debug.Log ("failure");
					//txtTest.text = "failure";
					//mStatusText = "Authentication failed.";
				}
			});
		}/* else {
			Debug.Log ("Social.localUser.authenticated");
			// Sign out!
			//mStatusText = "Signing out.";
			((GooglePlayGames.PlayGamesPlatform) Social.Active).SignOut();

			highScoreButton.SetActive(false);
			achievementButton.SetActive(false);
			signinButton.SetActive(true);
			txtSocial.SetActive(true);
		}*/

	}
	


	public void OnAchievements() {
		//Beep();
		//GameManager.Instance.ShowAchievementsUI();

		if (Authenticated) {
			Social.ShowAchievementsUI();
		}

	}
	
	public void OnHighScores() {
		//Beep();
		//GameManager.Instance.ShowLeaderboardUI();
		if (Authenticated) {
			Social.ShowLeaderboardUI();
		}
	}

	public bool Authenticated {
		get {
			return Social.Active.localUser.authenticated;
		}
	}

	public void checkpoints (){


			// legendary
			if(lastScore >= 350){
				// unlock achievement (achievement ID "Cfjewijawiu_QA")
				Social.ReportProgress("CgkIjoqE3s8VEAIQDA", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// guru
			if(lastScore >= 300){
				Social.ReportProgress("CgkIjoqE3s8VEAIQCw", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// grumpy cats
			if(lastScore >= 250){
				Social.ReportProgress("CgkIjoqE3s8VEAIQCg", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// coconuts
			if(lastScore >= 200){
				Social.ReportProgress("CgkIjoqE3s8VEAIQCQ", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// bananas
			if(lastScore >= 150){
				Social.ReportProgress("CgkIjoqE3s8VEAIQCA", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// chaos
			if(lastScore >= 100){
				Social.ReportProgress("CgkIjoqE3s8VEAIQBw", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// insanity
			if(lastScore >= 75){
				Social.ReportProgress("CgkIjoqE3s8VEAIQBg", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// wat
			if(lastScore >= 50){
				Social.ReportProgress("CgkIjoqE3s8VEAIQBQ", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// cray cray
			if(lastScore >= 20){
				Social.ReportProgress("CgkIjoqE3s8VEAIQBA", 100.0f, (bool success) => {
					// handle success or failure
				});
			}
			
			// wow
			if(lastScore >= 10){
				Social.ReportProgress("CgkIjoqE3s8VEAIQAg", 100.0f, (bool success) => {
					// handle success or failure
				});
			}

	}
	//
	//
	//	// game manager
	//	private bool mAuthenticating = false;
	//
	//	public bool Authenticating {
	//		get {
	//			return mAuthenticating;
	//		}
	//	}
	//
//
//
//
//	public void Authenticate() {
//		if (Authenticated || mAuthenticating) {
//			Debug.LogWarning("Ignoring repeated call to Authenticate().");
//			return;
//		}
//		
//		// Enable/disable logs on the PlayGamesPlatform
//		PlayGamesPlatform.DebugLogEnabled = GameConsts.PlayGamesDebugLogsEnabled;
//		
//		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
//			.EnableSavedGames()
//				.Build();
//		PlayGamesPlatform.InitializeInstance(config);
//		
//		// Activate the Play Games platform. This will make it the default
//		// implementation of Social.Active
//		PlayGamesPlatform.Activate();
//		
//		// Set the default leaderboard for the leaderboards UI
//		((PlayGamesPlatform) Social.Active).SetDefaultLeaderboardForUI(GameIds.LeaderboardId);
//		
//		// Sign in to Google Play Games
//		mAuthenticating = true;
//
//
//		Social.localUser.Authenticate((bool success) => {
//			mAuthenticating = false;
//			if (success) {
//				// if we signed in successfully, load data from cloud
//				Debug.Log("Login successful!");
//			} else {
//				// no need to show error message (error messages are shown automatically
//				// by plugin)
//				Debug.LogWarning("Failed to sign in with Google Play Games.");
//			}
//		});
//	}

}