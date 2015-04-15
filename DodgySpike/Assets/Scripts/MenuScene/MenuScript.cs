using UnityEngine;
using System.Collections;
using UnityEngine.UI;
// in order to use android social game centre features
using UnityEngine.SocialPlatforms;

// manages the menu buttons and music
// navigating to: play, character select
// toggling sound on/off
// tweeting your score to your followers

public class MenuScript : MonoBehaviour {
	//public GameObject BGMusic;
	public Text txtBestScore, txtLastScore;//, txtTest;
	public Button btnMute, btnMusic;

	private Color32 normCol = new Color32(0, 144, 144, 255);
	private Color32 selCol = new Color32(5, 80, 0, 255);			//(50, 200, 0, 173);

	public GameObject ctrlSocial;


	// toggle if first game
	public GameObject btnTwitter, btnFacebook, txtShare, txtSocial;
	
	// local storage
	int highScore, lastScore;
	bool firstGame;



	//public Text txtTest;
	private int isMuted = 0, isMusicMuted = 0;

	void Update(){
		// enable back button to quit
		if (Input.GetKeyDown(KeyCode.Escape)) 
			Application.Quit(); 
	}

	void OnApplicationPause(bool pauseStatus) {
		//txtTest.text = paused.ToString ();

		// is all muted
		isMuted = PlayerPrefs.GetInt ("isMuted");
		if(isMuted == 1){
			btnMute.image.color = selCol;
			AudioListener.pause = true;
		}
		else{
			btnMute.image.color = normCol;
			AudioListener.pause = false;
		}

		// is music muted
		isMusicMuted = PlayerPrefs.GetInt ("isMusicMuted");
		if(isMusicMuted == 1){
			btnMusic.image.color = selCol;
			//AudioListener.pause = true;
			PlayerPrefs.SetInt("isMusicMuted", 1);
		}
		else{
			btnMusic.image.color = normCol;
			//AudioListener.pause = false;
			PlayerPrefs.SetInt("isMusicMuted", 0);
		}
	}
	
	public void Start(){

		//txtTest.text = PlayerPrefs.GetInt ("firstGame").ToString();

		// load storage settings
		if(PlayerPrefs.GetInt ("firstGame") == 0){
			firstGame = true;
		}
		highScore = PlayerPrefs.GetInt("highScore");
		lastScore = PlayerPrefs.GetInt("lastScore");
		// display scores
		txtBestScore.text = highScore.ToString();
		txtLastScore.text = lastScore.ToString();

		// check if muted
		if (AudioListener.pause == false) {
			btnMute.image.color = normCol;
//			btnMute.GetComponentInChildren<Text>().text = "Mute";
		}
		else{
			btnMute.image.color = selCol;
	//		btnMute.GetComponentInChildren<Text>().text = "Unmute";
		}

		// check if Muisc muted
		if (PlayerPrefs.GetInt("isMusicMuted") == 0) {
			btnMusic.image.color = normCol;
			//			btnMute.GetComponentInChildren<Text>().text = "Mute";
		}
		else{
			btnMusic.image.color = selCol;
			//		btnMute.GetComponentInChildren<Text>().text = "Unmute";
		}


		// remove share, add log in
		if(firstGame){
			txtSocial.SetActive(true);
			txtShare.SetActive(false);
			btnFacebook.SetActive(false);
			btnTwitter.SetActive(false);
		}
		else{
			txtSocial.SetActive(false);
			txtShare.SetActive(true);
			btnFacebook.SetActive(true);
			btnTwitter.SetActive(true);
		}

	}

	public void ChangeScene(string chosenScene){
		//DontDestroyOnLoad (BGMusic);
		Application.LoadLevel(chosenScene);
	}

	public void ToggleMute(){
		if (AudioListener.pause == false) {
		//	btnMute.GetComponentInChildren<Text>().text = "Unmute";
			btnMute.image.color = selCol;

			AudioListener.pause = true;

			isMuted = 1;
			PlayerPrefs.SetInt("isMuted", isMuted);
		}
		else{
		//	btnMute.GetComponentInChildren<Text>().text = "Mute";
			btnMute.image.color = normCol;

			AudioListener.pause = false;

			isMuted = 0;
			PlayerPrefs.SetInt("isMuted", isMuted);
		}
	}

