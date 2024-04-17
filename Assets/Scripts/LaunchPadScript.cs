using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchPad : MonoBehaviour
{
    // The amount of time for the ball to reach the target
    public float timeToTarget = 2.0f;
    public float amplitudeFactor = 100.0f; // Increase this value to make the curve higher

    // Reference to the controller gameobject
    public GolfGameController Controller;

    // Reference to the target destination GameObject
    public GameObject targetDestination;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is tagged "GolfBall" or "Player"
        if ( (other.CompareTag("GolfBall")) || (other.CompareTag("Player")) )        
        {
            //if tag is golfball
            if (other.CompareTag("GolfBall"))
            {
                // Get the Rigidbody component of the object if it exists
                Rigidbody GolfBall = other.GetComponent<Rigidbody>();

                if (GolfBall != null)
                {
                    // Stop the ball's existing movement
                    GolfBall.velocity = Vector3.zero;
                    GolfBall.angularVelocity = Vector3.zero;

                    // Start the coroutine to move the ball along the curve
                    StartCoroutine(MoveObjectToTarget(other.transform));
                }
            } else {
                // Start the coroutine to move the person along the curve
                StartCoroutine(MoveObjectToTarget(other.transform));
            }
        }
    }

    // Coroutine to move the ball along a curve to the target destination
    IEnumerator MoveObjectToTarget(Transform ballTransform)
    {
        Controller.PlayBumperNoise();
        Vector3 startPoint = ballTransform.position;
        Vector3 endPoint = targetDestination.transform.position;
        Vector3 controlPoint = (startPoint + endPoint) * 0.5f; // You can modify this for different curves

        controlPoint.y += amplitudeFactor; // Lift the control point up to create a curve

        float elapsedTime = 0;

        while (elapsedTime < timeToTarget)
        {
            float t = elapsedTime / timeToTarget;
            ballTransform.position = CalculateBezierPoint(t, startPoint, controlPoint, endPoint);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ballTransform.position = endPoint;
        
        Controller.SpawnGolfClub();
    }


    // Calculate a point along a quadratic Bezier curve
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}
