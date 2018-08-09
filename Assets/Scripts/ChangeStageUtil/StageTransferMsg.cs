using UnityEngine.Networking;
public class StageTransferMsg : MessageBase
{
    public int stage;
    // 只有在同一个房间里，服务器才能要求客户端切换场景
    public int roomId;

    public StageTransferMsg(Stage stage)
    {
        this.stage = (int)stage;
    }

    public StageTransferMsg()
    {
    }
}