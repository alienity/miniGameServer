using System;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Collections.Generic;

public class Server : ICmdNetHandler
{
    public static Server Instance { get; private set; }

    //[Header("Movement direction")]
    //public float LeftJoystickHorizontal;
    //public float LeftJoystickVertical;
    [Header("User message")]
    public int panelID;             // 当前场景 用于控制状态切换
    public int groupID;
    public int userID;
    public int skill;
    public int joystickAvailable;  // 当前摇杆是否可以操控，0表示不行，1表示可以
    public int coolingTime;        // 剩余冷却时间
    //[Header("GameObject")]
    //public GameObject Canvas;
    //[Header("PlayerBotton")]
    //public Button Penguin_1P;
    //public Button Penguin_2P;
    //public Button Penguin_3P;
    //public Button Penguin_4P;
    //public Button Pig_1P;
    //public Button Pig_2P;
    //public Button Pig_3P;
    //public Button Pig_4P;
    //private Button[] buttons;

    Vector2 LeftJoystickVector2;
    Vector2 RightJoystickVector2;

    //NetworkServerSimple myServer;
    private const short MsgTypeServerMessageRecive = 100;
    private const short MsgTypeServerMessageSend = 101;
    private const short MsgTypeChooseRoleRecive = 102;
    const short ClientNum = 8;     //客户端数量+1个服务器=8

    public int[] channelId = new int[ClientNum];
    public int[] connectionId = new int[ClientNum];
    public int[] flagClientUsing = new int[ClientNum];
    private int portTCP = 5555;
    private int portBroadCastUDP = 6666;
    private int broadcastInterval = 50; // ms
    private int clientConnectNum = 0;   //客户端当前链接数

    public int ClientConnectNum
    {
        get
        {
            return clientConnectNum;
        }
    }

    public Queue<string> queueServerRcvCommands = new Queue<string>();     // 命令接收队列
    Queue<string> queueServerSendCommands = new Queue<string>();    // 命令发送队列
    public Queue<string> queueChooseRoleCommands = new Queue<string>();     // 命令接收队列
    private NetworkDiscovery ServerCast;

    private void Awake()
    {
        //保证场景转换的时候不会被清掉
        DontDestroyOnLoad(gameObject);

        Instance = this;

        panelID = 0;        //panel号 代表了状态机状态
        joystickAvailable = 1;
        coolingTime = 0;
        clientConnectNum = 0;
        broadcastInterval = 50; // ms
        for (int i = 0; i < ClientNum; ++i)
        {
            flagClientUsing[i] = -1; //未使用
        }
    }
    private void Start()
    {   
        SetupServer();
        BroadCast(portBroadCastUDP, broadcastInterval);
        //InitButtons();
    }

    private void Update()
    {
        if(clientConnectNum == ClientNum)
        {
            if(ServerCast.running)
                StopBroadCast();
        }
    }

    private void FixedUpdate()
    {
        //ServerSendMessage();
        ServerSendCommands();
    }

    public override Queue<string> GetCommands()
    {
        return queueServerRcvCommands;  // 返回当前接收的命令队列
    }

    public override void SendCommand(string cmd)
    {
        lock (queueServerSendCommands)
        {
            queueServerSendCommands.Enqueue(cmd);   //待发送命令入队
        }
    }

