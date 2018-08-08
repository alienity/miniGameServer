using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem Trail;
    private void Start()
    {
        Instantiate(Trail, transform);
    }
    private void OnCollisionEnter(Collision collision)
    {
        explosion = Instantiate(explosion,transform.position,Quaternion.identity) as ParticleSystem;

        Destroy(this.gameObject, 0.1f);
    }
}
