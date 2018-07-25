using UnityEngine.Networking;

public class ChooseResultMsg : MessageBase
{
    public bool succeed; // 0 -> 成功  -1 -> 失败
    public bool hasOld;
    public int gid;
    public int uid;
    public int oldGid;
    public int oldUid;

    public ChooseResultMsg(bool succeed, bool hasOld, int gid, int uid, int oldGid, int oldUid)
    {
        //this.stageId = stageId;
        this.succeed = succeed;
        this.hasOld = hasOld;
        this.gid = gid;
        this.uid = uid;
        this.oldGid = oldGid;
        this.oldUid = oldUid;
    }

    public ChooseResultMsg()
    {
    }
}