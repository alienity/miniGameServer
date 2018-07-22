using UnityEngine;
using System.Collections;

public class SnowBall : ShotBall
{
    // 移动速度
    public float flySpeed;
    // 持续时间
    public float flyTime;
    // 已经飞行时间
    private float fliedTime = 0;
    // 击退加成速度
    public float suddenSpeed;
    
    // 自有组件
    private Collider mCollider;
    private Transform mTrans;

    void Start()
    {
        mCollider = GetComponent<Collider>();
        mTrans = GetComponent<Transform>();
        fliedTime = 0;
    }
    
    private void FixedUpdate()
    {
        if (fliedTime < flyTime)
        {
            mTrans.position += mTrans.forward * flySpeed * Time.deltaTime;
            fliedTime += Time.deltaTime;
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
            gp.EffectSpeedMovement(mTrans.forward * suddenSpeed);
            gp.SetAttacker(attackerId); // 设置攻击者Id
            DestroySelf();
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // 后期优化做修改
    }

}
