using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShotBallController : MonoBehaviour {

    // 控制的ball的对象
    public ShotBall ball;
    // Ball在发动后剩余的冷却时间
    protected float remainColdingTime;
    // Ball的最大冷却时间
    [SerializeField]
    protected float maxColdingTime;
    // 最大蓄力时长
    public float maxChargeTime = 3;

    // 蓄力process的Id
    protected string processId;
    // 记录蓄力开始时刻
    protected float chargeStartTime = -1;
    // 记录蓄力到现在，当前时刻
    protected float chargeCurrentTime = -1;

    // 完成蓄力
    public bool chargeFinished { get; protected set; }
    // 攻击完成
    public bool attackFinished { get; protected set; }

    // 使用一个Ball
    public abstract void UseBall(int ownerId, Vector3 position, Quaternion rotation);

    public float HandleChargeAttack(string processId, float chargeCurrentTime, int touchId)
    {
        // 检查冷却时间
        if (remainColdingTime > 0) return 0;

        this.attackFinished = false;

        switch (touchId)
        {
            case 0: // 0表示charge
                {
                    // 判断充能
                    if (this.processId != processId)
                    {
                        ResetCharge();
                        this.processId = processId;
                        this.chargeStartTime = chargeCurrentTime;
                        this.chargeCurrentTime = chargeCurrentTime;
                    }
                    else
                    {
                        this.chargeCurrentTime = chargeCurrentTime;
                    }
                    this.chargeFinished = false;
                }
                break;
            case 1: // 1表示finish
                {
                    if (this.processId != processId)
                    {
                        ResetCharge();
                        return 0;
                    }
                    else
                    {
                        if (this.chargeFinished) return 0;

                        this.chargeCurrentTime = chargeCurrentTime;

                        this.chargeFinished = true;
                        this.processId = ""; // 完成充能后重置 技能按键过程id
                    }
                }
                break;
        }
        
        float chargePastTime = Mathf.Clamp(this.chargeCurrentTime - this.chargeStartTime, 0, this.maxChargeTime);
        return chargePastTime / maxChargeTime;
    }

    // 持续充能
    //public float HandleChargeAttack(float chargeStartTime, float chargeCurrentTime)
    //{
    //    //Debug.Log("chargeCurrentTime = " + chargeCurrentTime);

    //    if (chargeStartTime == 0) return 0;

    //    if ((remainColdingTime > 0) || (chargeStarted && this.chargeStartTime != chargeStartTime)
    //        || (!chargeStarted && chargeCurrentTime == 0))
    //    {
    //        ResetCharge();
    //        return 0;
    //    }
        
    //    if (!chargeStarted)
    //    {
    //        chargeStarted = true;
    //        this.chargeStartTime = chargeStartTime;
    //        if (chargeStartTime != chargeCurrentTime)
    //            this.chargeBiasTime = chargeCurrentTime - chargeStartTime;
    //    }

    //    if (chargeStarted && !chargeFinished)
    //    {
    //        this.chargeCurrentTime = chargeCurrentTime;
    //    }

    //    float chargedPastTime = this.chargeCurrentTime - (this.chargeBiasTime + this.chargeStartTime);

    //    if (chargedPastTime >= maxChargeTime)
    //    {
    //        chargeFinished = true;
    //    }

    //    Debug.Log("Internal Time = " + chargedPastTime);

    //    return Mathf.Lerp(0, 1, chargedPastTime / maxChargeTime);
    //}

    // 重置充能
    public void ResetCharge()
    {
        chargeStartTime = -1;
        chargeCurrentTime = -1;
        chargeFinished = false;
        attackFinished = true;

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
    
    // 最大蓄力时长
    public float MaxChargeTime()
    {
        return maxChargeTime;
    }

    // 最长冷却时间
    public float MaxColdingTime()
    {
        return maxColdingTime;
    }

    // 剩余冷却时间
    public float RemainColdingTime()
    {
        return remainColdingTime;
    }

}
