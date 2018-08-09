using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem Trail;
    public ParticleSystem markCircle;

    //wwq
    public float explosionRadius;
    public float radio = 7;

    private bool readyPlay = true;
    private void Start()
    {
        readyPlay = true;
        Instantiate(Trail, transform);
        Vector3 circlePos = transform.position;
        circlePos.y = 6.2f;
        /*markCircle =*/

        Destroy(Instantiate(markCircle, circlePos, Quaternion.LookRotation(new Vector3(0,-90,0))).gameObject,3);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("line explosion"+" "+collision.gameObject.tag);
        if (collision.gameObject.tag == "Plane"||collision.gameObject.tag == "Player")
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity).gameObject, 1);
            explode();
        }
    }
    private void explode()
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
                    explosionvelocity = explosionvelocity *(radio*-1);
                    b.velocity = explosionvelocity;
                }
            }
        }
        Destroy(this.gameObject);
    }
}
