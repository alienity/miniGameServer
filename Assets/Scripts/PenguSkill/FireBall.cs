using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : ShotBall
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
    // 击退加成速度
    public float suddenSpeed;
    // 蓄力击退强度
    public float attackStrength = 8;
    // 可以碰到自己
    private bool canTouchSelf = false;
    // 多少秒之后取消自己的碰撞
    public float cancelTouchDuring = 0.4f;
    // 爆炸半径
    public float boomRadius = 6f;
    // 半径威力加成
    public float explosionRatio = 2;

    // 特效组件
    public GameObject impactParticle;
    public GameObject projectileParticle;
    public GameObject muzzleParticle;
    public GameObject[] trailParticles;
    [HideInInspector]
    public Vector3 impactNormal; //Used to rotate impactparticle.

    // 特效组建实例
    private GameObject impactParticleInstance;
    private GameObject projectileParticleInstance;
    private GameObject muzzleParticleInstance;
    private GameObject[] trailParticlesInstance;

    // 自有组件
    private Transform mTrans;

    private void Start()
    {
        mTrans = GetComponent<Transform>();
        fliedTime = 0;

        AddParticles();
        SetSnowBallColor(ballColor);
        StartCoroutine(CountDownTouchSelf(cancelTouchDuring));
    }

    private void FixedUpdate()
    {
        if (fliedTime < flyTime)
        {
            float newSpeed = flySpeed + flySpeedAdd * chargeAttackTime;
            mTrans.position += mTrans.forward * newSpeed * Time.deltaTime;
            fliedTime += Time.deltaTime;
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
            int otherId = other.GetComponent<GroupPlayer>().gId;
            if (!canTouchSelf && (otherId == attackerId)) return;
        }

        if (isHitted) return;
        isHitted = true;

        ShowHitParticleEffects(impactNormal);

        Collider[] colliders = Physics.OverlapSphere(transform.position, boomRadius);
        foreach (Collider collider in colliders)
        {
            if(collider.gameObject.tag == "Player")
            {
                Debug.Log(collider.gameObject.name);
                int otherId = collider.GetComponent<GroupPlayer>().gId;
                if (!canTouchSelf && otherId == attackerId) continue;

                GroupPlayer gp = other.GetComponent<GroupPlayer>();
                Vector3 explosionDir = gp.transform.position - transform.position;
                gp.EffectSpeedMovement(explosionDir.normalized * explosionRatio);
                gp.SetAttacker(attackerId); // 设置攻击者Id
            }
        }

        DestroySelf();
    }

    IEnumerator CountDownTouchSelf(float time)
    {
        canTouchSelf = false;
        yield return new WaitForSeconds(time);
        canTouchSelf = true;
    }

    private void DestroySelf()
    {
        Destroy(gameObject, 5f); // 后期优化做修改
    }

    // 启动时添加粒子效果
    private void AddParticles()
    {
        Debug.Log("生成ball时的粒子效果");

        impactNormal = mTrans.forward;
        projectileParticleInstance = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticleInstance.transform.parent = transform;
        projectileParticleInstance.transform.localScale = Vector3.one;
        if (muzzleParticle)
        {
            muzzleParticleInstance = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticleInstance.transform.localScale = Vector3.one;
            Destroy(muzzleParticleInstance, 1.5f); // Lifetime of muzzle effect.
        }
    }


    private void SetSnowBallColor(Color color)
    {
        ParticleSystem[] pss = projectileParticleInstance.GetComponentsInChildren<ParticleSystem>();
        for (int i = 0; i < pss.Length; i++)
        {
            ParticleSystem ps = pss[i];
            //            if (ps.gameObject.name.Contains("FrostSpikes"))
            //            {
            ps.startColor = color;
            //            }
        }

    }

    // 碰撞到时，添加粒子特效
    private void ShowHitParticleEffects(Vector3 impactNormal)
    {

        impactParticleInstance = Instantiate(impactParticle, transform.position, Quaternion.FromToRotation(Vector3.up, impactNormal)) as GameObject;

        //yield WaitForSeconds (0.05);
        foreach (GameObject trail in trailParticles)
        {
            GameObject curTrail = transform.Find(projectileParticle.name + "/" + trail.name).gameObject;
            curTrail.transform.parent = null;
            curTrail.transform.localScale = Vector3.one;
            Destroy(curTrail, 3f);
        }
        Destroy(projectileParticleInstance, 3f);
        Destroy(impactParticleInstance, 5f);
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
