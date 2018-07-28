
using UnityEngine.Networking;

public class ClientScenChangeUtil
{
    public static void ChangeClientStage(int connectionId, Stage stage)
    {
        
        StageTransferMsg stageSwitchMsg = new StageTransferMsg(stage);
        NetworkServer.SendToClient(connectionId, CustomMsgType.Stage, stageSwitchMsg);
    }
    public static void ChangeAllClientStage(Stage stage)
    {
        StageTransferMsg sceneTransferMsg = new StageTransferMsg(stage);
        NetworkServer.SendToAll(CustomMsgType.Stage, sceneTransferMsg);
    }

//    private string StageToScene(Stage stage)
//    {
//        string sceneName = null;
//        switch (stage)
//        {
//            case Stage.StartStage:
//                sceneName = "StartStage"
//            case Stage.ChoosingRoleStage:
//                sceneName = "ChooseRoleStage";
//                break;
//            case Stage.GammingStage:
//                sceneName = "GammingStage";
//                break;
//            case Stage.Prepare:
//                sceneName = "PrepareStage";
//                break;
//        }
//
//        return sceneName;
//    }
}