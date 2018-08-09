using UnityEngine.Networking;

public class ChooseRequestMsg : MessageBase
{
    // 房间Id, 表示每一场游戏
    public int roomId;
    public int gid;
    public int uid;
    // 玩家的名字
    public string name;

    public ChooseRequestMsg(int roomId, int gid, int uid, string name)
    {
        this.roomId = roomId;
        this.gid = gid;
        this.uid = uid;
        this.name = name;
    }

    public ChooseRequestMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("RoomId: {0}, Gid: {1}, Uid: {2}, Name: {3}", roomId, gid, uid, name);
    }
}

