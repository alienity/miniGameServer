using System;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Security.Policy;

public class Server : MonoBehaviour
{

    public static Server Instance { get; private set; }

    // 连接客户端记录
    public HashSet<int> connections = new HashSet<int>();

    // connectionId到gId和uId的映射表, roleId = gId * 2 + uId
    public Dictionary<int, int> role2connectionID = new Dictionary<int, int>();
    public Dictionary<int, int> connectionID2role = new Dictionary<int, int>();
    public Dictionary<int, int> session2connection = new Dictionary<int, int>();
    public Dictionary<int, int> connection2session = new Dictionary<int, int>();
    public Dictionary<int, int> session2role = new Dictionary<int, int>();
    public HashSet<int> sessionIsConfirmed = new HashSet<int>();
    public HashSet<int> kownSessions = new HashSet<int>();

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
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    private void Start()
    {
        roleChooseHandler = RoleChooseHandler.Instance;
        // Todo  reconnectHandler需要在所有场景中存在，暂时挂载在server上了
        reconnectHandler = gameObject.AddComponent<ReConnectHandler>();

        SetupServer();

        BroadCast(portBroadCastUDP, broadcastInterval);
    }
    
    public void SetupServer()
    {
        if (!NetworkServer.active)
        {
            Debug.Log("setup server");
            ServerRegisterHandler();
            NetworkServer.Listen(portTCP);

            if (NetworkServer.active)
            {
                Debug.Log("Server setup ok.");
            }
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
        Debug.Log("client disconnected");
        connections.Remove(netmsg.conn.connectionId);
    }

    // 上线记录
    public void OnClientConnect(NetworkMessage netmsg)
    {
        Debug.Log("client connected");
        
        reconnectHandler.OnClientConnect(netmsg);
        connections.Add(netmsg.conn.connectionId);
        // 人数到达游戏人数后，发送消息给client切换到选人界面 
        if (stage == Stage.Prepare && connections.Count == roleChooseHandler.toNumberTransfer)
        {
            ClientScenChangeUtil.ChangeAllClientStage(Stage.ChoosingRoleStage);
            stage = Stage.ChoosingRoleStage;
        }
    }


}

