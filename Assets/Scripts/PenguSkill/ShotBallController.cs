﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBallController : MonoBehaviour {

    // 控制的ball的对象
    public ShotBall ball;
    // Ball在发动后剩余的冷却时间
    protected float remainColdingTime;
    

    // 使用一个Ball
    public virtual void UseBall(int ownerId, Vector3 position, Quaternion rotation)
    {

    }

    // 剩余的能用的数量
    public virtual int RemainNums()
    {
        return 1;
    }

    // Ball是否可用
    public virtual int AvailableNow()
    {
        return 1;
    }

    // 剩余冷却时间
    public float RemainColdingTime()
    {
        return remainColdingTime;
    }
    

}
