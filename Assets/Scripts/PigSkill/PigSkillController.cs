﻿using UnityEngine;
using System.Collections;

public abstract class PigSkillController : MonoBehaviour
{
    // pig对象，后期更改
    protected PigPlayer pigPlayer;
    // pig刚体
    protected Rigidbody pigRd;

    // 猪在发动后剩余的冷却时间
    protected float remainColdingTime;
    // 技能最大冷却时间
    [SerializeField] protected float maxColdingTime;

    // 技能持续更新，只有部分技能会用到
    public virtual void ContinueUpdate(Transform pigCurTransform)
    {

    }

    // 使用技能
    public abstract void UseSkill(PigPlayer pigPlayer);

    // 剩余的能用的数量
    public virtual int RemainNums()
    {
        return 1;
    }

    // 返回技能可用
    public virtual int AvailableNow()
    {
        return 1;
    }

    // 重置技能
    public virtual void Reset()
    {
        remainColdingTime = 0;
    }

    // 剩余冷却时间
    public float RemainColdingTime()
    {
        return remainColdingTime;
    }

    // 最大冷却时间
    public float MaxColdingTime()
    {
        return maxColdingTime;
    }

}
