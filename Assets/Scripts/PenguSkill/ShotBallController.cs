using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // 蓄力音效
    public AudioClip chargeAudioClip;
    private AudioSource myAudioSource;

    // 完成蓄力
    public bool chargeFinished { get; protected set; }
    // 攻击完成
    public bool attackFinished { get; protected set; }

    // 更新冷却时间
    protected virtual void Update()
    {
        // 没有开始蓄力，就是释放结束了
        if (this.attackFinished)
        {
            if (remainColdingTime > 0)
                remainColdingTime -= Time.deltaTime;
        }
    }

    // 使用一个Ball
    public abstract void UseBall(int ownerId, Vector3 position, Quaternion rotation, Color ballColor);

    // 处理蓄力音效
    public void HandleChargeAudio(int touchId)
    {
        if (chargeAudioClip == null) return;
        if (myAudioSource == null)
        {
            myAudioSource = GetComponent<AudioSource>();
            if (myAudioSource == null)
                myAudioSource = gameObject.AddComponent<AudioSource>();
            myAudioSource.clip = chargeAudioClip;
        }
        if(touchId == 0) // 0 表示开始蓄力
        {
            if (!myAudioSource.isPlaying)
                myAudioSource.Play();
        }
        else             // 1 表示结束蓄力
        {
            if (myAudioSource.isPlaying)
                myAudioSource.Stop();
        }
    }

    // 处理蓄力
    public float HandleChargeAttack(string processId, float chargeCurrentTime, int touchId)
    {
        // 检查冷却时间
        if (remainColdingTime > 0) return 0;

        this.attackFinished = false;

        switch (touchId)
        {
            case 0: // 0表示charge
                {
                    HandleChargeAudio(0);
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
                    HandleChargeAudio(1);

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

    // 重置充能
    public void ResetCharge()
    {
        chargeStartTime = -1;
        chargeCurrentTime = -1;
        chargeFinished = false;
        attackFinished = true;

        HandleChargeAudio(1);
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
