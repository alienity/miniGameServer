using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;


// Todo 游戏结束后清空session，玩家回到开始界面，重新选择进入游戏进行连接
public class ReConnectHandler : MonoBehaviour
{
    private void Start()
    {

        NetworkServer.RegisterHandler(CustomMsgType.Session, OnReceiveSeesionResponse);
    }

    private Random random = new Random();

    
    /*
     * 在准备阶段客户端进行连接是正常情况，为每一个客户端分配一个sessionid用来标记客户端
     * 返回一个是否可以由ReConnectHandler完全处理，如果不需要其他人处理，返回true
     */
    public void OnClientConnect(NetworkMessage netmsg)
    {
        Debug.Log("client connected");
//        int clientConnectionId = netmsg.conn.connectionId;
        switch (DataSaveController.Instance.stage)
        {
            case Stage.Prepare:
                /*
                 * 分配sessionID，客户端会根据roomID进行判断是否切换到准备界面
                 */
                int sessionId = -1;
                // 避免生成一样的sessionid的情况（可能性很小）
                while (sessionId == -1 || DataSaveController.Instance.kownSessions.Contains(sessionId))
                {
                    sessionId = random.Next();
                }
                DistributeSession(netmsg.conn.connectionId, sessionId);
                DataSaveController.Instance.session2connection[sessionId] = netmsg.conn.connectionId;
                DataSaveController.Instance.connection2session[netmsg.conn.connectionId] = sessionId;
                DataSaveController.Instance.kownSessions.Add(sessionId);
//                ClientScenChangeUtil.ChangeAllClientStage(Stage.ConnectedToChooseRoomStage);
                break;
            /*
             * 在非选人阶段还收到连接请求，说明极其有可能是断线重连
             */
            case Stage.ChoosingRoleStage:
            case Stage.GammingStage:
                AskHasSession(netmsg.conn.connectionId);
                break;
            case Stage.GameOverStage:
                // 在游戏结束连接到客户端，让客户端回到开始界面
                NetworkServer.SendToClient(netmsg.conn.connectionId, CustomMsgType.Stage, new StageTransferMsg(Stage.StartStage));
                break;
            default:
                break;
        }
    }
    
    private void OnReceiveSeesionResponse(NetworkMessage netmsg)
    {
        SessionMsg sessionResponse = netmsg.ReadMessage<SessionMsg>();
        int sessionId = sessionResponse.sessionId;
        /*
         * 如果请求的重连玩家属于这场比赛，针对现在处于哪一阶段发送不同的指令
         */
        if (DataSaveController.Instance.kownSessions.Contains(sessionId))
        {
            DataSaveController.Instance.session2connection[sessionId] = netmsg.conn.connectionId;
            switch (DataSaveController.Instance.stage)
            {
//                case Stage.Prepare: // 在准备阶段不会发生请求重连的情况
//                    //Todo 准备阶段断线重连可能会有问题
//                    SessionMsg sessionDuringPrepare = new SessionMsg(false, false, 0, Stage.Prepare, false, 0, 0, null, null, null);
//
//                    SendSessionMsg(netmsg.conn.connectionId, sessionDuringPrepare);
//                    Debug.LogError("received session nmsg in prepare!");
//                    break;
                /*
                 * 断线重连成功，让玩家恢复状态
                 */
                case Stage.ChoosingRoleStage:
                    SessionMsg sessionDuringChoose = new SessionMsg(false, false, 0, 0, Stage.ChoosingRoleStage, false, 0, 0, DataSaveController.Instance.session2role, DataSaveController.Instance.sessionIsConfirmed, DataSaveController.Instance.session2name);
                    SendSessionMsg(netmsg.conn.connectionId, sessionDuringChoose);
                    Debug.Log("send roll back to choosing: " + sessionDuringChoose);

                    break;
                case Stage.GammingStage:
                    int roleId = DataSaveController.Instance.session2role[sessionId];
                    SessionMsg sessionDuringGamming = new SessionMsg(false, false, 0, 0, Stage.GammingStage, true, roleId/2, roleId%2, null, null, null);
                    SendSessionMsg(netmsg.conn.connectionId, sessionDuringGamming);
                    Debug.Log("send roll back to Game: " + sessionDuringGamming);
                    break;
                case Stage.GameOverStage:
                    NetworkServer.SendToClient(netmsg.conn.connectionId, CustomMsgType.Stage, new StageTransferMsg(Stage.StartStage));
                    break;
            }
        }
        else  // TODO 玩家持有的session可能是以前场次的
        {
            Debug.LogError("不知名的sessionID: " + sessionResponse);
            // 让玩家回到开始阶段
            NetworkServer.SendToClient(netmsg.conn.connectionId, CustomMsgType.Stage, new StageTransferMsg(Stage.StartStage));
            
        }
    }

    private void SendSessionMsg(int connectionId, SessionMsg sessionMsg)
    {
        NetworkServer.SendToClient(connectionId, CustomMsgType.Session, sessionMsg);
    }

    private void AskHasSession(int connectionId)
    {
        SessionMsg askSessionMsg = new SessionMsg(true, false, 0, 0, DataSaveController.Instance.stage, false, 0, 0, null, null, null);
        NetworkServer.SendToClient(connectionId, CustomMsgType.Session, askSessionMsg);
        Debug.Log("ask: " + askSessionMsg);
    }

    /*
     * 在服务器连接阶段，客户端进行连接时，服务器为每个客户端分配一个sessionId
     */
    private void DistributeSession(int connectionId, int sessionId)
    {
        SessionMsg sessionMsg = new SessionMsg(false, true, sessionId, DataSaveController.Instance.curRoomId,Stage.Prepare, false, 0, 0, null, null, null);
        NetworkServer.SendToClient(connectionId, CustomMsgType.Session, sessionMsg);
        Debug.Log("distribute session " + sessionMsg + "connectionId: " + connectionId );
    }
}