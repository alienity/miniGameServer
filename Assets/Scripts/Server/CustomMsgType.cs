using UnityEngine.Networking;

public class CustomMsgType
{
    public const short Choose = MsgType.Highest + 1;           // 用于发送、接收选择角色的信息
    public const short Confirm = MsgType.Highest + 2;          // 发送确认选择信息
    public const short RoleState = MsgType.Highest + 3;        // 发送、接收某一个角色当前可用状态的信息
    public const short ClientChange = MsgType.Highest + 4;     // 通知手机端切换界面

    public const short GroupJoystick = MsgType.Highest + 5;    // 发送摇杆信息
    public const short GroupChargeSkill = MsgType.Highest + 6; // 发送蓄力信息
    public const short GroupRushSkill = MsgType.Highest + 8;   // 发送冲刺信息
    public const short GroupState = MsgType.Highest + 7;       // 发送到手机端状态信息

    public const short AdvanceControl = MsgType.Highest + 10; // 震动等消息调用


    public const short Session = MsgType.Highest + 11; // 断线重连功能
}