using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowball : BoxEffects
{
    // 说明：大雪球推着玩家走，通过刚体组件
    public static BigSnowball Instance { get; private set; }
    //// 移动速度
    //public float Speed;
    //// 死亡地点
    //public Transform end;

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
    private BigSnowBallManager bigSnowBallManager;
    private void Awake()
    {
        Instance = this;
        bigSnowBallManager = BigSnowBallManager.Instance;
    }
    private void Start ()
    {
        mTrans = gameObject.GetComponent<Transform>();
        AddParticles();
        //end = gameObject.GetComponentInParent<Transform>();
    }

    private void FixedUpdate()
    {
        mTrans.position = Vector3.MoveTowards(mTrans.position, end.position, Speed * Time.deltaTime);
        if((mTrans.position-end.position).magnitude <= 0.1f)
        {
            bigSnowBallManager.IsReBornBigSnowBall(true);
            Destroy(gameObject);
        }
    }

    // 更改运动轨迹结束地点
    //public override void EndPointChange(Transform point)
    //{
    //    end = point;
    //}

    // 启动时添加粒子效果
    private void AddParticles()
    {
        Debug.Log("生成大雪球时的粒子效果");

        impactNormal = mTrans.forward;
        projectileParticleInstance = Instantiate(projectileParticle, transform.position, transform.rotation) as GameObject;
        projectileParticleInstance.transform.parent = transform;
        projectileParticleInstance.transform.localScale = Vector3.one * 3f;
        if (muzzleParticle)
        {
            muzzleParticleInstance = Instantiate(muzzleParticle, transform.position, transform.rotation) as GameObject;
            muzzleParticleInstance.transform.localScale = Vector3.one;
            Destroy(muzzleParticleInstance, 1.5f); // Lifetime of muzzle effect.
        }
    }
}
