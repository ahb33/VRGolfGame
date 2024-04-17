using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void StartGameAgainstClock()
    {
        // Here, you should set the game mode using your SceneManagerScript
        SceneManagerScript.Instance.SetGameMode("TimeTrial");

        //If there is no more in the build index then load the first scene
        // Or load by scene index (if you've set this in Build Settings)
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    }

    public void StartFreePlayMode()
    {
        SceneManagerScript.Instance.SetGameMode("FreePlay");
        //If there is no more in the build index then load the first scene
        // Or load by scene index (if you've set this in Build Settings)
        if (SceneManager.sceneCountInBuildSettings <= SceneManager.GetActiveScene().buildIndex + 1)
        {
            SceneManager.LoadScene(0);

        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
