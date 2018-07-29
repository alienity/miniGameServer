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

    // 特效组件
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    // 自有组件
    private Transform mTrans;

    private void Start()
    {
        mTrans = GetComponent<Transform>();
        flyDist = flyTime * flySpeed;
        fliedTime = 0;
        
        AddParticles();

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
            ShowHitParticleEffects(impactNormal);
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

            if (isHitted) return;
            isHitted = true;

            ShowHitParticleEffects(impactNormal);

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
        Destroy(gameObject, 3f); // 后期优化做修改
    }

    // 启动时添加粒子效果
    private void AddParticles()
    {
        impactNormal = mTrans.forward;
        projectileParticle = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticle.transform.parent = transform;
        projectileParticle.transform.localScale = Vector3.one;
        if (muzzleParticle)
        {
            muzzleParticle = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticle.transform.localScale = Vector3.one;
            Destroy(muzzleParticle, 1.5f); // Lifetime of muzzle effect.
        }
    }

    // 碰撞到时，添加粒子特效
    private void ShowHitParticleEffects(Vector3 impactNormal)
    {

        impactParticle = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

        //yield WaitForSeconds (0.05);
        foreach (GameObject trail in trailParticles)
        {
            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            curTrail.transform.parent = null;
            curTrail.transform.localScale = Vector3.one;
            Destroy(curTrail, 3f);
        }
        Destroy(projectileParticle, 3f);
        Destroy(impactParticle, 5f);
        Destroy(gameObject);
        //projectileParticle.Stop();

        ParticleSystem[] trails = GetComponentsInChildren<ParticleSystem>();
        //Component at [0] is that of the parent i.e. this object (if there is any)
        for (int i = 1; i < trails.Length; i++)
        {
            ParticleSystem trail = trails[i];
            if (!trail.gameObject.name.Contains("Trail"))
                continue;

            trail.transform.SetParent(null);
            trail.transform.localScale = Vector3.one;
            Destroy(trail.gameObject, 2);
        }
    }

}
