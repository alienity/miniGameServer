using UnityEngine;
using System.Collections.Generic;

public class AttractiveForceBall : ShotBall
{

    // 移动速度
    public float flySpeed;
    // 持续时间
    public float flyTime;
    // 已经飞行时间
    private float fliedTime = 0;
    // 引力球半径
    public float atrRadus = 5;
    // 最大引力
    public float maxAttractForce;
    
    // 受影响对象列表
    private List<GroupPlayer> groupPlayers;

    // 自有对象
    private SphereCollider attractiveCollider;
    private Transform mTrans;

    // Use this for initialization
    void Start()
    {
        attractiveCollider = GetComponent<SphereCollider>();
        attractiveCollider.radius = atrRadus;

        mTrans = GetComponent<Transform>();
        groupPlayers = new List<GroupPlayer>();
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

        foreach (GroupPlayer gpObj in groupPlayers)
        {
            Vector3 attrFoceDir = mTrans.position - gpObj.groupTrans.position;
            attrFoceDir.y = 0;
            Vector3 attrForce = (atrRadus - attrFoceDir.magnitude) * attrFoceDir.normalized * maxAttractForce;
            gpObj.EffectForce(attrForce);
            gpObj.SetAttacker(attackerId); // 设置攻击者Id
        }
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
            if (clGp.gId == attackerId) return;

            if (!groupPlayers.Contains(clGp))
                groupPlayers.Add(clGp);
        }
    }
    
    private void OnTriggerExit(Collider collider)
    {
        if (collider.tag == "Player")
        {
            GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
            if (clGp.gId == attackerId) return;

            if (groupPlayers.Contains(clGp))
                groupPlayers.Remove(clGp);
        }
    }

    private void DestroySelf()
    {
        Destroy(gameObject); // 后期优化做修改
    }

}
