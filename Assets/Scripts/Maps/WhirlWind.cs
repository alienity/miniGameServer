using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhirlWind : BoxEffects {

    public float windCenterR = 1;
    // 引力半径
    public float atrRadius = 5;
    public ParticleSystem windParticle;
    // 最大引力
    public float maxAttractForce;

    // 受影响对象列表
    private List<GroupPlayer> groupPlayers;

    // 自有对象
    private SphereCollider attractiveCollider;
    private Transform mTrans;
    private BigSnowBallManager bigSnowBallManager;

    // 风音效
    public AudioClip windAudioClip;

    private AudioSource windAudioSource;

    // Use this for initialization
    void Start()
    {
        bigSnowBallManager = BigSnowBallManager.Instance;
        attractiveCollider = GetComponent<SphereCollider>();
        attractiveCollider.radius = atrRadius;

        mTrans = GetComponent<Transform>();
        groupPlayers = new List<GroupPlayer>();

        windParticle.Play(true);
        windParticle.transform.position = transform.position;

        windAudioSource = GetComponent<AudioSource>();
        windAudioSource.clip = windAudioClip;
        windAudioSource.Play();
    }

    private void Update()
    {
        attractiveCollider.radius = atrRadius;
    }

    private void FixedUpdate()
    {
        mTrans.position = Vector3.MoveTowards(mTrans.position, end.position, Speed * Time.deltaTime);
        if ((mTrans.position - end.position).magnitude <= 0.1f)
        {
            bigSnowBallManager.IsReBornBigSnowBall(true);
            Destroy(gameObject);
        }
        foreach (GroupPlayer gpObj in groupPlayers)
        {
            Vector3 attrFoceDir = mTrans.position - gpObj.groupTrans.position;
            attrFoceDir.y = 0;

            Vector3 attrVelocity = (atrRadius - attrFoceDir.magnitude) * attrFoceDir.normalized * maxAttractForce * 0.01f;

            //自然规律
            if (attrFoceDir.magnitude < windCenterR)
            {
                continue;
            }
            gpObj.EffectSpeedMovement(attrVelocity);
        }
    }

    // 更改运动轨迹结束地点
    //public override void EndPointChange(Transform point)
    //{
    //    end = point;
    //}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
            if (!groupPlayers.Contains(clGp))
                groupPlayers.Add(clGp);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
            if (groupPlayers.Contains(clGp))
                groupPlayers.Remove(clGp);
        }
    }

}
