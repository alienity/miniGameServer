using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableSnowBallController : SnowBallController
{

    // 剩余可用的ball的数量
    public int remainBallNums = 5;

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
    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        if (0 == AvailableNow()) return;
        float chargedPastTime = this.chargeCurrentTime - this.chargeStartTime;
        SnowBall snowBall = Instantiate(ball, position, rotation) as SnowBall;
        snowBall.InitiateBall(ownerId, Mathf.Clamp(chargedPastTime, 0, maxChargeTime));
        remainColdingTime = maxColdingTime;
        remainBallNums -= 1;
        ResetCharge();
    }

    // 剩余的能用的ball的数量
    public override int RemainNums()
    {
        return remainBallNums;
    }

}
