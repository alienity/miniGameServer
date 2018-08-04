using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurvivalBoard : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<GroupPlayer>().IsSurivalSkillNow = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<GroupPlayer>().IsSurivalSkillNow = false;
        }
    }
}
