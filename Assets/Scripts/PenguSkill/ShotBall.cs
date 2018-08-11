using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ShotBall : MonoBehaviour {

    // 本次攻击的发起者
    public int attackerId;
    // 已经击中了
    public bool isHitted = false;
    [HideInInspector]
    public float chargeAttackTime = 0;

    protected Color ballColor = Color.black;
    
    public void SpawnBall(int ownerId, Vector3 position, Quaternion rotation, float chargeTime)
    {
        this.isHitted = false;
        this.attackerId = ownerId;
        SetChargeAttackTime(chargeTime);
        GameObject cloneBall = Instantiate(gameObject, position, rotation);
        Debug.Log("attackerId = " + cloneBall.GetComponent<ShotBall>().attackerId);
        Debug.Log(cloneBall.GetComponent<ShotBall>().isHitted ? "击中" : "未击中");
        //cloneBall.GetComponent<ShotBall>().SetChargeAttackTime(chargeAttackTime);
    }
    
    // 初始化Ball
    public virtual void InitiateBall(int ownerId, float chargeTime, Color color)
    {
        this.isHitted = false;
        this.attackerId = ownerId;
        this.chargeAttackTime = chargeTime;
        this.ballColor = color;
    }

    public void SetChargeAttackTime(float time = 0)
    {
        chargeAttackTime = time;
    }
}
