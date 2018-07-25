using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ShotBall : MonoBehaviour {

    // 本次攻击的发起者
    protected int attackerId;
    [HideInInspector]
    public float chargeAttackTime = 0;
    
    public void SpawnBall(int ownerId, Vector3 position, Quaternion rotation, float chargeTime)
    {
        attackerId = ownerId;
        SetChargeAttackTime(chargeTime);
        GameObject cloneBall = Instantiate(gameObject, position, rotation);
        //cloneBall.GetComponent<ShotBall>().SetChargeAttackTime(chargeAttackTime);
    }



    public void SetChargeAttackTime(float time = 0)
    {
        chargeAttackTime = time;
    }
}
