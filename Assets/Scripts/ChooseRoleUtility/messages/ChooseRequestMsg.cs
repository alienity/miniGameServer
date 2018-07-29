using UnityEngine.Networking;

public class ChooseRequestMsg : MessageBase
{
    public int gid;
    public int uid;
    public string name;

    public ChooseRequestMsg(int gid, int uid, string name)
    {
        this.gid = gid;
        this.uid = uid;
        this.name = name;
    }

    public ChooseRequestMsg()
    {
    }

    public override string ToString()
    {
        return string.Format("Gid: {0}, Uid: {1}, Name: {2}", gid, uid, name);
    }
}

