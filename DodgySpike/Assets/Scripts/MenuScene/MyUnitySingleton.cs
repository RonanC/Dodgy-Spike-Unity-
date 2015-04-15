using UnityEngine;
using System.Collections;

// makes sure only one instance of this game object exists

public class MyUnitySingleton : MonoBehaviour {

	// this script makes sure only one instance of BG music is allowed
	// http://answers.unity3d.com/questions/11314/audio-or-music-to-continue-playing-between-scene-c.html




	private static MyUnitySingleton instance = null;

	public static MyUnitySingleton Instance {
		get { return instance; }
	}

	void Awake() {

		if (instance != null && instance != this) {
			Destroy(this.gameObject);
			return;
		} else {
			instance = this;
		}
		DontDestroyOnLoad(this.gameObject);
	}

	// checks if music is muted
	void Update(){
		if (PlayerPrefs.GetInt("isMusicMuted") == 1){
			audio.mute = true;
		}
		else{
			//audio.mute = false;
			audio.mute = false;
		}
	}

	void Start(){
		// SET LOCAL STORAGE:
		//PlayerPrefs.SetInt("isMusicMuted", 0);

	}
}

