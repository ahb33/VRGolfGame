using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{

    [SerializeField]
    private float LeftTargetPost = -5f; // adjust in editor- the target should not sway back and forth greater than this range

    [SerializeField]
    private float RightTargetPost = 5f;

    [SerializeField]
    private float speed = -1f;

    private float amplitude; // maximum distance from the center

    public Transform other;

    // Target swaying range should be sin( value of left post) --> sin(value of right post)

    // Start is called before the first frame update
    void Start()
    {
        amplitude = (RightTargetPost - LeftTargetPost) / 2f; /* amplitude is half the distance the target is going to move; Sin function will always return a value 
                                                              * between -1 and 1. Multiplying sin by the amplitude the range will be -amplitude to +amplitude*/


    }

    // Update is called once per frame
    void Update()
    {
        // Only shift the target along the Z-axis based on the sine calculation
        float zPosition = amplitude * Mathf.Sin(Time.time * speed) + (LeftTargetPost + RightTargetPost) / 2f;

        // Apply the new Z position to the target while keeping its X and Y positions the same
        this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, zPosition);

    }
}
