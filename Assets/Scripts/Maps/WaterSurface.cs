using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : BoxEffects
{
    private float radio = 4;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<GroupPlayer>().GetComponent<Rigidbody>().drag /= 2; ;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<GroupPlayer>().GetComponent<Rigidbody>().drag *= 2; ;
    }
}
