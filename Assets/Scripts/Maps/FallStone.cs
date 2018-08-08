using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStone : MonoBehaviour {

    public RandomMap map_;
    public ParticleSystem explosion;
    public ParticleSystem Trail;
    public ParticleSystem markCircle;
    private bool readyPlay = true;
    private void Start()
    {
        readyPlay = true;
        Instantiate(Trail, transform);
        Vector3 circlePos = transform.position;
        circlePos.y = 10;
        /*markCircle =*/
            Destroy(Instantiate(markCircle,circlePos,new Quaternion(0,90,0,0)),5);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("line explosion"+" "+collision.gameObject.tag);
        if (collision.gameObject.tag != "FallStone")
        {
            Destroy(Instantiate(explosion, transform.position, Quaternion.identity), 1);
            Destroy(this.gameObject, 0.2f);
            map_.getObject();
        }
    }
}
