using UnityEngine;
using System.Collections;

public class SnowBall : ShotBall
{
    // 移动速度
    public float flySpeed;
    // 飞行速度加成
    public float flySpeedAdd;
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
    // 可以碰到自己
    private bool canTouchSelf = false;
    // 多少秒之后取消自己的碰撞
    public float cancelTouchDuring = 0.4f;
    // 自有组件
    private Collider mCollider;
    private Transform mTrans;

    private void Start()
    {
        mCollider = GetComponent<Collider>();
        mTrans = GetComponent<Transform>();
        flyDist = flyTime * flySpeed;
        fliedTime = 0;
        StartCoroutine(CountDownTouchSelf(cancelTouchDuring));
    }
    
    private void FixedUpdate()
    {
        if ((fliedTime < flyTime) && ( fliedDist < flyDist))
        {
            float newSpeed = flySpeed + flySpeedAdd * chargeAttackTime;
            mTrans.position += mTrans.forward * newSpeed * Time.deltaTime;
            fliedTime += Time.deltaTime;
            fliedDist += Time.deltaTime * newSpeed;
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
            GroupPlayer gp = other.GetComponent<GroupPlayer>();
            // 当组是无敌的时候，执行组的对应的方法
            if (gp.isInvulnerable)
            {
                gp.ReflectAttack(transform);
                return;
            }
            // 正常情况下执行触碰攻击方法
            if (!canTouchSelf)
            {
                int otherId = other.GetComponent<GroupPlayer>().gId;
                if (otherId == attackerId) return;
            }
            gp.EffectSpeedMovement(transform.forward * (suddenSpeed + chargeAttackTime * attackStrength));
            gp.SetAttacker(attackerId); // 设置攻击者Id
            DestroySelf();
        }
    }

    IEnumerator CountDownTouchSelf(float time)
    {
        canTouchSelf = false;
        yield return new WaitForSeconds(time);
        canTouchSelf = true;
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // 后期优化做修改
    }

}
