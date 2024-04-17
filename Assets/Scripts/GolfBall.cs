using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.SceneManagement;

public class GolfBall : MonoBehaviour
{
    [SerializeField]
    GolfGameController Controller;
    Rigidbody rb;


    // Threshold for the y-coordinate below which the ball is considered to have fallen off the map
    [SerializeField]
    float fallOffThreshold = -10.0f;

    public void SetGolfGameController(GolfGameController controller)
    {
        Controller = controller;
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the ball has fallen off the map
        if (transform.position.y < fallOffThreshold)
        {
            if (Controller != null)
            {
                Destroy(this.gameObject);
                Controller.PlayGolfErrorSound();
                Controller.SpawnGolfBall();
                Controller.RespawnPlayer();
            }
        }

        // Update distanceToHole of Controller by calculating distance to GameObject with tag "Hole"
        if (Controller != null)
        {
            GameObject hole = GameObject.FindGameObjectWithTag("Hole");
            if (hole != null)
            {
                Controller.distanceToHole = Vector3.Distance(this.transform.position, hole.transform.position);
            }
            else
            {
                Debug.LogWarning("No GameObject found with the tag 'Hole'");  // Debug warning
            }
        }
        else
        {
            Debug.LogWarning("Controller is not initialized");  // Debug warning
        }



    }

    //ontriggerenter
    private void OnTriggerEnter(Collider other)
    {
        //print "collided with" + the tag of what we collided with
        print("Collided with " + other.gameObject.tag);

        if (other.gameObject.tag == "KillZone")
        {
            if (Controller != null)
            {
                Destroy(this.gameObject);
                Controller.PlayGolfErrorSound();
                Controller.SpawnGolfBall();
                Controller.RespawnPlayer();

            }
        }

        //if the other object has the tag Hole
        else if (other.gameObject.tag == "Hole")
        {
            //if the controller is not null
            if (Controller != null)
            {
                //play the golf success sound
                Controller.PlayGolfSuccessSound();
                //destroy the game object
                Destroy(this.gameObject);
                //Trigger AddScore
                Controller.AddScore();

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
    }
}
