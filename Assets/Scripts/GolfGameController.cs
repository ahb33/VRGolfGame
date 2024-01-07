using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GolfGameController : MonoBehaviour
{
    [SerializeField]
    public GolfSpawner Spawner;

    //SerializedField that takes in the XR Origin, and some UI elements.
    [SerializeField]
    public GameObject XROrigin;

    //ContinuousMovementLocomotionSystem
    [SerializeField]
    public GameObject ContinuousMovementLocomotionSystem;

    //TurnProviderLocomotion
    [SerializeField]
    public GameObject TurnProviderLocomotion;

    // Reference to your SceneManagerScript instance
    public SceneManagerScript sceneManager; // Make sure to assign this in the editor



    [SerializeField]
    public GameObject GameOverImage;
    [SerializeField]
    public GameObject GameButtonUI;
    [SerializeField]
    public GameObject RestartButton;
    [SerializeField]
    public GameObject StartButton;

    [SerializeField]
    public GameObject startingPositionObject;
    [SerializeField]
    public Vector3 startingPosition;

    // Threshold for the y-coordinate below which the player is considered to have fallen off the map
    [SerializeField]
    float fallOffThreshold = -10.0f;

    [SerializeField]
    private float GameTime;

    public float CurrentTime;

    public int GameScore;

    public bool GameStarted;

    [SerializeField]
    public GameObject GameScoreUI;

    [SerializeField]
    public GameObject PlayerStatsUI;



    [SerializeField]
    private TextMeshPro TimerValueText;
    [SerializeField]
    private TextMeshPro ScoreValueText;
    //A Serialized Field that will take in a sound of a goll ball hitting a golf club
    [SerializeField]
    AudioClip GolfBallHitSound;
    //A Serialized Field that will take in a sound of a point noise
    [SerializeField]
    AudioClip GolfSuccessSound;
    //A serialized field that will take the fail buzzer noise
    [SerializeField]
    AudioClip GolfFailSound;
    //ClubBreakSound
    [SerializeField]
    AudioClip ClubBreakSound;
    //BumperNoise
    [SerializeField]
    AudioClip BumperNoise;


    


    [SerializeField]
    AudioSource AudioSource;


    //Variables for instance.
    public int HighScore;
    public int currentLevel;
    public int par;
    public int totalStrokes;
    public float distanceToHole;

    // Game state
    private string gameMode;

    //private bool gameStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        // First, check if XROrigin is set in the inspector
        if (XROrigin == null)
        {
            Debug.LogError("XROrigin is not set.");
            // Optionally, you could disable the script or take other appropriate action.
            // For example: this.enabled = false;
            return;
        }

        //// Try to find the XR rig in the current scene by its tag or name
        //if (XROrigin == null)
        //{
        //    XROrigin = GameObject.FindGameObjectWithTag("XRRig");  // Assuming you've tagged the XR rig with "XRRig"
        //}


        // Check for null and log error for ContinuousMovementLocomotionSystem and TurnProviderLocomotion
        if (ContinuousMovementLocomotionSystem == null)
        {
            Debug.LogError("ContinuousMovementLocomotionSystem is not set.");
            return; // Consider adding return here for consistency
        }
        else
        {
            ContinuousMovementLocomotionSystem.SetActive(false);
        }

        if (TurnProviderLocomotion == null)
        {
            Debug.LogError("TurnProviderLocomotion is not set.");
            return; // Consider adding return here for consistency
        }
        else
        {
            TurnProviderLocomotion.SetActive(false);
        }


        // Load high score from PlayerPrefs
        HighScore = PlayerPrefs.GetInt("HighScore", 0);  // 0 is the default value

        // Set the game to not started upon initialization
        GameStarted = false;

        // Initialize the current time for the game mode (if needed)
        CurrentTime = GameTime;

        // Initialize starting position
        startingPositionObject = GameObject.FindGameObjectWithTag("Respawn");
        if (startingPositionObject != null)
        {
            startingPosition = startingPositionObject.transform.position;
            Debug.Log("Starting Position set to: " + startingPosition);
        }
        else
        {
            Debug.LogError("No object with tag 'Respawn' found.");
        }
    }

    void Awake()
    {

        // If sceneManager is not assigned, try finding the GameObject
        if (sceneManager == null)
        {
            // This finds the GameObject named "SceneManager" and retrieves the component
            sceneManager = GameObject.Find("SceneManager").GetComponent<SceneManagerScript>();

            // Alternative: If SceneManagerScript is a singleton, you might access the instance directly
            // sceneManager = SceneManagerScript.Instance;
        }

        if (sceneManager == null)
        {
            Debug.LogError("Failed to find SceneManagerScript. Ensure it is loaded before this script.");
        }

        DontDestroyOnLoad(this.gameObject); // This will keep your game controller alive across scene transitions.
    }


    // Update is called once per frame
    void Update()
    {
        // Check if the player has fallen off the map
        if(XROrigin != null)
        {
            if (XROrigin.transform.position.y < fallOffThreshold)
            {
                //Respawn Player at Starting Position
                RespawnPlayer();

                //Play Error Sound
                PlayGolfErrorSound();
            }


        }
        if (GameStarted)
        {
            CurrentTime -= Time.deltaTime;
            Debug.Log(CurrentTime);
        }

        if (CurrentTime <= 0)
        {
            GameStarted = false;

            // Combine UI element activation into one block
            if (GameButtonUI != null) GameButtonUI.SetActive(true);
            if (RestartButton != null) RestartButton.SetActive(true);
            if (GameOverImage != null) GameOverImage.SetActive(true);


            print("Game Over");
        }

        //update the timer and score if safe
        if (TimerValueText != null)
        {
            TimerValueText.text = CurrentTime.ToString("00");
        }
        else
        {
            Debug.LogWarning("GameObject with tag TimerValueText not found.");
        }
        if (ScoreValueText != null)
        {
            ScoreValueText.text = GameScore.ToString("00");
        }
        else
        {
            Debug.LogWarning("GameObject with tag TimerValueText not found.");
        }

    }

    public void StartGame()
    {
        

        Debug.Log("Start Game Called");
        if (GameStarted) return; // Prevent starting the game again if it's already running




        // Read the game mode from PlayerPrefs and start the appropriate mode
        string gameMode = PlayerPrefs.GetString("GameMode", "FreePlay"); // Default to FreePlay if not set

        // Indicate that the game has started
        GameStarted = true;
        RespawnPlayer();
        SpawnGolfBall();


        if(gameMode.Equals("FreePlay"))
        {
            CurrentTime = 9999;
            par = 9999;

            PlayerStatsUI.SetActive(true); // you only want PlayerStats to appear after starting the game
            GameScoreUI.SetActive(false);
            
        }
        else if(gameMode.Equals("TimeTrial"))
        {
            PlayerStatsUI.SetActive(true);
            CurrentTime = GameTime;
            //if par was not set equal to a value set it to 15
            if (par == 0)
            {
                par = 15;

            }
            totalStrokes = 0;
            GameScore = par - totalStrokes;

        }




        ContinuousMovementLocomotionSystem.SetActive(true);
        TurnProviderLocomotion.SetActive(true);
        StartButton.SetActive(false);
        GameButtonUI.SetActive(false);
        RestartButton.SetActive(false);
        GameOverImage.SetActive(false);

        Debug.Log(gameMode);
    }

    public void RestartGame()
    {
        CurrentTime = GameTime;
        PlayGolfErrorSound();
        //Go back to scene 0 in build index
        SceneManager.LoadScene(0);

        totalStrokes = 0;
        GameScore = par - totalStrokes;

        GameButtonUI.SetActive(false);
        RestartButton.SetActive(false);
        GameOverImage.SetActive(false);
        GameStarted = true;
        RespawnPlayer();
        SpawnGolfBall();
    }

    public void AddScore()
    {
        if (GameStarted)
        {

            if (GameScore > HighScore)
            {
                // Update the high score
                HighScore = GameScore;

                // Save the high score to PlayerPrefs
                PlayerPrefs.SetInt("HighScore", HighScore);
                PlayerPrefs.Save();  // Don't forget to save!
            }

            //Respawn Player at Starting Position
            RespawnPlayer();

            //Reset Score
            GameScore = 0;

            //Reset totalStrokes
            totalStrokes = 0;

        }
    }

    public void RespawnPlayer()
    {
        // Starting position coordinates = location of the GameObject with tag "Respawn"
        startingPosition = GameObject.FindGameObjectWithTag("Respawn").transform.position;

        //Respawn Player at Starting Position
        XROrigin.transform.position = startingPosition;
        Debug.Log("Respawned player at: " + XROrigin.transform.position);

        //Respawn Golf Club
        SpawnGolfClub();

    }

    public void SpawnGolfBall()
    {
        Spawner.SpawnBall();
    }

    public void SpawnGolfClub()
    {
        // Check if 'Spawner' is not null before calling the method on it
        if (Spawner != null)
        {
            Spawner.SpawnClub();  // This is safe as we're sure Spawner is not null
        }
        else
        {
            // Log an error message to understand if the 'Spawner' was not set in the editor
            Debug.LogError("Spawner is not assigned in the editor.");
        }
    }

    public void PlayGolfBallHitSound()
    {
        AudioSource.PlayOneShot(GolfBallHitSound);
    }

    public void PlayGolfSuccessSound()
    {
        AudioSource.PlayOneShot(GolfSuccessSound);
    }

    public void PlayGolfErrorSound()
    {
        AudioSource.PlayOneShot(GolfFailSound);
    }
    //play club break sound
    public void PlayClubBreakSound()
    {
        AudioSource.PlayOneShot(ClubBreakSound);
    }

    public void PlayBumperNoise()
    {
        AudioSource.PlayOneShot(BumperNoise);
    }


    //Clear High Score
    public void ClearHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        HighScore = 0;
    }

}
