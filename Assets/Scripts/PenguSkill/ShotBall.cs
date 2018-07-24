using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ShotBall : MonoBehaviour {

    // 本次攻击的发起者
    protected int attackerId;
    protected float chargeAttackTime = 0;
    public void SpawnBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        attackerId = ownerId;
        GameObject cloneBall =Instantiate(gameObject, position, rotation);
        cloneBall.GetComponent<ShotBall>().SetChargeAttackTime(chargeAttackTime);
        SetChargeAttackTime(0);
    }
    public void SetChargeAttackTime(float time = 0)
    {
        if (time > 4.0f) time = 4.0f;
        chargeAttackTime = time;
    }
}
