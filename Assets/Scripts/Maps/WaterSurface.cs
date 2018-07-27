using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : BoxEffects
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // other.GetComponent<Rigidbody>().useGravity = true;
            other.GetComponent<GroupPlayer>().Die();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<GroupPlayer>().Die();
        }
    }

}
