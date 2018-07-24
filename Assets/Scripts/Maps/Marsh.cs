using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marsh : MonoBehaviour {

    public int dampingRadio =2;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
            other.GetComponent<Rigidbody>().drag = other.GetComponent<Rigidbody>().mass * dampingRadio;
    }
}
