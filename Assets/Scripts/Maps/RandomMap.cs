using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomMap : MonoBehaviour
{

    private int[] pos;

    private System.Random rand;
    public GameObject cloneObject;

    public int cloneNum;
    private int clonedNum;

    public List<Transform> reBorns;

    public float reBornCD;
    private float reTimes;

    void Start()
    {
        reTimes = -1; clonedNum = 0;

        pos = new int[10];
        rand = new System.Random();

        for (int i = 0; i < 9; i++) pos[i] = i;
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
    private void setMap(int idx)
    {
        if(cloneObject.tag =="FallStone")
        {
            GameObject cloneObjInstance = Instantiate(cloneObject, transform);
            cloneObjInstance.transform.position = reBorns[idx].position;
            getObject();
        }
    }
    //保持地图上 随机的物品数，由物品的 触发器函数调用
    public void getObject()
    {
        clonedNum--;
        reTimes = reBornCD;
    }
}
