using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class RoleChooseHandler : MonoBehaviour
{
    public static RoleChooseHandler Instance { get; private set; }


    
    // 确认选择的玩家数
    [HideInInspector]
    public int ConfirmPlayerNums { get; private set; }

    // 等待开始游戏的时间
    public int countDownTime = 5;
    // 本次要选择的人数
    public int toNumberTransfer = 8;
    // 已经确定的玩家
    public List<int> confirmedPlayers;


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
        if (confirmedPlayers == null)
            confirmedPlayers = new List<int>();

        ConfirmPlayerNums = 0;

        NetworkServer.RegisterHandler(CustomMsgType.Choose, OnReceiveChoose);
        NetworkServer.RegisterHandler(CustomMsgType.Confirm, OnPlayerCnfirm);
    }

    private void Update()
    {
        if (roleChoosingUIController.StartCanvas.activeInHierarchy)
        {
            roleChoosingUIController.ProgressBarPlay(server.connections.Count, toNumberTransfer);
        }
        if(server.connections.Count == toNumberTransfer)
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
        ChooseResultMsg result = new ChooseResultMsg();
        int selectingGid = curRequest.gid,
            selectingUid = curRequest.uid,
            selectingRoleId = selectingGid * 2 + selectingUid;
        if (server.role2connectionID.ContainsKey(selectingGid * 2 + selectingUid)) // 要选择的角色已经不可用
        {
            result.succeed = false;
        }
        else // 要选择的角色可用
        {
            if (server.connectionID2role.ContainsKey(curConnectionID)) // 如果之前已经选择过角色
            {

                int oldRoleId = server.connectionID2role[curConnectionID];
                server.connectionID2role.Remove(curConnectionID);
                server.role2connectionID.Remove(oldRoleId);
            }
            else // 如果之前没有选择过角色
            {
                roleChoosingUIController.SetButtonRoleSelected(selectingGid, selectingUid);
            }

            // 给选择的用户发送成功选上的信息
            result.succeed = true;
            result.gid = selectingGid;
            result.uid = selectingUid;
        }
        // 新的角色和用户对应关系添加入记录中
        server.connectionID2role[curConnectionID] = selectingRoleId;
        server.role2connectionID[selectingRoleId] = curConnectionID;
        SendRoleMessageToALl(new RoleStateMsg(server.connectionID2role));
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
    }

    // 对玩家确定的回调
    public void OnPlayerCnfirm(NetworkMessage netmsg)
    {
        ConfirmChooseMsg ccm = netmsg.ReadMessage<ConfirmChooseMsg>();
        int connection = ccm.gid * 2 + ccm.uid;
        if (confirmedPlayers.Contains(connection)) return;
        confirmedPlayers.Add(connection);
        Debug.Log("玩家确认!!!");
        ConfirmPlayerNums += 1;
        if (ConfirmPlayerNums == toNumberTransfer)
        {
            roleChoosingUIController.CountDownTextSetActive();
            StartCoroutine(CountDownToStartGame(countDownTime));
			DataSaveController.Instance.playerNumber = confirmedPlayers.Count;
        }
    }

    IEnumerator CountDownToStartGame(int time)
    {
        while (time >= 0)
        {
            roleChoosingUIController.CountDownPlay(time);
            yield return new WaitForSeconds(1);
            --time;
        }
        //server.StopBroadCast();
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