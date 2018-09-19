using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Class used on the GameManager gameobject to help control
//scene flow/actions from UI elements such as buttons
public class UIDriver : MonoBehaviour {

	[Header("UI Panels")]
	public GameObject settingsPanel;
	public GameObject startPanel;
	public GameObject gameOverPanel;
	public GameObject instructionsPanel;
    public GameObject musicManager;
    public GameObject soundEffectManager;
    [Space(10)]
    [Header("Buttons")]
	public Button activeMusicButton;
	public Button activeSoundFXButton;
	public Button mutedMusicButton;
	public Button mutedSoundFXButton;
	public Button googleSignInButton;
	public Button googleSignOutButton;
    public Button pauseButton;
    public Button unpauseButton;
    [Space(10)]
    public Text scoreMultiplierText;
    public AudioSource buttonSound;

	[SerializeField]
	private string facebookURL;
	[SerializeField]
	private string twitterURL;
	private enum UI {Menu, Settings, Instructions, GameOver};//Enumerated types used with handling "Escape" input
	private UI curUI;//Enumerated types used with handling "Escape" input

    public void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			HandleEscapeInput ();
		}
	}

	public void Start(){
        if (PlayerPrefs.GetInt("SoundFX") == 0)
        {
            activeSoundFXButton.gameObject.SetActive(true);
            mutedSoundFXButton.gameObject.SetActive(false);
            soundEffectManager.SetActive(true);
        }
        if(PlayerPrefs.GetInt("SoundFX")== 1)
        {
            activeSoundFXButton.gameObject.SetActive(false);
            mutedSoundFXButton.gameObject.SetActive(true);
            soundEffectManager.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Music") == 0)
        {
            activeMusicButton.gameObject.SetActive(true);
            mutedMusicButton.gameObject.SetActive(false);
            musicManager.SetActive(true);
            if (GameObject.Find("StartPanel")!=null){
                if (GameObject.Find("StartPanel").activeInHierarchy)
                    GameObject.Find("Stars").GetComponent<AudioSource>().Play();
            }
        }
        if (PlayerPrefs.GetInt("Music") == 1)
        {
            activeMusicButton.gameObject.SetActive(false);
            mutedMusicButton.gameObject.SetActive(true);
            musicManager.SetActive(false);
        }
        //If this is the very first time playing, show instructions
        if (PlayerPrefs.GetInt("Tutorial") != 1){
			//Pop-up instructions
			ToggleInstructionsPanel();
			curUI = UI.Instructions;
			//flag that the user has seen the instructions for the first time
			PlayerPrefs.SetInt("Tutorial", 1);
			return;
		}
		curUI = UI.Menu;
	}

    public void ShowVideoAd() {
        AdDriver.Instance.ShowVideoAd();
    }

	public void ReplayGame(){
        buttonSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void LoadFacebookPage(){
		if (facebookURL == "") {
			Debug.LogError ("Missing a reference to the URL...");
		} 
		else {
			Application.OpenURL (facebookURL);
		}


	}

    public void UpdateMultiplierText(int amt) {
        if (amt <= 1)
        {
            scoreMultiplierText.text = "";
        }
        else {
            scoreMultiplierText.text = amt + "X";
        }
    }

	public void LoadTwitterPage(){
		if (twitterURL == "") {
			Debug.LogError ("Missing a reference to the URL...");
		} 
		else {
			Application.OpenURL (twitterURL);
		}
	}

	public void LoadMainMenu(){
		settingsPanel.SetActive (false);
		gameOverPanel.SetActive (false);

		Replay.isReplay = 0;
        buttonSound.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

	}

	public void ToggleSignInButtons(){
		if (googleSignInButton.IsActive ()) {
			googleSignInButton.gameObject.SetActive (false);
			googleSignOutButton.gameObject.SetActive (true);
		} else {
			googleSignInButton.gameObject.SetActive (true);
			googleSignOutButton.gameObject.SetActive (false);
		}
	}

	public void ToggleGameOverPanel(){
        if (!gameOverPanel.activeInHierarchy){
            gameOverPanel.SetActive(true);
            curUI = UI.GameOver;
        }
        else {
            gameOverPanel.SetActive(false);
            curUI = UI.Menu;
        }
		
	}

	public void ToggleSettingsPanel(){
        buttonSound.Play();
        settingsPanel.SetActive (!settingsPanel.activeSelf);
		if (settingsPanel.activeSelf) {
			curUI = UI.Settings;
		} else {
			curUI = UI.Menu;
		}
	}

	public void ToggleInstructionsPanel(){
        buttonSound.Play();
        instructionsPanel.SetActive (!instructionsPanel.activeSelf);
		if (instructionsPanel.activeSelf) {
			curUI = UI.Instructions;
		} else {
			curUI = UI.Menu;
		}
	}

	public void ToggleSoundFX(){
		if(activeSoundFXButton.IsActive()){
			
			activeSoundFXButton.gameObject.SetActive (false);
			mutedSoundFXButton.gameObject.SetActive (true);
            soundEffectManager.SetActive(false);
            PlayerPrefs.SetInt("SoundFX", 1);
        }
		else if(mutedSoundFXButton.IsActive()){
            buttonSound.Play();
            activeSoundFXButton.gameObject.SetActive (true);
			mutedSoundFXButton.gameObject.SetActive (false);
            soundEffectManager.SetActive(true);
            PlayerPrefs.SetInt("SoundFX", 0);
        }
	}

	public void ToggleMusic(){
		if(activeMusicButton.IsActive()){
			activeMusicButton.gameObject.SetActive (false);
			mutedMusicButton.gameObject.SetActive (true);
            musicManager.SetActive(false);
            PlayerPrefs.SetInt("Music", 1);
        }
		else if(mutedMusicButton.IsActive()){
            buttonSound.Play();
            activeMusicButton.gameObject.SetActive (true);
			mutedMusicButton.gameObject.SetActive (false);
            musicManager.SetActive(true);
            GameObject.Find("Stars").GetComponent<AudioSource>().Play();
            PlayerPrefs.SetInt("Music", 0);
		}
	}

    public void TogglePause() {
        buttonSound.Play();
        if (pauseButton.IsActive()) {
            pauseButton.gameObject.SetActive(false);
            unpauseButton.gameObject.SetActive(true);
            Time.timeScale = 0.0f;

            gameObject.GetComponent<GameDriver>().scoreCount.Pause();
            gameObject.GetComponent<GameDriver>().gameMusic.Pause();
            gameObject.GetComponent<GameDriver>().asteroidCapture.Pause();
            gameObject.GetComponent<GameDriver>().starCharacter.GetComponentInChildren<starJitter>().starExplosion.Pause();
            gameObject.GetComponent<GameDriver>().starCharacter.GetComponent<StarDriver>().starSwipeSound.Pause();
            StarDriver.isPaused = true;
        }
        else if (unpauseButton.IsActive()) {
            unpauseButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            Time.timeScale = 1.0f;

            gameObject.GetComponent<GameDriver>().scoreCount.UnPause();
            gameObject.GetComponent<GameDriver>().gameMusic.UnPause();
            gameObject.GetComponent<GameDriver>().asteroidCapture.UnPause();
            gameObject.GetComponent<GameDriver>().starCharacter.GetComponentInChildren<starJitter>().starExplosion.UnPause();
            gameObject.GetComponent<GameDriver>().starCharacter.GetComponent<StarDriver>().starSwipeSound.UnPause();
            StarDriver.isPaused = false;
        }
    }

    public void RateUs()
    {
        Application.OpenURL("https://play.google.com/store/apps/details?id=com.gridlinegames.starswipe");
    }

	public void HandleEscapeInput(){
		switch(curUI)
		{
		case UI.Menu:
			Application.Quit();
			break;

		case UI.Settings:
			ToggleSettingsPanel ();
			break;
		
		case UI.Instructions:
			ToggleInstructionsPanel ();
			break;

		case UI.GameOver:
			LoadMainMenu ();
			break;
		default:
			Debug.LogError ("Found a new case for handling escape key input.");
			break;
		}
	}

    public void BuyRemoveAds(){
        IAPManager.Instance.BuyRemoveAds();
    }
}
