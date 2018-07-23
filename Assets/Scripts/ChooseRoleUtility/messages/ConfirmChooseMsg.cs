using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class ConfirmChooseMsg : MessageBase
{

    public int gid;
    public int uid;

    public ConfirmChooseMsg(int gid, int uid)
    {
        this.gid = gid;
        this.uid = uid;
    }

    public ConfirmChooseMsg()
    {
    }

}