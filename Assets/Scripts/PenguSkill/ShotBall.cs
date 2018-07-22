using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ShotBall : MonoBehaviour {

    // 本次攻击的发起者
    protected int attackerId;
    
    public void SpawnBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        attackerId = ownerId;
        Instantiate(gameObject, position, rotation);
    }

}
