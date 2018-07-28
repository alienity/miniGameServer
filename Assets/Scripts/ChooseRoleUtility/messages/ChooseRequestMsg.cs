using UnityEngine.Networking;

public class ChooseRequestMsg : MessageBase
{
    public int gid;
    public int uid;

    public ChooseRequestMsg(int gid, int uid)
    {
        this.gid = gid;
        this.uid = uid;
    }

    public ChooseRequestMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("Gid: {0}, Uid: {1}", gid, uid);
    }
}