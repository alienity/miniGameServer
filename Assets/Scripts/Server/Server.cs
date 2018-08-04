using System;
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

    // 连接客户端记录
//    public HashSet<int> connections = new HashSet<int>();

    // connectionId到gId和uId的映射表, roleId = gId * 2 + uId
    public Dictionary<int, int> role2connectionID = new Dictionary<int, int>();
    public Dictionary<int, int> connectionID2role = new Dictionary<int, int>();
    public Dictionary<int, int> session2connection = new Dictionary<int, int>();
    public Dictionary<int, int> connection2session = new Dictionary<int, int>();
    public Dictionary<int, int> session2role = new Dictionary<int, int>();
    public HashSet<int> sessionIsConfirmed = new HashSet<int>();
    public HashSet<int> kownSessions = new HashSet<int>();
//    public HashSet<string> clientAddresses = new HashSet<string>();
    
    public Dictionary<int, string> session2name = new Dictionary<int, string>();

    // 服务器配置
    const short ClientNum = 8;     //客户端数量+1个服务器=8

    private int portTCP = 5555;
    private int portBroadCastUDP = 6666;
    private int broadcastInterval = 50;    // ms
    private int clientConnectNum = 0;

    private NetworkDiscovery ServerCast;
    private RoleChooseHandler roleChooseHandler;
    private ReConnectHandler reconnectHandler;



    public Stage stage = Stage.Prepare;
    
    private void Awake()
    {
        if(Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
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
        if (stage == Stage.Prepare)
        {
            int sessionId = connection2session[connectionId];
            kownSessions.Remove(sessionId);
            session2connection.Remove(sessionId);
            connection2session.Remove(sessionId);
            Debug.Log("client disconnected during prepare, sessions: " + kownSessions.Count);
        }
//        connections.Remove(netmsg.conn.connectionId);
        Debug.Log("client disconnected :" + kownSessions.Count);


    }

    // 上线记录
    public void OnClientConnect(NetworkMessage netmsg)
    {
        Debug.Log("client connected asddress" + netmsg.conn.address);
        
        reconnectHandler.OnClientConnect(netmsg);
//        connections.Add(netmsg.conn.connectionId);
        // 人数到达游戏人数后，发送消息给client切换到选人界面 
        if (stage == Stage.Prepare )
        {
            ClientScenChangeUtil.ChangeAllClientStage(Stage.Prepare);
            if (kownSessions.Count == roleChooseHandler.toNumberTransfer)
            {
                ClientScenChangeUtil.ChangeAllClientStage(Stage.ChoosingRoleStage);
                stage = Stage.ChoosingRoleStage;
                Debug.Log("client switch to choosingRoleStage");
            }

        }
    }


    public void ClearData()
    {
        role2connectionID.Clear();
        connectionID2role.Clear();
        session2connection.Clear();
        session2role.Clear();
        connection2session.Clear();
//        connections.Clear();
        kownSessions.Clear();
        sessionIsConfirmed.Clear();
        session2name.Clear();
    }


}

