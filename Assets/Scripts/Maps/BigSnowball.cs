using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSnowball : MonoBehaviour
{
    // 说明：大雪球推着玩家走，通过刚体组件
    public static BigSnowball Instance { get; private set; }
    // 移动速度
    public float Speed;
    // 死亡地点
    public Transform end;

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
    public void EndPointChange(Transform point)
    {
        end = point;
    }
}
