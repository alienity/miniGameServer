using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ConfirmChooseMsg : MessageBase
{

    public int gid;
    public int uid;
    public bool succeed;

    public ConfirmChooseMsg(int gid, int uid, bool succeed)
    {
        this.gid = gid;
        this.uid = uid;
        this.succeed = succeed;
    }

    public ConfirmChooseMsg()
    {
    }

}