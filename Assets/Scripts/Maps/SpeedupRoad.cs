using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedupRoad : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("加速路面");
            other.GetComponent<GroupPlayer>().PigSpeedupRoad();
        }
            
    }
}
