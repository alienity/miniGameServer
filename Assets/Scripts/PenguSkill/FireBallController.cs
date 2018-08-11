﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallController : ShotBallController
{
    
    void Start()
    {
        if (ball == null)
            ball = FindObjectOfType<SnowBall>();
        remainColdingTime = 0;
    }

    void Update()
    {
        // 没有开始蓄力，就是释放结束了
        if (this.attackFinished)
        {
            if (remainColdingTime > 0)
                remainColdingTime -= Time.deltaTime;
        }
    }

    // 充能结束后释放
    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation, Color ballColor)
    {
        if (0 == AvailableNow()) return;
        float chargedPastTime = this.chargeCurrentTime - this.chargeStartTime;
        //ball.SpawnBall(ownerId, position, rotation, Mathf.Clamp(chargedPastTime, 0, maxChargeTime));
        FireBall snowBall = Instantiate(ball, position, rotation) as FireBall;
        snowBall.InitiateBall(ownerId, Mathf.Clamp(chargedPastTime, 0, maxChargeTime), ballColor);
        remainColdingTime = maxColdingTime;
        ResetCharge();
    }

    // 剩余的能用的ball的数量
    public override int RemainNums()
    {
        return 1;
    }

    // 当前是否可以使用Ball
    public override int AvailableNow()
    {
        //Debug.Log(remainColdingTime);
        if (remainColdingTime <= 0)
            return 1;
        return 0;
    }
}
