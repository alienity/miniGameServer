﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowBallController : ShotBallController
{

    //// Ball的冷却时间
    //public float snowBallColdingTime;
    //// Ball的指向箭头
    //public Sprite snowArrow;

    void Start () {
        if (ball == null)
            ball = FindObjectOfType<SnowBall>();
        remainColdingTime = 0;
    }

    // 充能结束后释放
    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation, Color ballColor)
    {
        if (0 == AvailableNow()) return;
        float chargedPastTime = this.chargeCurrentTime - this.chargeStartTime;
        //ball.SpawnBall(ownerId, position, rotation, Mathf.Clamp(chargedPastTime, 0, maxChargeTime));
        SnowBall snowBall = Instantiate(ball, position, rotation) as SnowBall;
        snowBall.InitiateBall(ownerId, Mathf.Clamp(chargedPastTime, 0, maxChargeTime), ballColor, ShotBall.BallType.SnowBall);
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
