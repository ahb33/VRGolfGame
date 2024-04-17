using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoWalker : MonoBehaviour
{
    [SerializeField]
    public Vector3 startingPosition;

    // Variable for walking speed
    [SerializeField]
    public float walkingSpeed = 1f;

    // Variable for animation speed
    [SerializeField]
    public float animationSpeed = 1f;

    // Variable for move duration
    [SerializeField]
    public float moveDuration = 4f;

    // Variable for pause duration
    [SerializeField]
    public float pauseDuration = 2f;

    // Time until the dinosaur turns around
    [SerializeField]
    public float turnTime = 40f;

    // Reference to the Animator component
    private Animator animator;

    // Total time dinosaur has been walking
    [SerializeField]
    public float totalWalkTime = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize starting position
        startingPosition = transform.position;

        // Get the Animator component from this GameObject
        animator = GetComponent<Animator>();

        // If the Animator is not null, set its speed
        if (animator != null)
        {
            animator.speed = animationSpeed;
        }

        // Start the walking coroutine
        StartCoroutine(WalkAndPause());
    }

    // Coroutine for walking and pausing
    IEnumerator WalkAndPause()
    {
        while (true)
        {
            // Walk for moveDuration seconds
            float walkTime = Time.time + moveDuration;
            while (Time.time < walkTime)
            {
                transform.position += transform.forward * walkingSpeed * Time.deltaTime;
                yield return null;
            }

            // Update the total walk time
            totalWalkTime += moveDuration;

            // Check if it's time to turn around
            if (totalWalkTime >= turnTime)
            {
                // Reset total walk time
                totalWalkTime = 0f;

                // Turn around (rotate 180 degrees)
                transform.Rotate(0f, 180f, 0f);
            }

            // Pause for the duration set in pauseDuration
            yield return new WaitForSeconds(pauseDuration);
        }
    }
}
