using System.Collections;
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
	
	void Update () {
        // 没有开始蓄力，就是释放结束了
        if (!chargeStarted)
        {
            if (remainColdingTime > 0)
                remainColdingTime -= Time.deltaTime;
        }

        if (chargeStarted && !chargeFinished)
        {
            float chargedPastTime = chargeCurrentTime - chargeCurrentTime;
            if (chargedPastTime >= maxChargeTime)
            {
                chargeFinished = true;
            }
        }

    }

    // 充能结束后释放
    public override void UseBall(int ownerId, Vector3 position, Quaternion rotation)
    {
        if (0 == AvailableNow()) return;
        if (!chargeStarted) return; // 如果没有开始蓄力，而接受到了蓄力结束，就直接忽略掉
        float chargedPastTime = this.chargeCurrentTime - (this.chargeBiasTime + this.chargeStartTime);
        ball.SpawnBall(ownerId, position, rotation, Mathf.Clamp(chargedPastTime, 0, maxChargeTime));
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
