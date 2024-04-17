using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateSunOnStart : MonoBehaviour
{
    // Variable for degrees to rotate
    [SerializeField]
    private float degreesToRotate = 45.0f;  // Rotate by 45 degrees as an example

    // Variable for duration of rotation in seconds
    [SerializeField]
    private float rotationDuration = 5.0f;  // Rotate over 5 seconds as an example

    private float startTime;
    private Quaternion originalRotation;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        originalRotation = this.transform.rotation;  // Save the original rotation
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the elapsed time
        float elapsedTime = Time.time - startTime;

        // Calculate the fraction of the total duration
        float fraction = elapsedTime / rotationDuration;

        // If the rotation is not yet complete
        if (fraction < 1.0f)
        {
            // Calculate the new rotation
            Quaternion startRotation = originalRotation;
            Quaternion endRotation = originalRotation * Quaternion.Euler(degreesToRotate, 0, 0);
            Quaternion currentRotation = Quaternion.Lerp(startRotation, endRotation, fraction);

            // Apply the rotation
            transform.rotation = currentRotation;
        }
    }
}
