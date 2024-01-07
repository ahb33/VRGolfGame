using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject GolfClubObject;

    [SerializeField]
    private GameObject GolfBallObject;

    // Distance in front of the player to spawn the object
    [SerializeField]
    private float spawnDistance = 0.5f;

    [SerializeField]
    private Vector3 clubRightOffset = new Vector3(0.5f, 0f, 0f); // Adjust as needed


    // Transform locations to spawn the objects
    public Transform GolfBallTransform;
    public Transform GolfClubTransform;

    public GameObject PlayerPosition;

    public GolfGameController Controller;

    private void Update()
    {
        
    }


    public void SpawnBall()
    {
        //Destroy All Golf Balls
        GameObject[] golfBalls = GameObject.FindGameObjectsWithTag("GolfBall");
        foreach (GameObject golfBall in golfBalls)
        {
            Destroy(golfBall);
        }

        if (GolfBallObject != null && Controller != null && GolfBallTransform != null)
        {
            GolfBall golfBall = GolfBallObject.GetComponent<GolfBall>();
            if (golfBall != null)
            {
                golfBall.SetGolfGameController(Controller);
            }

            GameObject GameGolfBall = Instantiate(GolfBallObject, GolfBallTransform.position, GolfBallTransform.rotation);

            Debug.Log("Spawned Golf Ball at: " + GameGolfBall.transform.position);
            Debug.Log("Is Golf Ball parented to something? " + (GameGolfBall.transform.parent != null));

            GameGolfBall.transform.parent = null;

            Rigidbody rb = GameGolfBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }
        }
    }

    public void SpawnClub()
    {
        //Destroy All Golf Clubs
        GameObject[] golfClubs = GameObject.FindGameObjectsWithTag("GolfClub");
        foreach (GameObject golfClub in golfClubs)
        {
            Destroy(golfClub);
        }

        if (PlayerPosition != null)
        {
            // Debug line to print the PlayerPosition
            //Debug.Log("Player Position: " + PlayerPosition.transform.position);

            // Calculate the position in front of the player
            Vector3 spawnPosition = PlayerPosition.transform.position + PlayerPosition.transform.forward * spawnDistance;

            // Add an upward offset to the y-coordinate to raise the objects above the ground
            float upwardOffset = 1f;  // Adjust this value as needed
            spawnPosition.y += upwardOffset;

            // Update the GolfClub Transforms to be in front of the player

            if (GolfClubTransform != null)
            {
                GolfClubTransform.position = spawnPosition;
            }
        }

        if (GolfClubObject != null && Controller != null && GolfClubTransform != null)
        {
            GolfClub golfClub = GolfClubObject.GetComponent<GolfClub>();
            if (golfClub != null)
            {
                golfClub.SetGolfGameController(Controller);
            }

            GameObject GameGolfClub = Instantiate(GolfClubObject, GolfClubTransform.position, GolfClubTransform.rotation);
            Debug.Log("Spawned Golf Club at: " + GameGolfClub.transform.position);
            Debug.Log("Is Golf Club parented to something? " + (GameGolfClub.transform.parent != null));

            Rigidbody rb = GameGolfClub.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            GameGolfClub.transform.parent = null;
        }
    }

    // function will be used to disable golf club once timer in golfgame controller is out
    public void DisableAllGolfClubs()
    {
        GameObject[] golfClubs = GameObject.FindGameObjectsWithTag("GolfClub");
        foreach (GameObject golfClub in golfClubs)
        {
            
            golfClub.SetActive(false);


        }
    }
}