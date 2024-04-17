using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPlayerToGroundLevel : MonoBehaviour
{
    void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }
}
