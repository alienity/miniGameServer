using UnityEngine.Networking;

// 服务器告知客户端对应的角色是否可用
public class RoleStateMsg : MessageBase
{
    public int gid;
    public int uid;
    public bool available;

    public RoleStateMsg(int gid, int uid, bool available)
    {
        this.gid = gid;
        this.uid = uid;
        this.available = available;
    }

    public RoleStateMsg()
    {
    }
}