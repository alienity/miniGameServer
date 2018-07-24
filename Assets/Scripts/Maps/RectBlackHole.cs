using UnityEngine;
using System.Collections.Generic;

public class RectBlackHole : MonoBehaviour
{
    //// 距离黑洞中心多远的时候死亡
    //public float deathRadius = 1;
    //// 引力半径
    //public float atrRadius = 5;
    //// 最大引力
    //public float maxAttractForce;

    //// 受影响对象列表
    //private List<GroupPlayer> groupPlayers;

    //// 自有对象
    //private SphereCollider attractiveCollider;
    //private Transform mTrans;

    //// Use this for initialization
    //void Start()
    //{
    //    attractiveCollider = GetComponent<SphereCollider>();
    //    attractiveCollider.radius = atrRadius;

    //    mTrans = GetComponent<Transform>();
    //    groupPlayers = new List<GroupPlayer>();
    //}

    //private void Update()
    //{
    //    attractiveCollider.radius = atrRadius;
    //}

    //private void FixedUpdate()
    //{

    //    foreach (GroupPlayer gpObj in groupPlayers)
    //    {
    //        Vector3 attrFoceDir = mTrans.position - gpObj.groupTrans.position;
    //        attrFoceDir.y = 0;

    //        if (attrFoceDir.magnitude < deathRadius)
    //        {
    //            gpObj.Die();
    //            continue;
    //        }

    //        Vector3 attrForce = (atrRadius - attrFoceDir.magnitude) * attrFoceDir.normalized * maxAttractForce;
    //        gpObj.EffectForce(attrForce);

    //    }
    //}

    //private void OnTriggerEnter(Collider collider)
    //{
    //    if (collider.tag == "Player")
    //    {
    //        Debug.Log(collider.name);
    //        GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
    //        if (!groupPlayers.Contains(clGp))
    //            groupPlayers.Add(clGp);
    //    }
    //}

    //private void OnTriggerExit(Collider collider)
    //{
    //    if (collider.tag == "Player")
    //    {
    //        GroupPlayer clGp = collider.GetComponent<GroupPlayer>();
    //        if (groupPlayers.Contains(clGp))
    //            groupPlayers.Remove(clGp);
    //    }
    //}
}
