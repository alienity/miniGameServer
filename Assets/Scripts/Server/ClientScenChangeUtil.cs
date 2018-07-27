
using UnityEngine.Networking;

public class ClientScenChangeUtil
{
    public static void ChangeScence(int connectionId, Stage stage)
    {
        string nextScence = "";
        switch (stage)
        {
                case Stage.ChoosingRoleStage:
                    nextScence = "ChooseRoleStage";
                    break;
                case Stage.GammingStage:
                    nextScence = "GammingStage";
                    break;
                case Stage.Prepare:
                    nextScence = "Prepare Stage";
                    break;
        }
        SceneTransferMsg sceneTransferMsg = new SceneTransferMsg("", nextScence);
        NetworkServer.SendToClient(connectionId, CustomMsgType.ClientChange, sceneTransferMsg);
    }
    
    public static void ChangeScenceAll(Stage stage)
    {
        string nextScence = "";
        switch (stage)
        {
            case Stage.ChoosingRoleStage:
                nextScence = "ChooseRoleStage";
                break;
            case Stage.GammingStage:
                nextScence = "GammingStage";
                break;
            case Stage.Prepare:
                nextScence = "Prepare Stage";
                break;
        }
        SceneTransferMsg sceneTransferMsg = new SceneTransferMsg("", nextScence);
        NetworkServer.SendToAll(CustomMsgType.ClientChange, sceneTransferMsg);
    }
}