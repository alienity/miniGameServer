using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameControllHandler : MonoBehaviour
{

//    public static GameControllHandler Instance { get; private set; }

    // 存储所有操作命令的队列
    private Queue<JoystickControllMsg> cmdQueue = new Queue<JoystickControllMsg>();

    private Server server;

//    private void Awake()
//    {
//        Instance = this;
//    }
    
    private void Start()
    {
        if (server == null)
            server = Server.Instance;
		NetworkServer.RegisterHandler(CustomMsgType.GroupControll, OnReceiveControll);
        SceneTransferMsg sceneTransferMsg = new SceneTransferMsg("ChooseRoleScene", "GameScene");
        NetworkServer.SendToAll(CustomMsgType.ClientChange, sceneTransferMsg);
    }

    public void OnReceiveControll(NetworkMessage netmsg)
    {
        int curConnectionID = netmsg.conn.connectionId;
        JoystickControllMsg curControllMsg = netmsg.ReadMessage<JoystickControllMsg>();
        Debug.Log("receive control " + curControllMsg.gId + ", " + curControllMsg.uId);
        cmdQueue.Enqueue(curControllMsg);
    }
    
    //--------------------------------------------------------------------------------------------

    public void SendCommand(PlayerStateMsg psm)
    {
        int gId = psm.gId;
        int uId = psm.uId;
        int selectionId = gId * 2 + uId;
        if (!server.role2connectionID.ContainsKey(selectionId)) // 要选择的角色已经不可用
            return;
        int connectionId = server.role2connectionID[selectionId];
        NetworkServer.SendToClient(connectionId, CustomMsgType.GroupState, psm);
    }

    public Queue<JoystickControllMsg> GetCommands()
    {
        return cmdQueue;
    }
    
}
