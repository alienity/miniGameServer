using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class RoleChooseHandler : MonoBehaviour
{
    public static RoleChooseHandler Instance { get; private set; }

    // 等待开始游戏的时间
    public int countDownTime = 5;
    // 本次要选择的人数
    public int toNumberTransfer = 8;
    // 已经确定的玩家
//    public List<int> confirmedPlayers;


    // 控制角色选择的UI控制器
    private RoleChoosingUIController roleChoosingUIController;

    // 不会终止的服务器
    private Server server;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Start()
    {
        if (server == null)
            server = Server.Instance;
        if (roleChoosingUIController == null)
            roleChoosingUIController = FindObjectOfType<RoleChoosingUIController>();

        NetworkServer.RegisterHandler(CustomMsgType.Choose, OnReceiveChoose);
        NetworkServer.RegisterHandler(CustomMsgType.Confirm, OnPlayerCnfirm);
    }

    private void Update()
    {
        if (roleChoosingUIController.StartCanvas.activeInHierarchy)
        {
            roleChoosingUIController.ProgressBarPlay(server.connections.Count, toNumberTransfer);
        }
        if(!roleChoosingUIController.ChooseRoleCanvas.activeInHierarchy && server.connections.Count == toNumberTransfer)
        {
            roleChoosingUIController.changeCanvas();
            Debug.Log("changeCanvas");
        }
    }
    /*
     * 如果 <uid, gid> 已经被选中了，那么返回用户该角色不可再选择
     * 没有选中时，先检查该用户之前是否已经选择过角色
     *    1.如果没有选择过角色，将这个角色置为不可用，并把该角色不可用的信息进行广播
     *    2.如果选择过角色，那么将原来角色重新设置为可用，并把本轮选择角色不可用、原来角色可用的信息进行广播
     */
    public void OnReceiveChoose(NetworkMessage netmsg)
    {
        Debug.Log("OnReceiveChoose");
        int curConnectionID = netmsg.conn.connectionId;
        ChooseRequestMsg curRequest = netmsg.ReadMessage<ChooseRequestMsg>();
        int selectingGid = curRequest.gid,
            selectingUid = curRequest.uid,
            selectingRoleId = selectingGid * 2 + selectingUid;
        string playerName = curRequest.name;
        server.session2name[server.connection2session[netmsg.conn.connectionId]] = playerName;
        if (!server.role2connectionID.ContainsKey(selectingGid * 2 + selectingUid)) // 只有选择的人物可选，才更改玩家和人物的对应关系
        {
            if (server.connectionID2role.ContainsKey(curConnectionID)) // 如果之前已经选择过角色
            {

                int oldRoleId = server.connectionID2role[curConnectionID];
                server.session2role.Remove(server.connection2session[curConnectionID]);
                server.connectionID2role.Remove(curConnectionID);
                server.role2connectionID.Remove(oldRoleId);
            }
            else // 如果之前没有选择过角色
            {
                roleChoosingUIController.SetButtonRoleSelected(selectingGid, selectingUid);
            }
            // 新的角色和用户对应关系添加入记录中
            server.connectionID2role[curConnectionID] = selectingRoleId;
            server.role2connectionID[selectingRoleId] = curConnectionID;
            server.session2role[server.connection2session[curConnectionID]] = selectingRoleId;
        }
        
        SendRoleMessageToALl(new RoleStateMsg(server.session2role, server.sessionIsConfirmed, server.session2name));
        UpdateRoleChoosingUI();
    }

    private void UpdateRoleChoosingUI()
    {
        for (int i = 0; i < 8; i++)
        {
            roleChoosingUIController.SetButtonRoleAvailable(i / 2, i % 2);
        }

        foreach (KeyValuePair<int,int> connection2role in server.connectionID2role)
        {
            int gid = connection2role.Value / 2;
            int uid = connection2role.Value % 2;
            roleChoosingUIController.SetButtonRoleSelected(gid, uid);
        }

        foreach (int sessionId in server.sessionIsConfirmed)
        {
            int roleId = server.session2role[sessionId];
            roleChoosingUIController.SetButtonRoleLocked(roleId/2, roleId%2);
        }
        
        Dictionary<int, string> role2name = new Dictionary<int, string>();
        foreach (int sessionid in server.session2name.Keys)
        {
            string name = server.session2name[sessionid];
            int role = server.session2role[sessionid];
            role2name.Add(role, name);
        }
        roleChoosingUIController.SetRoleNames(role2name);

    }

    // 对玩家确定的回调
    public void OnPlayerCnfirm(NetworkMessage netmsg)
    {
        ConfirmChooseMsg ccm = netmsg.ReadMessage<ConfirmChooseMsg>();
        if (Server.Instance.sessionIsConfirmed.Contains(Server.Instance.connection2session[netmsg.conn.connectionId])) return;
        Server.Instance.sessionIsConfirmed.Add(Server.Instance.connection2session[netmsg.conn.connectionId]);
        Debug.Log("玩家确认!!! session2role " + JsonConvert.SerializeObject(Server.Instance.session2role));
        
        foreach (int session in Server.Instance.sessionIsConfirmed)
        {
            int roleId = Server.Instance.session2role[session];
            roleChoosingUIController.SetButtonRoleLocked(roleId/2, roleId%2);
        }
        if (server.sessionIsConfirmed.Count == toNumberTransfer)
        {
            roleChoosingUIController.CountDownTextSetActive();
            StartCoroutine(CountDownToStartGame(countDownTime));
			DataSaveController.Instance.playerNumber = Server.Instance.kownSessions.Count;
        }
        SendRoleMessageToALl(new RoleStateMsg(server.session2role, server.sessionIsConfirmed, server.session2name));
    }

    IEnumerator CountDownToStartGame(int time)
    {
        while (time > 0)
        {
            roleChoosingUIController.CountDownPlay(time);
            yield return new WaitForSeconds(1);
            --time;
        }
        //server.StopBroadCast();
        Server.Instance.stage = Stage.GammingStage;
        SceneTransformer.Instance.TransferToNextScene();
    }

    private void SendRoleMessageToALl(RoleStateMsg roleState)
    {
        NetworkServer.SendToAll(CustomMsgType.RoleState, roleState);
    }

    private void SendRoleMessage(RoleStateMsg roleState, int connectionID)
    {
        NetworkServer.SendToClient(connectionID, CustomMsgType.Choose, roleState);
    }

    private void SendChooseMessage(ChooseResultMsg result, int connectionId)
    {
        NetworkServer.SendToClient(connectionId, CustomMsgType.Choose, result);
        Debug.Log("sended " + result);
    }
}