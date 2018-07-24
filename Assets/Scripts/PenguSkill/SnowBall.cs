using UnityEngine;
using System.Collections;

public class SnowBall : ShotBall
{
    // 移动速度
    public float flySpeed;
    // 持续时间
    private float flyDist;
    public float flyTime;
    // 已经飞行时间
    private float fliedTime = 0;
    private float fliedDist = 0;
    // 击退加成速度
    public float suddenSpeed;
    // 蓄力击退强度
    public float attackStrength = 8;
    
    // 自有组件
    private Collider mCollider;
    private Transform mTrans;

    void Start()
    {
        mCollider = GetComponent<Collider>();
        mTrans = GetComponent<Transform>();
        flyDist = flyTime * flySpeed;
        fliedTime = 0;
    }
    
    private void FixedUpdate()
    {
        if (fliedTime < flyTime &&( fliedDist < flyDist ))
        {
            mTrans.position += mTrans.forward * flySpeed * Time.deltaTime;
            fliedTime += Time.deltaTime;
            fliedDist += Time.deltaTime * (flySpeed+chargeAttackTime);
            Debug.Log("flying....");
        }
        else
        {
            DestroySelf();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int otherId = other.GetComponent<GroupPlayer>().gId;
            if (otherId == attackerId) return;
            GroupPlayer gp = other.GetComponent<GroupPlayer>();
            gp.EffectSpeedMovement(mTrans.forward * (suddenSpeed + chargeAttackTime * attackStrength));
            //积分
            gp.SetAttacker(attackerId); // 设置攻击者Id
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // 后期优化做修改
    }

}
