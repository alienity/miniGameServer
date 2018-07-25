using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ChargeSkillMsg : MessageBase
{

    public int gId;
    public int uId;
    public float chargeStartTime;   // 蓄力开始时刻
    public float chargeCurrentTime; // 蓄力当前时刻
    public bool chargeReturn;

    public ChargeSkillMsg(int gId, int uId, float chargeStartTime, float chargeCurrentTime, bool chargeReturn)
    {
        this.gId = gId;
        this.uId = uId;
        this.chargeStartTime = chargeStartTime;
        this.chargeCurrentTime = chargeCurrentTime;
        this.chargeReturn = chargeReturn;
    }

    public ChargeSkillMsg() { }

}