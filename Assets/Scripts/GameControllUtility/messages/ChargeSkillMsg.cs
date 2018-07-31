using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChargeSkillMsg : MessageBase
{

    public int gId;
    public int uId;
    public string processId;           // 记录按键的一个过程
    //public float chargeStartTime;   // 蓄力开始时刻
    public float chargeCurrentTime; // 蓄力当前时刻
    public int touchId;               // 0表示charge  1表示finish
    //public bool chargeReturn;

    public ChargeSkillMsg(int gId, int uId, string processId, float chargeCurrentTime, int touchId)
    {
        this.gId = gId;
        this.uId = uId;
        this.processId = processId;
        this.chargeCurrentTime = chargeCurrentTime;
        this.touchId = touchId;
    }

    //public ChargeSkillMsg(int gId, int uId, float chargeStartTime, float chargeCurrentTime, bool chargeReturn)
    //{
    //    this.gId = gId;
    //    this.uId = uId;
    //    this.chargeStartTime = chargeStartTime;
    //    this.chargeCurrentTime = chargeCurrentTime;
    //    this.chargeReturn = chargeReturn;
    //}

    public ChargeSkillMsg() { }

}