using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ChooseRoleUIController : MonoBehaviour {
    [Header("PlayerBotton")]
    public Button Penguin_1P;
    public Button Penguin_2P;
    public Button Penguin_3P;
    public Button Penguin_4P;
    public Button Pig_1P;
    public Button Pig_2P;
    public Button Pig_3P;
    public Button Pig_4P;
    private Button[] buttons;

    [Header("Platform")]
    public GameObject Network;

    [Header("User message")]
    public int panelID;             // 当前场景 用于控制状态切换
    public int groupID;
    public int userID;

    Queue<string> queueChooseRole;
    //List<int> acceptSignUpPlayer = new List<int>();

    // 数据控制器
    private DataSaveController dataSaveController;

    // Use this for initialization
    private void Start()
    {
        dataSaveController = DataSaveController.Instance;
        InitButtons();
        queueChooseRole = Network.GetComponent<Server>().queueChooseRoleCommands;
    }

    private bool fullFlag = false;

    private void Update()
    {
        /*
        int clientConnectNum = Server.Instance.ClientConnectNum;
        // TODO:后期修改
        //**************************后期修改*******************************

        if (clientConnectNum == 4 && !fullFlag)
        {
            fullFlag = true;

            DataSaveController.Instance.playerNumber = clientConnectNum;

            JObject json = new JObject()
            {
                {"commMode", 1},                                // 新增通讯模式字段，0表示单播，1表示组播
                {"stageId", 1},                           // 正在哪个panel
                { "gId", 0},                              // 组ID： 0~3表示四个组
                { "uId", 0},                               // 用户ID：0或1， 0表示大将，1表示 马
                { "joystickAvailable", 1},      // 当前摇杆是否可以操控，0表示不行，1表示可以
                { "coolingTime", 0}                   //剩余冷却时间
            };
            string output = json.ToString();//JsonConvert.SerializeObject(json);

            //NetworkServer.SendToClient(connectionId[0], ServerMessageSend, msg);
            Server.Instance.SendCommand(output);
            SceneTransformer.Instance.TransferToNextScene();
        }
        */
        //**************************后期修改*******************************
    }

    public void TransferScene()
    {
        DataSaveController.Instance.playerNumber = 4;
        JObject json = new JObject()
            {
                {"commMode", 1},                // 新增通讯模式字段，0表示单播，1表示组播
                {"stageId", 1},                 // 正在哪个panel
                { "gId", 0},                    // 组ID： 0~3表示四个组
                { "uId", 0},                    // 用户ID：0或1， 0表示大将，1表示 马
                { "joystickAvailable", 1},      // 当前摇杆是否可以操控，0表示不行，1表示可以
                { "coolingTime", 0}             //剩余冷却时间
            };
        string output = json.ToString();//JsonConvert.SerializeObject(json);

        //NetworkServer.SendToClient(connectionId[0], ServerMessageSend, msg);
        Server.Instance.SendCommand(output);
        SceneTransformer.Instance.TransferToNextScene();
    }

    private void FixedUpdate()
    {
        handleChooseRoleCommand();

    }

    private void handleChooseRoleCommand()
    {

        if (queueChooseRole.Count > 0)
        {
            string input = queueChooseRole.Peek();
            queueChooseRole.Dequeue();
            Debug.Log(input);
            JObject JoysticMessage = JObject.Parse(input);
            groupID = (int)JoysticMessage["gId"];           // 0~3表示四个组
            userID = (int)JoysticMessage["uId"];            // 0或1， 0表示大将，1表示 马
            int connID = groupID * 2 + userID;              // 通过gId和uId计算出对应通道号
            //buttons[connID].interactable = true;            // 人物激活
            //acceptSignUpPlayer.Add(connID);
            buttons[connID].image.color = dataSaveController.groupColor[groupID];
        }
    }

    /************************************************
         * 按键
    ************************************************/
    private void InitButtons()
    {
        buttons = new Button[8];
        buttons[0] = Penguin_1P;
        buttons[1] = Pig_1P;
        buttons[2] = Penguin_2P;
        buttons[3] = Pig_2P;
        buttons[4] = Penguin_3P;
        buttons[5] = Pig_3P;
        buttons[6] = Penguin_4P;
        buttons[7] = Pig_4P;
        foreach (Button button in buttons)
        {
            //button.interactable = false;
            button.image.color = Color.white;   //默认白色
        }
    }

}
