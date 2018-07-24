using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWall :BoxEffects
{
    private void Awake()
    {
        Debug.Log("spring start");
    }
    private void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "Ball")
        {
            Vector3 symmetryAxis = gameObject.transform.position - collider.transform.position;
            Vector3 Forward = collider.transform.forward;

            Vector3 newVector3 = -2 * (Vector3.Dot(Forward, symmetryAxis)) / (Vector3.Dot(Forward, Forward)) * Forward -symmetryAxis;
            Debug.Log(newVector3.ToString());
            collider.transform.forward = newVector3; 
        }
    }
}