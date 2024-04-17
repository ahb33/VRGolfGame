using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public static SceneManagerScript Instance { get; private set; } // Singleton instance

    // Variables to keep track of game state
    private string currentGameMode;

    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("SceneManager set to: " + gameObject.name);
        }
        else if (Instance != this)
        {
            Debug.Log("Duplicate SceneManager found on: " + gameObject.name + "; destroying duplicate.");
            Destroy(gameObject);
        }
    }

    public void SetGameMode(string gameMode)
    {
        currentGameMode = gameMode;
        PlayerPrefs.SetString("GameMode", gameMode);
    }

    public string GetGameMode()
    {
        return currentGameMode;
    }

    // Call this to start the TimeTrial mode
    public void StartTimeTrialMode()
    {
        PlayerPrefs.SetString("GameMode", "TimeTrial");
        LoadSceneByName("Golf1");
    }

    // Call this to start the FreePlay mode
    public void StartFreePlayMode()
    {
        PlayerPrefs.SetString("GameMode", "FreePlay");
        LoadSceneByName("Golf1");
    }

    // General function to load scenes
    public void LoadSceneByName(string sceneName)
    {
        // Check if the scene is available.
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            SceneManager.LoadScene(sceneName);

            // If transitioning to the Golf1 scene, destroy any existing XR rigs
            if (sceneName == "Golf1")
            {
                GameObject existingXRRig = GameObject.FindGameObjectWithTag("XRRig");
                if (existingXRRig != null)
                {
                    Destroy(existingXRRig);
                }
            }      
        }
        else
        {
            Debug.LogError("Scene " + sceneName + " cannot be loaded. Check if it's added in the build settings.");
        }
    }

    // This function will be used to restart the game or handle game over logic
    //public void GameOver()
    //{
    //    // Retrieve the current scene's name
    //    string currentScene = SceneManager.GetActiveScene().name;

    //    // Determine the next scene based on the current scene
    //    if (currentScene == "Golf1")
    //    {
    //        LoadSceneByName("Golf2");
    //    }
    //    else if (currentScene == "Golf2")
    //    {
    //        // You can decide what to do after Golf2 ends.
    //        // For example, you can go back to the menu or even go to a third golf map if you have one.
    //        LoadSceneByName("GolfMenu");
    //    }
    //    else
    //    {
    //        // Default case, go back to Golf1 or any other default scene.
    //        LoadSceneByName("Golf1");
    //    }
    //}

    // Call this method when you want to exit to the main menu
    public void ExitToMainMenu()
    {
        LoadSceneByName("GolfMenu");
    }

    // This function could be called by the SceneManager when the scene changes.
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Golf1")
        {
            // Find the menu UI in this scene. This assumes the menu is a direct child of the manager.
            // You could also use a tag or other method to find the menu.
            var menuUI = transform.Find("MenuUI");  // Replace with your actual menu's name or method to find it.
            if (menuUI != null)
            {
                // If found, we're still in a scene that should not have the menu.
                Destroy(menuUI.gameObject);
            }
        }

    }

}