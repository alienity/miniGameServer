using UnityEngine.Networking;


public class ChooseResultMsg : MessageBase
 {
     public bool succeed; // 0 -> 成功  -1 -> 失败
     public int gid;
     public int uid;

     public ChooseResultMsg(bool succeed, int gid, int uid)
     {
         this.succeed = succeed;
         this.gid = gid;
         this.uid = uid;
     }

     public ChooseResultMsg()
     {
     }

     public override string ToString()
     {
         return string.Format("Succeed: {0}, Gid: {1}, Uid: {2}", succeed, gid, uid);
     }
 }