	// toggle music mute
	public void ToggleMuteMusic(){
		if (PlayerPrefs.GetInt("isMusicMuted") == 0) {
			//	btnMute.GetComponentInChildren<Text>().text = "Unmute";
			btnMusic.image.color = selCol;
			
			//AudioListener.pause = true;

			isMusicMuted = 1;
			PlayerPrefs.SetInt("isMusicMuted", isMusicMuted);
		}
		else{
			//	btnMute.GetComponentInChildren<Text>().text = "Mute";
			btnMusic.image.color = normCol;
			
			//AudioListener.pause = false; 
			
			isMusicMuted = 0;
			PlayerPrefs.SetInt("isMusicMuted", isMusicMuted);
		}
	}

	// fb share
	public void fbShare(){
		string description;
		if (lastScore == 1) {
			description = "I just dodged " + lastScore + " spike!";
		}
		else{
			description = "I just dodged " + lastScore + " spikes!";
		}

		// message info
		string link = "http://ronanconnolly.ie/unity/dodgyspike/game.html";
		string pictureLink = "http://ronanconnolly.ie/unity/dodgyspike/image.png";
		string name = "Dodgy Spike";
		string caption = "Best Score: " + highScore;
		//string description = "I just dodged " + lastScore + " spikes!\nJust try and beat that!";
		//string description = "Description";

		//string redirectUri = "unity/dodgyspike/image.png";
		string redirectUri = "http://www.facebook.com/";

		//ShareFB(link, pictureLink, name, caption, description, redirectUri);
		ShareToFacebook(link, name, caption, description, pictureLink, redirectUri);
	}

	// facebook info
	const string AppId = "1406554872982202";
	const string ShareUrl = "http://www.facebook.com/dialog/feed";

	// facebook share
	void ShareToFacebook (string linkParameter, string nameParameter, string captionParameter, string descriptionParameter, string pictureParameter, string redirectParameter)
	{
		Application.OpenURL (ShareUrl + "?app_id=" + AppId +
		                     "&link=" + WWW.EscapeURL(linkParameter) +
		                     "&name=" + WWW.EscapeURL(nameParameter) +
		                     "&caption=" + WWW.EscapeURL(captionParameter) + 
		                     "&description=" + WWW.EscapeURL(descriptionParameter) + 
		                     "&picture=" + WWW.EscapeURL(pictureParameter) + 
		                     "&redirect_uri=" + WWW.EscapeURL(redirectParameter));
	}

	// twitter share
	public void twitterShare(){
		// message info
		string text;
		if (lastScore == 1) {
			text = "I just dodged " + lastScore + " spike!\n\n#DodgySpike #GamesFleadh\n@RonanDConnolly\n\nTry to beat me:\nhttp://goo.gl/Px0b5Y";
		}
		else{
			text = "I just dodged " + lastScore + " spikes!\n\n#DodgySpike #GamesFleadh\n@RonanDConnolly\n\nTry to beat me:\nhttp://goo.gl/Px0b5Y";
		}
		string url = "http://ronanconnolly.ie/unity/dodgyspike/game.html";
		string related = "@RonanDConnolly";
		string lang = "en";

		ShareTW(text, url, related, lang);
	}

	// twitter info
	const string Address = "http://twitter.com/intent/tweet";

	// share to twitter method
	public static void ShareTW(string text, string url,string related, string lang)//="en"
	{
		Application.OpenURL(Address +
		                    "?text=" + WWW.EscapeURL(text) +
		                    "&amp;url=" + WWW.EscapeURL(url) +
		                    "&amp;related=" + WWW.EscapeURL(related) +
		                    "&amp;lang=" + WWW.EscapeURL(lang));
	}

	public void SignIn(){
		ctrlSocial.SetActive(true);
	}
}