    /************************************************
            * 网络初始化
    ************************************************/
    /// <summary>
    /// 建立服务器
    /// </summary>
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
    /// <summary>
	/// 服务器端注册事件
	/// </summary>
	private void ServerRegisterHandler()
    {
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);
        NetworkServer.RegisterHandler(MsgType.Disconnect, OnServerDisconnected);
        //NetworkServer.RegisterHandler(MsgType.Error, OnServerError);
        NetworkServer.RegisterHandler(MsgTypeServerMessageRecive, ServerReciveMessage);
        NetworkServer.RegisterHandler(MsgTypeChooseRoleRecive, handleChooseRoleCommand);
    }

    /// <summary>
    /// 服务器通过UDP向客户端广播IP地址
    /// </summary>
    private void BroadCast(int port,int broadcastInterval)
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
    private void StopBroadCast()
    {
        ServerCast.StopBroadcast();
        Debug.Log("BroadCast Stop");
    }
    /************************************************
         * 收包
    ************************************************/
    private void OnServerConnected(NetworkMessage netmsg)
    {
        if (clientConnectNum >= ClientNum)  //不支持重连，只支持当前场次连接数为定值
        {
            return;
        }

        ++clientConnectNum; // 连机数
        Debug.Log("Client Connected to server: " + clientConnectNum + "connectionId:" + netmsg.conn.connectionId);
    }

    private void OnServerDisconnected(NetworkMessage netmsg)
    {
        --clientConnectNum; // 连机数
        int id = netmsg.conn.connectionId;
        int i;
        for (i=0;i<clientConnectNum && connectionId[i] != id; ++i) { }
        if (i != clientConnectNum)
            flagClientUsing[i] = -1;    // 标记为未使用

        Debug.Log("Client Disconnected to server" + clientConnectNum + "connectionId:" + netmsg.conn.connectionId);
    }

    private void ServerReciveMessage(NetworkMessage netmsg)
    {
        string input = netmsg.reader.ReadString();
        Debug.Log(input);

        //JObject JoysticMessage = JObject.Parse(input);
        ////panelID = (int)JoysticMessage["stageId"];       // 正在哪个panel
        //groupID = (int)JoysticMessage["gId"];           // 0~3表示四个组
        //userID = (int)JoysticMessage["uId"];            // 0或1， 0表示大将，1表示 马
        //LeftJoystickHorizontal = (float)JoysticMessage["direction"]["x"];
        //LeftJoystickVertical = (float)JoysticMessage["direction"]["y"];
        //skill = (int)JoysticMessage["skill"];
        //int connID = groupID * 2 + userID;       // 通过gId和uId计算出对应通道号
        //buttons[connID].interactable = true;     // 人物激活
        //if (flagClientUsing[connID] == -1) //******新人加入消息要广播出去*********
        //{
        //    Debug.Log("新增");
        //    flagClientUsing[connID] = 1;
        //    channelId[connID] = netmsg.channelId;
        //    connectionId[connID] = netmsg.conn.connectionId;
        //    JObject json = new JObject
        //    {
        //        {"commMode", 1},                                // 新增通讯模式字段，0表示单播，1表示组播
        //        {"stageId", panelID},                           // 正在哪个panel
        //        { "gId", groupID},                              // 组ID： 0~3表示四个组
        //        { "uId", userID},                               // 用户ID：0或1， 0表示大将，1表示 马
        //        { "joystickAvailable", joystickAvailable},      // 当前摇杆是否可以操控，0表示不行，1表示可以
        //        { "coolingTime", coolingTime}                   //剩余冷却时间
        //    };
        //    string output = json.ToString();//JsonConvert.SerializeObject(json);
        //    queueServerSendCommands.Enqueue(output);
        //    return;
        //}
        //else if(connectionId[connID] != netmsg.conn.connectionId) // 交替 消息要广播出去
        //{
        //    Debug.Log("交替");
        //    channelId[connID] = netmsg.channelId;
        //    connectionId[connID] = netmsg.conn.connectionId;
        //    JObject json = new JObject
        //    {
        //        {"commMode", 1},                                // 新增通讯模式字段，0表示单播，1表示组播
        //        {"stageId", panelID},                           // 正在哪个panel
        //        { "gId", groupID},                              // 组ID： 0~3表示四个组
        //        { "uId", userID},                               // 用户ID：0或1， 0表示大将，1表示 马
        //        { "joystickAvailable", joystickAvailable},      // 当前摇杆是否可以操控，0表示不行，1表示可以
        //        { "coolingTime", coolingTime}                   //剩余冷却时间
        //    };
        //    string output = json.ToString();//JsonConvert.SerializeObject(json);
        //    queueServerSendCommands.Enqueue(output);
        //}
        queueServerRcvCommands.Enqueue(input);          // 命令行入队
    }

    private void handleChooseRoleCommand(NetworkMessage netmsg)
    {
        string input = netmsg.reader.ReadString();
        Debug.Log(input);
        queueChooseRoleCommands.Enqueue(input);
        JObject JoysticMessage = JObject.Parse(input);
        JObject json = new JObject
        {
            {"commMode", 1},                                // 新增通讯模式字段，0表示单播，1表示组播
            {"stageId", 0},                                 // 正在哪个panel
            { "gId", (int)JoysticMessage["gId"]},           // 组ID： 0~3表示四个组
            { "uId", (int)JoysticMessage["uId"]},           // 用户ID：0或1， 0表示大将，1表示 马
            { "joystickAvailable", 1},      // 当前摇杆是否可以操控，0表示不行，1表示可以
            { "coolingTime", 0}                   //剩余冷却时间
        };
        string output = json.ToString();
        queueServerSendCommands.Enqueue(output);          // 命令行入队
    }

    /************************************************
         * 发包
    ************************************************/
    private void ServerSendMessage()
    {
        JObject json = new JObject
        {
            {"commMode", 1},                                // 新增通讯模式字段，0表示单播，1表示组播
            {"stageId", panelID},                           // 正在哪个panel
            { "gId", groupID},                              // 组ID： 0~3表示四个组
            { "uId", userID},                               // 用户ID：0或1， 0表示大将，1表示 马
            { "joystickAvailable", joystickAvailable},      // 当前摇杆是否可以操控，0表示不行，1表示可以
            { "coolingTime", coolingTime}                   //剩余冷却时间
        };
        string output = json.ToString();//JsonConvert.SerializeObject(json);

        RegisterHostMessage msg = new RegisterHostMessage();
        msg.m_Comment = output;

        //NetworkServer.SendToClient(connectionId[0], ServerMessageSend, msg);
        NetworkServer.SendToAll(MsgTypeServerMessageSend, msg);
    }

    private void ServerSendCommands()
    {
        if(queueServerSendCommands.Count != 0)
        {
            string cmd = (string)queueServerSendCommands.Peek();
            queueServerSendCommands.Dequeue();      //待发送命令出队发送

            RegisterHostMessage msg = new RegisterHostMessage();
            msg.m_Comment = cmd;
            Debug.Log("msg.m_Comment: " + msg.m_Comment);
            JObject Message = JObject.Parse(cmd);
            int commMode = (int)Message["commMode"]; //0表示单播，1表示组播
            panelID = (int)Message["stageId"];       // 正在哪个panel panelID仅通过服务器修改
            groupID = (int)Message["gId"];           // 0~3表示四个组
            userID = (int)Message["uId"];            // 0或1， 0表示大将，1表示 马

            //NetworkServer.SendToAll(ServerMessageSend, msg);

            int connID = groupID * 2 + userID;       // 通过gId和uId计算出对应通道号
            //if (connID < clientConnectNum)         // 这个判断不需要
            //{
                if (commMode == 0)       // 单播
                {
                    NetworkServer.SendToClient(connectionId[connID], MsgTypeServerMessageSend, msg);
                }
                else                    // 组播
                {
                    NetworkServer.SendToAll(MsgTypeServerMessageSend, msg);
                    Debug.Log("组播");
                }
            //}
        }
    }

    /************************************************
         * 按键
    ************************************************/
    //private void InitButtons()
    //{
    //    buttons = new Button[8];
    //    buttons[0] = Penguin_1P;
    //    buttons[1] = Pig_1P;
    //    buttons[2] = Penguin_2P; 
    //    buttons[3] = Pig_2P;
    //    buttons[4] = Penguin_3P; 
    //    buttons[5] = Pig_3P;
    //    buttons[6] = Penguin_4P; 
    //    buttons[7] = Pig_4P;
    //    foreach (Button button in buttons)
    //    {
    //        button.interactable = false;
    //    }
    //}

}

