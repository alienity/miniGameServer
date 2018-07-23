﻿using UnityEngine.Networking;

public class CustomMsgType
{
    public const short Choose = MsgType.Highest + 1; // 用于发送、接收选择角色的信息
    public const short Confirm = MsgType.Highest + 2;
    public const short RoleState = MsgType.Highest + 3;  // 发送、接收某一个角色当前可用状态的信息

    public const short GroupControll = MsgType.Highest + 10; // 发送和接受队伍和角色的控制信息
    public const short GroupState = MsgType.Highest + 11; // 发送到手机端状态信息

}