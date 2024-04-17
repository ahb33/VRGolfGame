using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit; // Make sure to add this

public class GolfClub : MonoBehaviour
{
    [SerializeField]
    GolfGameController Controller;

    [SerializeField]
    Collider hitCollider; // Collider variable for the golf club's "HitCollider"

    private Vector3 previousPosition;
    private Vector3 currentVelocity;

    private float collisionCooldown = 0.1f; // Set the cooldown period (in seconds)
    private float nextCollisionTime = 0;

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
        // Initialize the previous position to the HitCollider's initial position
        previousPosition = hitCollider.transform.position;
    }

    
    // Update is called once per frame
    void Update()
    {
        // Check if the ball has fallen off the map
        if (transform.position.y < fallOffThreshold)
        {
            if (Controller != null)
            { 
                Controller.PlayClubBreakSound();
                Controller.SpawnGolfClub();
            }
        }

        // Calculate the velocity of the HitCollider
        currentVelocity = (hitCollider.transform.position - previousPosition) / Time.deltaTime;

        // Update the previous position for the next frame
        previousPosition = hitCollider.transform.position;

        //
        
    }

    //OnTriggerEnter
    private void OnTriggerEnter(Collider other)
    {
        //print "collided with" + the tag of what we collided with
        print("Collided with " + other.gameObject.tag);

        

        if (other.gameObject.tag == "KillZone")
        {
            if (Controller != null)
            {
                Controller.PlayClubBreakSound();
                Controller.SpawnGolfClub();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if it's time for the next collision
        if (Time.time > nextCollisionTime)
        {
            // Check if collided with an object tagged "GolfBall"
            if (collision.gameObject.tag == "GolfBall")
            {
                // Play the golf ball hit sound
                Controller.PlayGolfBallHitSound();

                // Get the rigidbody component of the ball
                Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

                // Use the magnitude of the current velocity to scale the force applied to the ball
                float speed = currentVelocity.magnitude * 120f; // You can adjust the scaling factor

                // Calculate the force using the club's current velocity
                Vector3 force = currentVelocity.normalized * speed;

                // Apply the force to simulate the ball being hit
                rb.AddForce(force);

                // Add a stroke to the score
                Controller.totalStrokes += 1;

                //GameScore = par - totalStrokes
                Controller.GameScore = Controller.par - Controller.totalStrokes;
                

                // Update the time for the next possible collision
                nextCollisionTime = Time.time + collisionCooldown;
            }
        }
    }
}
