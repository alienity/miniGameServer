using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotBallController : MonoBehaviour {

    // 控制的ball的对象
    public ShotBall ball;
    // Ball在发动后剩余的冷却时间
    protected float remainColdingTime;
    // 蓄力时长
    public float maxChargeTime = 4;
    // 记录开始蓄力
    protected float chargeStartTime = -1;
    // 记录蓄力到现在，当前时刻
    protected float chargeCurrentTime = -1;
    // 完成蓄力
    protected bool chargeFinished = false;
    // 开始蓄力
    protected bool startCharge = false;

    // 使用一个Ball
    public abstract void UseBall(int ownerId, Vector3 position, Quaternion rotation);

    // 持续充能
    public float HandleChargeAttack(float chargeStartTime, float chargeCurrentTime)
    {
        if (!startCharge && this.chargeStartTime != chargeStartTime)
        {
            startCharge = true;
            this.chargeStartTime = chargeStartTime;
        }

        if (startCharge && !chargeFinished)
        {
            this.chargeCurrentTime = chargeCurrentTime;
        }

        return Mathf.Lerp(0, 1, (this.chargeCurrentTime - this.chargeStartTime) / maxChargeTime);
    }

    // 重置充能
    public void ResetCharge()
    {
        chargeStartTime = -1;
        chargeCurrentTime = -1;
        chargeFinished = false;
        startCharge = false;
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

    // 设置蓄力
    public void SetChargeAttackTime(float time = 0)
    {
        if (time > maxChargeTime) time = maxChargeTime;
        ball.SetChargeAttackTime(time);
    }

    // 剩余冷却时间
    public float RemainColdingTime()
    {
        return remainColdingTime;
    }
    
    // 最大蓄力时长
    public float MaxChargeTime()
    {
        return maxChargeTime;
    }

}
