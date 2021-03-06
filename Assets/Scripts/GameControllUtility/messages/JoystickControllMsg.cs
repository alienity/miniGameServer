﻿using UnityEngine;
using UnityEngine.Networking;

public class JoystickControllMsg : MessageBase
{

    public int gId;
    public int uId;
    public Vector2 direction;
    public bool skill;
    public bool finish;

    public JoystickControllMsg(int gId, int uId, Vector2 direction, bool skill, bool finish)
    {
        this.gId = gId;
        this.uId = uId;
        this.direction = direction;
        this.skill = skill;
        this.finish = finish;
    }

    public JoystickControllMsg()
    {

    }

}
