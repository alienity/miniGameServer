using UnityEngine.Networking;
public class StageTransferMsg : MessageBase
{
    public int stage;

    public StageTransferMsg(Stage stage)
    {
        this.stage = (int)stage;
    }

    public StageTransferMsg()
    {
    }
}