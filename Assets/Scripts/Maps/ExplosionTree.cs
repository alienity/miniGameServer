using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTree : BoxEffects
{
    public short lifeNumber = 5;
    public float explosionRadius = 50;
    public float radio = 7;

    private AudioSource selfAudioSource;
    public void Start()
    {
        selfAudioSource = gameObject.AddComponent<AudioSource>();
        selfAudioSource.clip = Resources.Load("Explosion") as AudioClip;
    }
    private void Explode()
    {
       
            Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
            if (col.Length > 0)
            {
                Debug.Log(col.Length);
                for (int i = 0; i < col.Length; i++)
                {
                    Vector3 velocityDir = transform.position - col[i].transform.position;
                    velocityDir.y = 0;
                    Rigidbody b = col[i].GetComponent<Rigidbody>();
                    if (b != null)
                    {
                        Vector3 explosionvelocity = (10 - velocityDir.magnitude) * velocityDir.normalized;
                        explosionvelocity = explosionvelocity * (radio * -1);
                        b.velocity = explosionvelocity;
                    }
                }
            }
            Destroy(this.gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        lifeNumber--;
        Debug.Log(lifeNumber);
        Destroy(other.gameObject);
        if (lifeNumber == 0) Explode();
    }
    void OnCollisionEnter()

    {
        lifeNumber--;
        if (lifeNumber == 0) Explode();
    }

}
