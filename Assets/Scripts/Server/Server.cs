﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Security.Policy;
using Newtonsoft.Json;

public class Server : MonoBehaviour
{

    public static Server Instance
    {
        get
        {
            if (instance != null)
                return instance;

            instance = FindObjectOfType<Server>();

            if (instance != null)
                return instance;

            GameObject Server = new GameObject("Server");
            instance = Server.AddComponent<Server>();

            return instance;
        }
    }

    protected static Server instance;



    // 服务器配置
    const short ClientNum = 8;     //客户端数量+1个服务器=8

    private int portTCP = 5555;
    private int portBroadCastUDP = 6666;
    private int broadcastInterval = 50;    // ms
    private int clientConnectNum = 0;

    private NetworkDiscovery ServerCast;
    private RoleChooseHandler roleChooseHandler;
    private ReConnectHandler reconnectHandler;



    // 检测显示在否在检查进入游戏房间的人数
    private bool checkingHeadCount;

    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (roleChooseHandler == null)
        {
            roleChooseHandler = RoleChooseHandler.Instance;
        }

        if (reconnectHandler == null)
        {
            // Todo  reconnectHandler需要在所有场景中存在，暂时挂载在server上了
            reconnectHandler = gameObject.AddComponent<ReConnectHandler>();
        }

        if (!NetworkServer.active)
        {
            SetupServer();
            BroadCast(portBroadCastUDP, broadcastInterval);
        }
    }
    
    public void SetupServer()
    {
       
        Debug.Log("setup server");
        ServerRegisterHandler();
        NetworkServer.Listen(portTCP);

        if (NetworkServer.active)
        {
            Debug.Log("Server setup ok.");
        }
    }
	// 服务器端注册事件
	private void ServerRegisterHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Connect, OnClientConnect);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnClientDisConnect);


    }

    // 开启广播，寻找客户端
    public void BroadCast(int port,int broadcastInterval)
    {
        ServerCast = gameObject.AddComponent<NetworkDiscovery>();
        ServerCast.Initialize();
        //Debug.Log("cast start");
        ServerCast.broadcastPort = port;
        ServerCast.broadcastInterval = broadcastInterval;
        ServerCast.showGUI = false;
        ServerCast.StartAsServer();
        Debug.Log("BroadCast Start...");
    }

    // 停止广播
    public void StopBroadCast()
    {
        ServerCast.StopBroadcast();
    }

    // 断线记录
    public void OnClientDisConnect(NetworkMessage netmsg)
    {
        int connectionId = netmsg.conn.connectionId;
        if (DataSaveController.Instance.stage == Stage.Prepare)
        {
            int sessionId = DataSaveController.Instance.connection2session[connectionId];
            DataSaveController.Instance.kownSessions.Remove(sessionId);
            DataSaveController.Instance.session2connection.Remove(sessionId);
            DataSaveController.Instance.connection2session.Remove(sessionId);
            Debug.Log("client disconnected during prepare, sessions: " + DataSaveController.Instance.kownSessions.Count);
        }
//        connections.Remove(netmsg.conn.connectionId);
        Debug.Log("client disconnected :" + DataSaveController.Instance.kownSessions.Count);


    }

    // 上线记录
    public void OnClientConnect(NetworkMessage netmsg)
    {
        Debug.Log("client connected asddress" + netmsg.conn.address);
        
        reconnectHandler.OnClientConnect(netmsg);
        // 人数到达游戏人数后，发送消息给client切换到选人界面 
        if (DataSaveController.Instance.stage == Stage.Prepare )
        {
//            ClientScenChangeUtil.ChangeAllClientStage(Stage.ConnectedToChooseRoomStage);
            if (DataSaveController.Instance.kownSessions.Count == roleChooseHandler.toNumberTransfer && !checkingHeadCount)
            {
                StartCoroutine(CountDownToStart(3));
            }
        }
    }


    private IEnumerator CountDownToStart(int seconds)
    {
        checkingHeadCount = true;
        // 先等待一段时间检查连接人数（避免同一个人重复连接"冒充"多个人）
        yield return new WaitForSeconds(seconds);
        if (DataSaveController.Instance.kownSessions.Count == roleChooseHandler.toNumberTransfer)
        {
            RoleChoosingUIController roleChoosingUIController = FindObjectOfType<RoleChoosingUIController>();
            roleChoosingUIController.changeCanvas();
            ClientScenChangeUtil.ChangeAllClientStage(Stage.ChoosingRoleStage);
            DataSaveController.Instance.stage = Stage.ChoosingRoleStage;
            Debug.Log("进入选人界面:  sessionCount:"  + DataSaveController.Instance.kownSessions.Count + " numberToTransfer: " + roleChooseHandler.toNumberTransfer);

        }
        checkingHeadCount = false;
    }

    


}

