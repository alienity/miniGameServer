using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallStoneRandomMap : MonoBehaviour {

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

    // 场上最大火球数
    public int maxFireBallItemNumbers;
    // 火球物品数
    private int curFireBallItemNumbers;

    void Start()
    {
        reTimes = -1; clonedNum = 0;

        curFireBallItemNumbers = 0;

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
                int rpos = rand.Next(len - clonedNum - 1);
                clonedNum++;
                setMap(pos[rpos]);
                //交换随机点到末尾
                int backPos = len - clonedNum - 1;
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
        if (cloneObject.tag == "FallStone")
        {
            GameObject cloneObjInstance = Instantiate(cloneObject, transform);
            FallStone fallStoneInstance = cloneObjInstance.GetComponent<FallStone>();
            fallStoneInstance.transform.position = reBorns[idx].position;
            fallStoneInstance.fallStoneRandomMap = this;
            getObject();
        }
    }
    
    // 增加火球数量
    public void IncreaseFireBallNumbers()
    {
        curFireBallItemNumbers += 1;
    }

    // 减少火球数量
    public void DecreaseFireBallNumbers()
    {
        curFireBallItemNumbers -= 1;
    }

    // 能够生成火球
    public bool BeableToInstantiateFireBall()
    {
        return curFireBallItemNumbers < maxFireBallItemNumbers;
    }

    //保持地图上 随机的物品数，由物品的 触发器函数调用
    public void getObject()
    {
        clonedNum--;
        reTimes = reBornCD;
    }
}
