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

            float newX= collider.transform.forward.x, newZ= collider.transform.forward.z;
            if (newX > newZ) newX = -newX;
               else newZ = -newZ; 
            collider.transform.forward = new Vector3(newX, 0, newZ);
        }
    }
}