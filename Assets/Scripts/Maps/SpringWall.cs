using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpringWall :BoxEffects
{
    private AudioSource selfAudioSource;
    public AudioClip hittedAudio;

    private void Start()
    {
        selfAudioSource = GetComponent<AudioSource>(); ;
        selfAudioSource.clip = hittedAudio;
        Debug.Log("spring start");
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.tag == "Ball")
        {

            selfAudioSource.Play();

            Vector3 symmetryAxis = gameObject.transform.position - collider.transform.position;
            Vector3 forward = collider.transform.forward;

            Vector3 newVector3 = -2 * (Vector3.Dot(forward, symmetryAxis)) / (Vector3.Dot(forward, forward)) * forward - symmetryAxis;
            newVector3.y = 0;
            collider.transform.forward = newVector3; 
        }
    }
}