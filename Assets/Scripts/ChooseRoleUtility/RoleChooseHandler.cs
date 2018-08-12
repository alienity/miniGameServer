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
            roleChoosingUIController.ProgressBarPlay(DataSaveController.Instance.kownSessions.Count, toNumberTransfer);
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
//        int curConnectionID = netmsg.conn.connectionId;
        ChooseRequestMsg curRequest = netmsg.ReadMessage<ChooseRequestMsg>();
        int selectingRoleId = curRequest.gid * 2 + curRequest.uid,
            selectingSessionId = curRequest.sessionId;
        string playerName = curRequest.name;
        DataSaveController.Instance.session2name[selectingSessionId] = playerName;
        if (!DataSaveController.Instance.session2role.ContainsValue(selectingRoleId)) // 只有选择的人物可选，才更改玩家和人物的对应关系
        {
            if (DataSaveController.Instance.session2role.ContainsKey(selectingSessionId)) // 如果之前已经选择过角色
            {
                int oldRoleId = DataSaveController.Instance.session2role[selectingSessionId];
                DataSaveController.Instance.session2role.Remove(selectingSessionId);
                DataSaveController.Instance.role2session.Remove(oldRoleId);
            }
            else // 如果之前没有选择过角色
            {
                roleChoosingUIController.SetButtonRoleSelected(curRequest.gid, curRequest.uid);
            }
            // 新的角色和用户对应关系添加入记录中
            DataSaveController.Instance.session2role[selectingSessionId] = selectingRoleId;
            DataSaveController.Instance.role2session[selectingRoleId] = selectingSessionId;
        }

        var roleStatesMsg = new RoleStateMsg(DataSaveController.Instance.session2role, DataSaveController.Instance.sessionIsConfirmed, DataSaveController.Instance.session2name);
        SendRoleMessageToALl(roleStatesMsg);
        Debug.Log("send " + roleStatesMsg);
        UpdateRoleChoosingUI();
    }

    private void UpdateRoleChoosingUI()
    {
        for (int i = 0; i < 8; i++)
        {
            roleChoosingUIController.SetButtonRoleAvailable(i / 2, i % 2);
        }

        foreach (KeyValuePair<int,int> session2role in DataSaveController.Instance.session2role)
        {
            int gid = session2role.Value / 2;
            int uid = session2role.Value % 2;
            roleChoosingUIController.SetButtonRoleSelected(gid, uid);
        }

        foreach (int sessionId in DataSaveController.Instance.sessionIsConfirmed)
        {
            int roleId = DataSaveController.Instance.session2role[sessionId];
            roleChoosingUIController.SetButtonRoleLocked(roleId/2, roleId%2);
        }
        
        Dictionary<int, string> role2name = new Dictionary<int, string>();
        foreach (int sessionid in DataSaveController.Instance.session2name.Keys)
        {
            string name = DataSaveController.Instance.session2name[sessionid];
            int role = DataSaveController.Instance.session2role[sessionid];
            role2name.Add(role, name);
        }
        roleChoosingUIController.SetRoleNames(role2name);

    }

    // 对玩家确定的回调
    public void OnPlayerCnfirm(NetworkMessage netmsg)
    {
        /*
         * 更新sessionIsConfirmed变量
         */
        ConfirmChooseMsg ccm = netmsg.ReadMessage<ConfirmChooseMsg>();
        int selectingRoleId = ccm.gid * 2 + ccm.uid;
        int playerSessionId = DataSaveController.Instance.connection2session[netmsg.conn.connectionId];
        if (DataSaveController.Instance.session2confirm.ContainsKey(playerSessionId) || 
            DataSaveController.Instance.session2confirm.ContainsValue(selectingRoleId))
        {
//            SendConfirmMsg(new ConfirmChooseMsg(ccm.gid, ccm.uid, false), netmsg.conn.connectionId);
            return;
        }
        DataSaveController.Instance.sessionIsConfirmed.Add(playerSessionId);
        var roleStatesMsg = new RoleStateMsg(DataSaveController.Instance.session2role, DataSaveController.Instance.sessionIsConfirmed, DataSaveController.Instance.session2name);
        SendRoleMessageToALl(roleStatesMsg);
//        SendConfirmMsg(new ConfirmChooseMsg(ccm.gid, ccm.uid, true), netmsg.conn.connectionId);
//        Debug.Log("玩家确认!!! session2role " + JsonConvert.SerializeObject(DataSaveController.Instance.session2role));
        
        /*
         * 2. 设置button样式
         */
        foreach (int session in DataSaveController.Instance.sessionIsConfirmed)
        {
            int roleId = DataSaveController.Instance.session2role[session];
            roleChoosingUIController.SetButtonRoleLocked(roleId/2, roleId%2);
        }
        if (DataSaveController.Instance.sessionIsConfirmed.Count == toNumberTransfer)
        {
            roleChoosingUIController.CountDownTextSetActive();
            StartCoroutine(CountDownToStartGame(countDownTime));
			DataSaveController.Instance.playerNumber = DataSaveController.Instance.kownSessions.Count;
        }
        SendRoleMessageToALl(new RoleStateMsg(DataSaveController.Instance.session2role, DataSaveController.Instance.sessionIsConfirmed, DataSaveController.Instance.session2name));
    }

    IEnumerator CountDownToStartGame(int time)
    {
        while (time > 0)
        {
            roleChoosingUIController.CountDownPlay(time);
            yield return new WaitForSeconds(1);
            --time;
        }
        roleChoosingUIController.CountDownPlay(time);
        //server.StopBroadCast();
        DataSaveController.Instance.stage = Stage.GammingStage;
        SceneTransformer.Instance.TransferToNextScene();
    }

    private void SendRoleMessageToALl(RoleStateMsg roleState)
    {
        NetworkServer.SendToAll(CustomMsgType.RoleState, roleState);
    }

    private void SendConfirmMsg(ConfirmChooseMsg confirmChooseMsg, int connectionId)
    {
        NetworkServer.SendToClient(connectionId, CustomMsgType.Confirm, confirmChooseMsg);

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