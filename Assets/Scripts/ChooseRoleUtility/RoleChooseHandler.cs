﻿using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Networking;

public class RoleChooseHandler : MonoBehaviour
{
    // 确认选择的玩家数
    [HideInInspector]
    public int ConfirmPlayerNums { get; private set; }

    // 等待开始游戏的时间
    public int countDownTime = 5;
    // 本次要选择的人数
    public int toNumberTransfer = 8;

    // 控制角色选择的UI控制器
    private RoleChoosingUIController roleChoosingUIController;
    // 不会终止的服务器
    private Server server;

    private void Start()
    {
        if (server == null)
            server = Server.Instance;
        if (roleChoosingUIController == null)
            roleChoosingUIController = FindObjectOfType<RoleChoosingUIController>();

        NetworkServer.RegisterHandler(CustomMsgType.Choose, OnReceiveChoose);
        NetworkServer.RegisterHandler(CustomMsgType.Confirm, OnPlayerCnfirm);
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
        result.stageId = 0;
        if (server.role2connectionID.ContainsKey(selectingGid * 2 + selectingUid)) // 要选择的角色已经不可用
        {
            result.succeed = false;
        }
        else // 要选择的角色可用
        {
            if (server.connectionID2role.ContainsKey(curConnectionID)) // 如果之前已经选择过角色
            {

                int oldRoleId = server.connectionID2role[curConnectionID];
                int oldGid = oldRoleId / 2,
                    oldUid = oldRoleId % 2;
                server.connectionID2role.Remove(curConnectionID);
                server.role2connectionID.Remove(oldRoleId);
                result.hasOld = true;
                SubstituteRole(selectingGid, selectingUid, oldGid, oldUid);
            }
            else // 如果之前没有选择过角色
            {
                SendRoleMessageToALl(new RoleStateMsg(selectingGid, selectingUid, false));
                roleChoosingUIController.SetButtonRoleSelected(selectingGid, selectingUid);
                result.hasOld = false;
            }

            // 给选择的用户发送成功选上的信息
            result.succeed = true;
            result.gid = selectingGid;
            result.uid = selectingUid;
            result.oldUid = curRequest.oldUid;
            result.oldGid = curRequest.oldGid;
        }
        // 新的角色和用户对应关系添加入记录中
        server.connectionID2role[curConnectionID] = selectingRoleId;
        server.role2connectionID[selectingRoleId] = curConnectionID;
        SendChooseMessage(result, curConnectionID);

        // 测试跳转
        //if (server.connections.Count == toNumberTransfer)
        //    StartCoroutine(CountDownToStartGame(countDownTime));
    }

    // 对玩家确定的回调
    public void OnPlayerCnfirm(NetworkMessage netmsg)
    {
        ConfirmPlayerNums += 1;
        if (ConfirmPlayerNums == toNumberTransfer)
            StartCoroutine(CountDownToStartGame(countDownTime));
    }

    IEnumerator CountDownToStartGame(int time)
    {
        yield return new WaitForSeconds(time);
        //server.StopBroadCast();
        SceneTransformer.Instance.TransferToNextScene();
    }

    // 替换原先选择的角色
    private void SubstituteRole(int selectingGid, int selectingUid, int oldGid, int oldUid)
    {
        roleChoosingUIController.SetButtonRoleAvailable(oldGid, oldUid);
        roleChoosingUIController.SetButtonRoleSelected(selectingGid, selectingUid);
        // 广播旧角色可用的信息
        RoleStateMsg oldRoleState = new RoleStateMsg(oldGid, oldUid, true);
        SendRoleMessageToALl(oldRoleState);
        // 这个角色被选择后，需要把该角色不可选的消息广播给所有用户
        RoleStateMsg roleState = new RoleStateMsg(selectingGid, selectingUid, false);
        SendRoleMessageToALl(roleState);
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