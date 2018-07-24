using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormHole : BoxEffects
{
    public GameObject matchHole;
    public float CDtime =0;
    private void Update()
    {
        CDtime -= Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (CDtime > 0) return;
        matchHole.GetComponent<WormHole>().CDtime = 3;
        other.transform.position = matchHole.transform.position;
    }
}
