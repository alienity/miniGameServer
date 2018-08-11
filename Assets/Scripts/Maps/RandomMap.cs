using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomMap : MonoBehaviour
{
    private int[] pos;

    private System.Random rand = new System.Random();

    // 需要随机的对象
    public GameObject cloneObject;
    // 总共要出现的数量
    public int cloneNum;
    // 已经出现的数量
    private int clonedNum;
    // 重生的位置
    public List<Transform> reBorns;
    // 重生设定时间
    public float reBornCD;
    // 重生剩余时间
    private float reTimes;

    void Start()
    {
        reTimes = -1; clonedNum = 0;

        int posNum = reBorns.Count;
        pos = new int[posNum];
        for (int i = 0; i < posNum; i++)
            pos[i] = i;
    }

    void Update()
    {
        if (reTimes < 0)
        {

            if (clonedNum < cloneNum)
            {
                //在有效区里随机
                int len = reBorns.Count;
                int rpos = rand.Next(len - clonedNum-1);
                clonedNum++;
                setMap(pos[rpos]);
                //交换随机点到末尾
                int backPos = len - clonedNum-1;
                pos[rpos] ^= pos[backPos];
                pos[backPos] ^= pos[rpos];
                pos[rpos] ^= pos[backPos];
            }
            
        }
        else reTimes -= Time.deltaTime;
    }

    // 写法非常糟糕，有时间再改
    private void setMap(int idx)
    {
        if(cloneObject.tag =="FallStone")
        {
            GameObject cloneObjInstance = Instantiate(cloneObject, transform);
            cloneObjInstance.transform.position = reBorns[idx].position;
            getObject();
        }
        else if (cloneObject.tag == "SkillItem")
        {
            GameObject cloneObjInstance = Instantiate(cloneObject, transform);
            cloneObjInstance.transform.position = reBorns[idx].position;
            SkillItem bigSnowBallItem = cloneObjInstance.GetComponent<SkillItem>();
            bigSnowBallItem.noticeGenerator = getObject;
        }
    }
    
    //保持地图上 随机的物品数，由物品的 触发器函数调用
    public void getObject()
    {
        clonedNum--;
        reTimes = reBornCD;
    }
}
