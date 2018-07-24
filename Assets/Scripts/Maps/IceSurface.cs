using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSurface : MonoBehaviour {

    // Use this for initialization
    public float radio = 4;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<GroupPlayer>().GetComponent<Rigidbody>().drag /=radio;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
            other.gameObject.GetComponent<GroupPlayer>().GetComponent<Rigidbody>().drag *= radio; ;
    }
}
