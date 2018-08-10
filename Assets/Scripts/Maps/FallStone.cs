using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public ParticleSystem explosion;
    public ParticleSystem Trail;
    public ParticleSystem markCircle;

    //wwq 爆炸半径
    public float explosionRadius;
    // 力度
    public float radio = 7;
    
    private void Start()
    {
        ParticleSystem trailSystem = Instantiate(Trail, transform);
        trailSystem.transform.position = transform.position;
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit))
        {
            Vector3 circlePos = hit.point + Vector3.up;
            Destroy(Instantiate(markCircle, circlePos, Quaternion.LookRotation(Vector3.up)).gameObject, 3);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("line explosion" + collision.gameObject.tag);
        if (collision.gameObject.tag == "Plane"|| collision.gameObject.tag == "Player")
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity).gameObject, 1);
            explode();
        }
    }

    // 为玩家添加速度
    private void explode()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, explosionRadius);
        for (int i = 0; i < col.Length; i++)
        {
            if (col[i].gameObject.tag != "Player") continue;

            Vector3 velocityDir = transform.position - col[i].transform.position;
            velocityDir.y = 0;

            Vector3 explosionvelocity = (explosionRadius - velocityDir.magnitude) * velocityDir.normalized;
            explosionvelocity = -radio * explosionvelocity;
            GroupPlayer gp = col[i].GetComponent<GroupPlayer>();
            gp.EffectSpeedMovement(explosionvelocity);
        }
        Destroy(this.gameObject);
    }
}
