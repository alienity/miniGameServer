using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class ShotBall : MonoBehaviour {

    // 球的类型
    public enum BallType{
        SnowBall,
        FireBall
    };
    public BallType ballType;
    // 本次攻击的发起者
    public int attackerId;
    // 已经击中了
    [HideInInspector] public bool isHitted = false;
    // 蓄力时间
    [HideInInspector] public float chargeAttackTime = 0;
    // 球颜色
    protected Color ballColor = Color.black;
    
    // 初始化Ball
    public virtual void InitiateBall(int ownerId, float chargeTime, Color color, BallType bType)
    {
        this.isHitted = false;
        this.attackerId = ownerId;
        this.chargeAttackTime = chargeTime;
        this.ballColor = color;
        this.ballType = bType;
    }

    // 设置充能时间
    public void SetChargeAttackTime(float chargeAttackTime)
    {
        this.chargeAttackTime = chargeAttackTime;
    }

}
