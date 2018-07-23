using UnityEngine.Networking;

public class ChooseRequestMsg : MessageBase
{
    public int gid;
    public int uid;
    public bool hasOld;
    public int oldGid;
    public int oldUid;

    public ChooseRequestMsg(int gid, int uid, bool hasOld, int oldGid, int oldUid)
    {
        this.gid = gid;
        this.uid = uid;
        this.hasOld = hasOld;
        this.oldGid = oldGid;
        this.oldUid = oldUid;
    }

    public ChooseRequestMsg()
    {
    }
}