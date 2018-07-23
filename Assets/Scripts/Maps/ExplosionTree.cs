using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTree : BoxEffects

{
    private short lifeNumber = 5;
    private float explosionRadius = 5;
    private float radio = -3.5f;

    private void explode()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);

        if (col.Length > 0)
        {
            Debug.Log(" tree die!!!!! = .= ");
            for (int i = 0; i < col.Length; i++)
            {
                Vector3 velocityDir = transform.position - col[i].transform.position;
                velocityDir.y = 0;
                Rigidbody b = col[i].GetComponent<Rigidbody>();
                if (b != null)
                {
                    Vector3 explosionvelocity = (explosionRadius - velocityDir.magnitude) * velocityDir.normalized;
                    explosionvelocity = explosionvelocity * radio;
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
        if (lifeNumber == 0) explode();
    }
    void OnCollisionEnter()

    {
        lifeNumber--;
        if (lifeNumber == 0) explode();
    }

}
