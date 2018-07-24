using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSurface : BoxEffects
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("die");
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<GroupPlayer>().Die() ;
    }
}
