using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionTreeManager : MonoBehaviour
{

    // 爆炸树原型
    public ExplosionTree explosionTree;
    // 重生延迟
    public float rebornDelay = 5;
    // 要生成南瓜的位置
    public List<Transform> explosionTrans;
    //// 管理所有的爆炸树
    private List<ExplosionTree> explosionTreeInstances;

    // 爆炸树的碰撞树的半长
    private Vector3 halfExtents;

    private struct TimeAndId {
        public float remainTime;
        public int pumpkinId;
    };

    private List<TimeAndId> tid;

    private void Start ()
    {
        halfExtents = explosionTree.GetComponent<BoxCollider>().size;
        explosionTreeInstances = new List<ExplosionTree>();
        for (int i = 0; i < explosionTrans.Count; ++i)
        {
            ExplosionTree et = InstantiateTree(i);
            explosionTreeInstances.Add(et);
        }
        tid = new List<TimeAndId>();
    }
	
	private void Update ()
    {
        for (int i = tid.Count-1; i >= 0; --i)
        {
            TimeAndId tai = tid[i];
            tai.remainTime -= Time.deltaTime;
            tid[i] = tai;
            if (tai.remainTime < 0 && BeableToReborn(explosionTrans[tai.pumpkinId]))
            {
                ExplosionTree et = InstantiateTree(tai.pumpkinId);
                explosionTreeInstances[tai.pumpkinId] = et;
                tid.Remove(tai);
            }
        }
    }

    private ExplosionTree InstantiateTree(int id)
    {
        ExplosionTree et = Instantiate(explosionTree, transform);
        et.transform.position = explosionTrans[id].position;
        et.pumpkinId = id;
        et.noticeFunc = (int pumpkinId) => {
            TimeAndId tai = new TimeAndId();
            tai.remainTime = rebornDelay;
            tai.pumpkinId = pumpkinId;
            this.tid.Add(tai);
        };
        return et;
    }

    private bool BeableToReborn(Transform pumpkinTrans)
    {
        RaycastHit m_Hit;
        if(Physics.BoxCast(pumpkinTrans.position, halfExtents, Vector3.down, out m_Hit))
        {
            if (m_Hit.collider.tag != "Plane")
                return false;
        }
        return true;
    }

    public void ChangeWeather()
    {

    }

}
