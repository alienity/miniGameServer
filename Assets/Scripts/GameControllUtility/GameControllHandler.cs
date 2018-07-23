using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GameControllHandler : MonoBehaviour
{

    // 存储所有操作命令的队列
    private Queue<JoystickControllMsg> cmdQueue = new Queue<JoystickControllMsg>();

    private Server server;

    private void Start()
    {
        if (server == null)
            server = Server.Instance;

        NetworkServer.RegisterHandler(CustomMsgType.GroupControll, OnReceiveControll);

    }
    
    public void OnReceiveControll(NetworkMessage netmsg)
    {
        int curConnectionID = netmsg.conn.connectionId;
        JoystickControllMsg curControllMsg = netmsg.ReadMessage<JoystickControllMsg>();
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
