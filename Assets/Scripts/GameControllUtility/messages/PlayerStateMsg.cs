﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerStateMsg : MessageBase
{

    public int gId;
    public int uId;
    public bool joystickAvailable;
    public float coolingTime;

    public PlayerStateMsg(int gid, int uid, bool joystickAvailable, float coolingTime)
    {
        this.gId = gid;
        this.uId = uid;
        this.joystickAvailable = joystickAvailable;
        this.coolingTime = coolingTime;
    }

    public PlayerStateMsg()
    {
    }

}
