using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSurface : MonoBehaviour {

    // 阻力减小系数
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
